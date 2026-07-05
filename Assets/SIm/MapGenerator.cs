using System;
using UnityEngine;

public class Cell
{
    public float Elevation;
    public float Fertility;
    public float Weight;
    public int OwnerFactionId = -1;
}

public static class MapGenerator
{
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
                float elevation = Mathf.PerlinNoise(
                    (x + offsetX) * scale,
                    (y + offsetY) * scale
                );

                float fertility = Mathf.PerlinNoise(
                    (x + offsetX + 500f) * scale,
                    (y + offsetY + 500f) * scale
                );

                grid[x, y] = new Cell
                {
                    Elevation = elevation,
                    Fertility = fertility,
                    Weight = fertility * (1f - Mathf.Abs(elevation - 0.5f))
                };
            }
        }

        return grid;
    }
}