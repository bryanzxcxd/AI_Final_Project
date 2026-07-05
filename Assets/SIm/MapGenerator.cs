using System;
using UnityEngine;

public class Cell
{
    public float Elevation;
    public float Fertility;
    public float Weight;       // desirability score, -1 = unclaimable
    public bool IsWater;
    public int OwnerFactionId = -1;
}

public static class MapGenerator
{
    public const float WaterLevel = 0.4f; // single source of truth

    public static Cell[,] Generate(int width, int height, System.Random rng)
    {
        var grid = new Cell[width, height];

        float offsetX = (float)rng.NextDouble() * 10000f;
        float offsetY = (float)rng.NextDouble() * 10000f;
        float scale = 0.08f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float elevation = Mathf.PerlinNoise((x + offsetX) * scale, (y + offsetY) * scale);
                float fertility = Mathf.PerlinNoise((x + offsetX + 500f) * scale, (y + offsetY + 500f) * scale);

                bool isWater = elevation < WaterLevel;

                float weight;
                if (isWater)
                {
                    weight = -1f; // unclaimable, excluded everywhere by construction
                }
                else
                {
                    // prefer fertile, mid-elevation land; mountains/extreme elevation are still claimable but less desirable
                    weight = fertility * (1f - Mathf.Abs(elevation - 0.55f));
                }

                grid[x, y] = new Cell
                {
                    Elevation = elevation,
                    Fertility = fertility,
                    IsWater = isWater,
                    Weight = weight
                };
            }
        }

        return grid;
    }
}