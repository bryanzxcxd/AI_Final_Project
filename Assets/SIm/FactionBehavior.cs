using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ActionType
{
    Expand,
    Attack,
    Trade,
    DefensiveAlliance,
    Alliance,
    PeaceTreaty,
    BreakPeace,
    Consolidate,
    Destroyed
}

public static class FactionBehavior
{

    public static (ActionType action, Faction target) ChooseAction(
        Faction faction, List<Faction> allFactions, Cell[,] grid, int width, int height, System.Random rng)
    {

        var scores = new Dictionary<ActionType, float>();
        Faction bestNeighbor = GetStrongestNeighbor(faction, allFactions, grid, width, height);
        int neutralBorderCount = CountNeutralBorderCells(faction, grid, width, height);

        scores[ActionType.Expand] = ScoreExpand(faction, neutralBorderCount);
        scores[ActionType.Consolidate] = ScoreConsolidate(faction);

        if (bestNeighbor != null)
        {
            scores[ActionType.Attack] = ScoreAttack(faction, bestNeighbor);
            scores[ActionType.Trade] = ScoreTrade(faction, bestNeighbor);
            scores[ActionType.Alliance] = ScoreAlliance(faction, bestNeighbor);
            scores[ActionType.DefensiveAlliance] = ScoreDefensiveAlliance(faction, bestNeighbor);
            scores[ActionType.PeaceTreaty] = ScorePeaceTreaty(faction, bestNeighbor);
            scores[ActionType.BreakPeace] = ScoreBreakPeace(faction, bestNeighbor);
        }

        var best = scores.OrderByDescending(kv => kv.Value).First();
        return (best.Key, bestNeighbor);
    }

    static float ScoreExpand(Faction f, int neutralBorderCount)
    {
        if (neutralBorderCount == 0) return 0f; // nothing left to claim, force other actions to compete
        return 0.4f + (1f - f.Aggression) * 0.3f;
    }

    static float ScoreConsolidate(Faction f) => (1f - f.Stability) * 0.6f;

    static float ScoreAttack(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.Alliance || rel == RelationState.DefensivePact) return 0f;

        float strengthRatio = f.Strength / target.Strength;
        float animosity = f.GetAnimosity(target.Id);

        float score = f.Aggression * 0.4f
                    + Mathf.Abs(strengthRatio - 1f) * 0.2f
                    + animosity * 0.5f; // animosity now a major driver of war

        if (rel == RelationState.War) score += 0.2f;
        return Mathf.Clamp01(score);
    }

    static float ScoreTrade(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.War) return 0f;
        return 0.3f + (1f - f.Aggression) * 0.3f;
    }

    static float ScoreAlliance(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.War || rel == RelationState.Alliance) return 0f;
        return (1f - f.Aggression) * 0.3f + 0.2f;
    }

    static float ScoreDefensiveAlliance(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel == RelationState.War || rel == RelationState.Alliance) return 0f;
        return 0.25f;
    }

    static float ScorePeaceTreaty(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel != RelationState.War) return 0f;
        float strengthRatio = f.Strength / target.Strength;
        return Mathf.Clamp01((1f - strengthRatio) * 0.7f);
    }

    static float ScoreBreakPeace(Faction f, Faction target)
    {
        var rel = f.Relations.GetValueOrDefault(target.Id, RelationState.Neutral);
        if (rel != RelationState.Peace) return 0f;
        return f.Aggression * 0.3f;
    }

    static Faction GetStrongestNeighbor(Faction f, List<Faction> all, Cell[,] grid, int width, int height)
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

        return all.Where(o => neighborIds.Contains(o.Id) && o.IsAlive)
                  .OrderByDescending(o => o.Strength)
                  .FirstOrDefault();
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