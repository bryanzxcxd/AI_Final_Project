using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public TMP_InputField seedInput;
    public Button generateButton;
    public RawImage mapDisplay;
    public TMP_Text loreText;

    int mapWidth = 64;
    int mapHeight = 64;

    void Start()
    {
        generateButton.onClick.AddListener(OnGenerateClicked);
    }

    void OnGenerateClicked()
    {
        int seed = string.IsNullOrEmpty(seedInput.text)
            ? Guid.NewGuid().GetHashCode()
            : seedInput.text.GetHashCode();

        var rng = new System.Random(seed);
        var grid = MapGenerator.Generate(mapWidth, mapHeight, rng);

        DrawMap(grid);
        loreText.text = $"Seed used: {seed}\n(history generation not implemented yet)";
    }

    void DrawMap(Cell[,] grid)
    {
        Texture2D tex = new Texture2D(mapWidth, mapHeight);
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Cell c = grid[x, y];
                Color col = ElevationToColor(c.Elevation);
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        mapDisplay.texture = tex;
    }

    Color ElevationToColor(float e)
    {
        if (e < 0.35f) return new Color(0.1f, 0.3f, 0.6f);   // water
        if (e < 0.45f) return new Color(0.8f, 0.75f, 0.5f);  // beach/sand
        if (e < 0.7f) return new Color(0.2f, 0.6f, 0.2f);    // grass/land
        if (e < 0.85f) return new Color(0.5f, 0.4f, 0.3f);   // hills
        return Color.white;                                  // mountains/snow
    }
}