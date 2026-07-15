using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldSim
{
    public Cell[,] Grid;
    public int Width, Height;
    public List<Faction> Factions;
    public List<SimEvent> History = new List<SimEvent>();
    public int CurrentTick = 0;
    public ScenarioChain Chain;

    System.Random rng;
    Dictionary<(int, int), SimEvent> lastSignificantEvent = new Dictionary<(int, int), SimEvent>();
    int lastStageIndex = -1;
    int totalTicksForStages = 1;
    StageModifiers currentStageModifiers = new StageModifiers();

    public WorldSim(Cell[,] grid, int width, int height, System.Random rng, int factionCount, ScenarioChain chain)
    {
        Grid = grid;
        Width = width;
        Height = height;
        this.rng = rng;
        Chain = chain;
        Factions = FactionSpawner.SpawnFactions(grid, width, height, rng, factionCount, chain);
    }

    public void SetTotalTicks(int total) => totalTicksForStages = total;

    public void RunFullHistory(int numTicks)
    {
        totalTicksForStages = numTicks;
        for (int i = 0; i < numTicks; i++) Tick();
    }

    public void Tick()
    {
        CurrentTick++;
        CheckStageTransition();
        FactionBehavior.TraceLog?.AppendLine($"--- Tick {CurrentTick} ({Chain.Stages[lastStageIndex].Name}) ---");
        UpdateAnimosity();
        CheckRebellions();

        foreach (var faction in Factions.Where(f => f.IsAlive))
        {
            var (action, target) = FactionBehavior.ChooseAction(faction, Factions, Grid, Width, Height, rng, currentStageModifiers);
            if (target != null && target.Id == faction.Id) continue;
            ApplyAction(faction, action, target);
        }
    }

    (int, int) PairKey(int a, int b) => a < b ? (a, b) : (b, a);

    bool IsSignificant(ActionType action)
    {
        return action == ActionType.Attack || action == ActionType.Alliance ||
               action == ActionType.DefensiveAlliance || action == ActionType.PeaceTreaty ||
               action == ActionType.BreakPeace;
    }

    void ApplyAction(Faction faction, ActionType action, Faction target)
    {
        var evt = new SimEvent { Tick = CurrentTick, Action = action, FactionId = faction.Id, TargetFactionId = target?.Id };
        bool alreadyLogged = false;

        switch (action)
        {
            case ActionType.Expand:
                TryExpand(faction);
                break;

            case ActionType.Consolidate:
                faction.Stability = Mathf.Min(1f, faction.Stability + 0.1f);
                faction.Wealth += 2f;
                break;

            case ActionType.Attack:
                {
                    var priorRelation = faction.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
                    bool isNewWar = priorRelation != RelationState.War;
                    faction.Relations[target.Id] = RelationState.War;
                    target.Relations[faction.Id] = RelationState.War;

                    if (isNewWar)
                    {
                        var key = PairKey(faction.Id, target.Id);
                        if (lastSignificantEvent.TryGetValue(key, out var priorEvt) &&
                            (priorEvt.Action == ActionType.BreakPeace || priorEvt.Action == ActionType.PeaceTreaty))
                        {
                            evt.CausedBy = priorEvt;
                        }
                        History.Add(evt);
                        lastSignificantEvent[key] = evt;
                        PullAlliesIntoWar(faction, target, evt);
                    }
                    alreadyLogged = true;
                    TryConquer(faction, target);
                    break;
                }

            case ActionType.Trade:
                {
                    var key = PairKey(faction.Id, target.Id);
                    faction.Wealth += 3f;
                    target.Wealth += 3f;

                    float tiesBefore = faction.GetEconomicTies(target.Id);
                    float tiesAfter = Mathf.Clamp01(tiesBefore + 0.05f);
                    faction.EconomicTies[target.Id] = tiesAfter;
                    target.EconomicTies[faction.Id] = tiesAfter;

                    bool isFirstTrade = tiesBefore == 0f;
                    bool crossedHalfway = tiesBefore < 0.5f && tiesAfter >= 0.5f;

                    if (isFirstTrade)
                    {
                        History.Add(evt);
                        lastSignificantEvent[key] = evt;
                    }
                    else if (crossedHalfway)
                    {
                        var milestoneEvt = new SimEvent { Tick = CurrentTick, Action = ActionType.TradeMilestone, FactionId = faction.Id, TargetFactionId = target.Id };
                        if (lastSignificantEvent.TryGetValue(key, out var firstTrade)) milestoneEvt.CausedBy = firstTrade;
                        History.Add(milestoneEvt);
                        lastSignificantEvent[key] = milestoneEvt;
                    }
                    alreadyLogged = true;
                    break;
                }

            case ActionType.Alliance:
                {
                    var key = PairKey(faction.Id, target.Id);
                    faction.Relations[target.Id] = RelationState.Alliance;
                    target.Relations[faction.Id] = RelationState.Alliance;
                    faction.Animosity[target.Id] = 0f;
                    target.Animosity[faction.Id] = 0f;

                    if (lastSignificantEvent.TryGetValue(key, out var priorTradeEvt) &&
                        (priorTradeEvt.Action == ActionType.Trade || priorTradeEvt.Action == ActionType.TradeMilestone))
                    {
                        evt.CausedBy = priorTradeEvt;
                    }
                    break;
                }

            case ActionType.DefensiveAlliance:
                faction.Relations[target.Id] = RelationState.DefensivePact;
                target.Relations[faction.Id] = RelationState.DefensivePact;
                break;

            case ActionType.PeaceTreaty:
                faction.Relations[target.Id] = RelationState.Peace;
                target.Relations[faction.Id] = RelationState.Peace;
                faction.Animosity[target.Id] = 0f;
                target.Animosity[faction.Id] = 0f;
                break;

            case ActionType.BreakPeace:
                {
                    var key = PairKey(faction.Id, target.Id);
                    faction.Relations[target.Id] = RelationState.War;
                    target.Relations[faction.Id] = RelationState.War;
                    if (lastSignificantEvent.TryGetValue(key, out var oldPeace) && oldPeace.Action == ActionType.PeaceTreaty)
                    {
                        evt.CausedBy = oldPeace;
                    }
                    break;
                }
        }

        if (!alreadyLogged)
        {
            if (target != null && IsSignificant(action))
            {
                lastSignificantEvent[PairKey(faction.Id, target.Id)] = evt;
            }
            History.Add(evt);
        }
    }

    void PullAlliesIntoWar(Faction attacker, Faction defender, SimEvent attackEvt)
    {
        foreach (var ally in Factions)
        {
            if (!ally.IsAlive || ally.Id == attacker.Id || ally.Id == defender.Id) continue;

            var relToDefender = ally.Relations.GetValueOrDefault(defender.Id, RelationState.Neutral);
            if (relToDefender != RelationState.Alliance && relToDefender != RelationState.DefensivePact) continue;

            var relToAttacker = ally.Relations.GetValueOrDefault(attacker.Id, RelationState.Neutral);
            if (relToAttacker == RelationState.War || relToAttacker == RelationState.Alliance ||
                relToAttacker == RelationState.DefensivePact) continue;

            ally.Relations[attacker.Id] = RelationState.War;
            attacker.Relations[ally.Id] = RelationState.War;

            var joinEvt = new SimEvent { Tick = CurrentTick, Action = ActionType.JoinWar, FactionId = ally.Id, TargetFactionId = attacker.Id, CausedBy = attackEvt };
            History.Add(joinEvt);
            lastSignificantEvent[PairKey(ally.Id, attacker.Id)] = joinEvt;
        }
    }

    void TryExpand(Faction faction)
    {
        var borderCells = GetBorderCells(faction);
        if (borderCells.Count == 0) return;

        var candidates = borderCells
            .Where(c => Grid[c.x, c.y].OwnerFactionId == -1)
            .OrderByDescending(c => Grid[c.x, c.y].Weight)
            .Take(3);

        foreach (var c in candidates)
        {
            Grid[c.x, c.y].OwnerFactionId = faction.Id;
            faction.Territory.Add((c.x, c.y));
        }
    }

    void TryConquer(Faction attacker, Faction defender)
    {
        if (attacker.Strength <= defender.Strength) return;

        var contested = attacker.Territory
            .SelectMany(GetNeighborCoords)
            .Where(c => IsInBounds(c) && Grid[c.x, c.y].OwnerFactionId == defender.Id)
            .ToList();

        if (contested.Count == 0) return;

        var cellToTake = contested[rng.Next(contested.Count)];
        var cell = Grid[cellToTake.x, cellToTake.y];

        if (cell.OriginalOwnerFactionId == -1) cell.OriginalOwnerFactionId = defender.Id;

        cell.OwnerFactionId = attacker.Id;
        attacker.Territory.Add(cellToTake);
        defender.Territory.Remove(cellToTake);

        if (defender.Territory.Count == 0 && defender.IsAlive)
        {
            defender.IsAlive = false;
            History.Add(new SimEvent { Tick = CurrentTick, Action = ActionType.Destroyed, FactionId = defender.Id, TargetFactionId = attacker.Id });
        }
    }

    void UpdateAnimosity()
    {
        foreach (var faction in Factions.Where(f => f.IsAlive))
        {
            var borderingFactionIds = GetBorderingFactionIds(faction);
            foreach (int otherId in borderingFactionIds)
            {
                if (otherId == faction.Id) continue;
                var rel = faction.Relations.GetValueOrDefault(otherId, RelationState.Neutral);
                if (rel == RelationState.Alliance || rel == RelationState.DefensivePact || rel == RelationState.War) continue;
                float current = faction.GetAnimosity(otherId);
                faction.Animosity[otherId] = Mathf.Clamp01(current + 0.01f);
            }
        }
    }

    HashSet<int> GetBorderingFactionIds(Faction faction)
    {
        var neighborIds = new HashSet<int>();
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        foreach (var (x, y) in faction.Territory)
        {
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i], ny = y + dy[i];
                if (nx < 0 || ny < 0 || nx >= Width || ny >= Height) continue;
                int ownerId = Grid[nx, ny].OwnerFactionId;
                if (ownerId != -1 && ownerId != faction.Id) neighborIds.Add(ownerId);
            }
        }
        return neighborIds;
    }

    void CheckRebellions()
    {
        foreach (var faction in Factions.Where(f => f.IsAlive))
        {
            var conqueredCells = faction.Territory
                .Where(c => Grid[c.x, c.y].OriginalOwnerFactionId != -1 && Grid[c.x, c.y].OriginalOwnerFactionId != faction.Id)
                .ToList();

            if (conqueredCells.Count == 0) continue;

            float rebellionChance = (1f - faction.Stability) * 0.02f * conqueredCells.Count;
            if (rng.NextDouble() < rebellionChance)
            {
                var cell = conqueredCells[rng.Next(conqueredCells.Count)];
                int originalOwnerId = Grid[cell.x, cell.y].OriginalOwnerFactionId;

                faction.Territory.Remove(cell);
                faction.Stability = Mathf.Clamp01(faction.Stability - 0.15f);
                Grid[cell.x, cell.y].OriginalOwnerFactionId = -1;

                var originalOwner = Factions.FirstOrDefault(f => f.Id == originalOwnerId);
                if (originalOwner != null && originalOwner.IsAlive)
                {
                    Grid[cell.x, cell.y].OwnerFactionId = originalOwnerId;
                    originalOwner.Territory.Add(cell);
                }
                else
                {
                    Grid[cell.x, cell.y].OwnerFactionId = -1;
                }

                var rebellionEvent = new SimEvent { Tick = CurrentTick, Action = ActionType.Rebellion, FactionId = faction.Id, TargetFactionId = originalOwnerId };
                var key = PairKey(faction.Id, originalOwnerId);
                if (lastSignificantEvent.TryGetValue(key, out var priorEvt)) rebellionEvent.CausedBy = priorEvt;

                History.Add(rebellionEvent);
            }
        }
    }

    void CheckStageTransition()
    {
        int stageCount = Chain.Stages.Count;
        float progress = (float)CurrentTick / totalTicksForStages;
        int currentStageIndex = Mathf.Min(stageCount - 1, Mathf.FloorToInt(progress * stageCount));

        if (currentStageIndex != lastStageIndex)
        {
            lastStageIndex = currentStageIndex;
            var stage = Chain.Stages[currentStageIndex];
            currentStageModifiers = StageModifiers.Get(stage.Mood);
            History.Add(new SimEvent { Tick = CurrentTick, Action = ActionType.StageTransition, FactionId = -1, Description = stage.Name });
        }
    }

    List<(int x, int y)> GetBorderCells(Faction faction)
    {
        var borders = new HashSet<(int, int)>();
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        foreach (var (x, y) in faction.Territory)
        {
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                if (nx < 0 || ny < 0 || nx >= Width || ny >= Height) continue;
                if (Grid[nx, ny].IsWater) continue;
                if (Grid[nx, ny].OwnerFactionId != faction.Id) borders.Add((nx, ny));
            }
        }
        return borders.ToList();
    }

    IEnumerable<(int x, int y)> GetNeighborCoords((int x, int y) cell)
    {
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        for (int i = 0; i < 4; i++) yield return (cell.x + dx[i], cell.y + dy[i]);
    }

    bool IsInBounds((int x, int y) c) => c.x >= 0 && c.y >= 0 && c.x < Width && c.y < Height;
}