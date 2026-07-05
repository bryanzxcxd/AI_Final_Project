using System;

public class Cell
{
    public float Weight;
    public int OwnerFactionId = -1;
}

public static class MapGenerator
{
    public static Cell[,] Generate(int width, int height, Random rng)
    {
        var grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell
                {
                    Weight = (float)rng.NextDouble()
                };
            }
        return grid;
    }
}