using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ActionType
{
    Expand, Attack, Trade, DefensiveAlliance, Alliance, PeaceTreaty, BreakPeace,
    Consolidate, Destroyed, Rebellion, TradeMilestone, GoldenAge, StageTransition,
    JoinWar
}

public static class FactionBehavior
{
    // When non-null, ChooseAction appends a per-faction score breakdown
    // each tick. Enabled by UIController's logDecisions toggle.
    public static System.Text.StringBuilder TraceLog;

    // Wars must last this long before a faction will sue for peace, and a
    // fresh peace holds this long before anyone will break it. Prevents
    // peace/attack ping-pong loops between a strong and a weak neighbor.
    const int MinWarTicks = 8;
    const int MinPeaceTicks = 20;

    public static (ActionType action, Faction target) ChooseAction(
        Faction faction, List<Faction> allFactions, Cell[,] grid, int width, int height, System.Random rng, StageModifiers stage, int currentTick)
    {
        float bestScore = float.MinValue;
        ActionType bestAction = ActionType.Consolidate;
        Faction bestTarget = null;
        var candidates = TraceLog != null ? new List<(ActionType action, Faction target, float score)>() : null;

        void Consider(ActionType action, float score, Faction target)
        {
            candidates?.Add((action, target, score));
            if (score > bestScore)
            {
                bestScore = score;
                bestAction = action;
                bestTarget = target;
            }
        }

        int neutralBorderCount = CountNeutralBorderCells(faction, grid, width, height);
        Consider(ActionType.Expand, ScoreExpand(faction, neutralBorderCount) * stage.Expand, null);
        Consider(ActionType.Consolidate, ScoreConsolidate(faction) * stage.Consolidate, null);

        foreach (var neighbor in GetNeighborFactions(faction, allFactions, grid, width, height))
        {
            Consider(ActionType.Attack, ScoreAttack(faction, neighbor) * stage.Attack, neighbor);
            Consider(ActionType.Trade, ScoreTrade(faction, neighbor) * stage.Trade, neighbor);
            Consider(ActionType.Alliance, ScoreAlliance(faction, neighbor) * stage.Alliance, neighbor);
            Consider(ActionType.DefensiveAlliance, ScoreDefensiveAlliance(faction, neighbor) * stage.DefensiveAlliance, neighbor);
            Consider(ActionType.PeaceTreaty, ScorePeaceTreaty(faction, neighbor, currentTick) * stage.PeaceTreaty, neighbor);
            Consider(ActionType.BreakPeace, ScoreBreakPeace(faction, neighbor, currentTick) * stage.BreakPeace, neighbor);
        }

        if (candidates != null)
        {
            var parts = candidates
                .OrderByDescending(c => c.score)
                .Take(5)
                .Select(c => $"{c.action}{(c.target != null ? ">" + c.target.Name : "")}={c.score:0.00}");
            TraceLog.AppendLine($"  {faction.Name}: {string.Join(" | ", parts)}");
        }

        return (bestAction, bestTarget);
    }

    static float ScoreExpand(Faction f, int neutralBorderCount)
    {
        if (neutralBorderCount == 0) return 0f;
        return 0.4f + (1f - f.Aggression) * 0.3f;
    }

    static float ScoreConsolidate(Faction f) => (1f - f.Stability) * 0.6f;

    static float ScoreAttack(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.Alliance || rel == RelationState.DefensivePact) return 0f;
        if (rel == RelationState.Peace) return 0f; // must formally break the peace first

        float strengthRatio = f.Strength / target.Strength;
        float animosity = f.GetAnimosity(target.Id);

        float score = (f.Aggression * 0.4f + Mathf.Max(0f, strengthRatio - 1f) * 0.2f + animosity * 0.5f) * f.Archetype.AggressionMultiplier;
        if (rel == RelationState.War) score += 0.2f;
        return Mathf.Clamp01(score);
    }

    static float ScoreTrade(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.War) return 0f;
        float diminish = 1f - f.GetEconomicTies(target.Id) * 0.3f;
        return (0.3f + (1f - f.Aggression) * 0.3f) * f.Archetype.DiplomacyMultiplier * diminish;
    }

    static float ScoreAlliance(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.War || rel == RelationState.Alliance) return 0f;
        float tieBonus = f.GetEconomicTies(target.Id) * 0.4f;
        return ((1f - f.Aggression) * 0.3f + 0.2f + tieBonus) * f.Archetype.DiplomacyMultiplier;
    }

    static float ScoreDefensiveAlliance(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.War || rel == RelationState.Alliance || rel == RelationState.DefensivePact) return 0f;
        return 0.25f * f.Archetype.DiplomacyMultiplier;
    }

    static float ScorePeaceTreaty(Faction f, Faction target, int currentTick)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel != RelationState.War) return 0f;
        if (currentTick - f.GetRelationSince(target.Id) < MinWarTicks) return 0f;
        float strengthRatio = f.Strength / target.Strength;
        return Mathf.Clamp01((1f - strengthRatio) * 0.7f * (1f - f.Archetype.WarStubbornness));
    }

    static float ScoreBreakPeace(Faction f, Faction target, int currentTick)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel != RelationState.Peace) return 0f;
        if (currentTick - f.GetRelationSince(target.Id) < MinPeaceTicks) return 0f;
        return f.Aggression * 0.3f + f.Archetype.BetrayalChance;
    }

    static List<Faction> GetNeighborFactions(Faction f, List<Faction> all, Cell[,] grid, int width, int height)
    {
        var neighborIds = new HashSet<int>();
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        foreach (var (x, y) in f.Territory)
        {
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i], ny = y + dy[i];
                if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;
                int ownerId = grid[nx, ny].OwnerFactionId;
                if (ownerId != -1 && ownerId != f.Id) neighborIds.Add(ownerId);
            }
        }

        return all.Where(o => neighborIds.Contains(o.Id) && o.IsAlive && o.Id != f.Id).ToList();
    }

    static int CountNeutralBorderCells(Faction f, Cell[,] grid, int width, int height)
    {
        int count = 0;
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        var visited = new HashSet<(int, int)>();

        foreach (var (x, y) in f.Territory)
        {
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i], ny = y + dy[i];
                if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;
                if (grid[nx, ny].IsWater) continue;
                if (grid[nx, ny].OwnerFactionId == -1 && visited.Add((nx, ny))) count++;
            }
        }
        return count;
    }
}