using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_InputField seedInput;
    public Button generateButton;
    public RawImage mapDisplay;
    public TMP_Text loreText;

    int mapWidth = 64;
    int mapHeight = 64;
    int factionCount = 6;
    int numTicks = 50;

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

        var sim = new WorldSim(grid, mapWidth, mapHeight, rng, factionCount);
        sim.RunFullHistory(numTicks);

        DrawMap(sim);

        string chronicle = LoreTemplater.BuildChronicle(sim.History, sim.Factions);
        loreText.text = string.IsNullOrEmpty(chronicle)
            ? $"Seed used: {seed}\n(no significant events occurred)"
            : $"Seed used: {seed}\n\n{chronicle}";
    }

    void DrawMap(WorldSim sim)
    {
        Texture2D tex = new Texture2D(mapWidth, mapHeight);
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Cell c = sim.Grid[x, y];
                Color col = c.OwnerFactionId >= 0
                    ? sim.Factions[c.OwnerFactionId].Color
                    : ElevationToColor(c.Elevation);
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        mapDisplay.texture = tex;
    }

    Color ElevationToColor(float e)
    {
        if (e < MapGenerator.WaterLevel) return new Color(0.1f, 0.3f, 0.6f);   // water
        if (e < 0.45f) return new Color(0.8f, 0.75f, 0.5f);                    // beach/sand
        if (e < 0.7f) return new Color(0.2f, 0.6f, 0.2f);                     // grass/land
        if (e < 0.85f) return new Color(0.5f, 0.4f, 0.3f);                     // hills
        return Color.white;                                                    // mountains/snow
    }
}