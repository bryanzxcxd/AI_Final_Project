using System;
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
    System.Random rng;

    public WorldSim(Cell[,] grid, int width, int height, System.Random rng, int factionCount)
    {
        Grid = grid;
        Width = width;
        Height = height;
        this.rng = rng;
        Factions = FactionSpawner.SpawnFactions(grid, width, height, rng, factionCount);
    }

    public void RunFullHistory(int numTicks)
    {
        for (int i = 0; i < numTicks; i++)
        {
            Tick();
        }
    }

    public void Tick()
    {
        CurrentTick++;

        UpdateAnimosity();

        foreach (var faction in Factions.Where(f => f.IsAlive))
        {
            var (action, target) = FactionBehavior.ChooseAction(faction, Factions, Grid, Width, Height, rng);
            Debug.Log($"Tick {CurrentTick}: {faction.Name} chose {action}" + (target != null ? $" targeting {target.Name}" : " (no target)"));
            ApplyAction(faction, action, target);
        }
    }

    void UpdateAnimosity()
    {
        foreach (var faction in Factions.Where(f => f.IsAlive))
        {
            var borderingFactionIds = GetBorderingFactionIds(faction);

            foreach (int otherId in borderingFactionIds)
            {
                var rel = faction.Relations.GetValueOrDefault(otherId, RelationState.Neutral);
                if (rel == RelationState.Alliance || rel == RelationState.DefensivePact) continue;
                if (rel == RelationState.War) continue; // already at war, animosity no longer needs to build

                float current = faction.GetAnimosity(otherId);
                float growthRate = 0.01f; // tune this to control how fast tension builds
                faction.Animosity[otherId] = Mathf.Clamp01(current + growthRate);
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
    void ApplyAction(Faction faction, ActionType action, Faction target)
    {
        var evt = new SimEvent { Tick = CurrentTick, Action = action, FactionId = faction.Id, TargetFactionId = target?.Id };

        switch (action)
        {
            case ActionType.Expand:
                TryExpand(faction);
                break;

            case ActionType.Consolidate:
                faction.Stability = Math.Min(1f, faction.Stability + 0.1f);
                faction.Wealth += 2f;
                break;

            case ActionType.Attack:
                faction.Relations[target.Id] = RelationState.War;
                target.Relations[faction.Id] = RelationState.War;
                TryConquer(faction, target);
                break;

            case ActionType.Trade:
                faction.Wealth += 3f;
                target.Wealth += 3f;
                break;

            case ActionType.Alliance:
                faction.Relations[target.Id] = RelationState.Alliance;
                target.Relations[faction.Id] = RelationState.Alliance;
                faction.Animosity[target.Id] = 0f;
                target.Animosity[faction.Id] = 0f;
                break;

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
                faction.Relations[target.Id] = RelationState.War;
                target.Relations[faction.Id] = RelationState.War;
                break;
        }

        History.Add(evt);
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
        Grid[cellToTake.x, cellToTake.y].OwnerFactionId = attacker.Id;
        attacker.Territory.Add(cellToTake);
        defender.Territory.Remove(cellToTake);

        if (defender.Territory.Count == 0 && defender.IsAlive)
        {
            defender.IsAlive = false;
            History.Add(new SimEvent
            {
                Tick = CurrentTick,
                Action = ActionType.Destroyed,
                FactionId = defender.Id,
                TargetFactionId = attacker.Id // who delivered the final blow
            });
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
                if (Grid[nx, ny].OwnerFactionId != faction.Id)
                {
                    borders.Add((nx, ny));
                }
            }
        }
        return borders.ToList();
    }

    IEnumerable<(int x, int y)> GetNeighborCoords((int x, int y) cell)
    {
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        for (int i = 0; i < 4; i++)
            yield return (cell.x + dx[i], cell.y + dy[i]);
    }

    bool IsInBounds((int x, int y) c) => c.x >= 0 && c.y >= 0 && c.x < Width && c.y < Height;
}