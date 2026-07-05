using System.Collections.Generic;
using UnityEngine;

public static class FactionSpawner
{
    public static List<Faction> SpawnFactions(Cell[,] grid, int width, int height, System.Random rng, int count)
    {
        var factions = new List<Faction>();
        var usedSpots = new List<(int, int)>();
        int minDistance = Mathf.Max(width, height) / (count + 1);

        int attempts = 0;
        while (factions.Count < count && attempts < count * 200)
        {
            attempts++;
            int x = rng.Next(0, width);
            int y = rng.Next(0, height);

            if (grid[x, y].IsWater) continue;

            bool tooClose = false;
            foreach (var spot in usedSpots)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(spot.Item1, spot.Item2));
                if (dist < minDistance) { tooClose = true; break; }
            }
            if (tooClose) continue;

            var faction = new Faction
            {
                Id = factions.Count,
                Name = NameGenerator.GenerateKingdomName(rng),
                Color = new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()),
                Capital = (x, y),
                Aggression = (float)rng.NextDouble(),
                Wealth = 10f,
                Stability = 1f
            };
            faction.Territory.Add((x, y));
            grid[x, y].OwnerFactionId = faction.Id;

            factions.Add(faction);
            usedSpots.Add((x, y));
        }

        return factions;
    }
}