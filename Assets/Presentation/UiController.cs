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
        tex.filterMode = FilterMode.Point; // crisp pixels, no blur

        for (int x = 0; x < mapWidth; x++)
            for (int y = 0; y < mapHeight; y++)
            {
                float w = grid[x, y].Weight;
                tex.SetPixel(x, y, new Color(w, w, w)); // grayscale by weight for now
            }
        tex.Apply();
        mapDisplay.texture = tex;
    }
}