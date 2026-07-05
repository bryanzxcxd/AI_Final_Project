using System;
using System.Collections;
using System.Linq;
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
    int numTicks = 200;
    public float secondsPerTick = 0.15f;

    WorldSim currentSim;
    Texture2D mapTexture;
    Coroutine playbackRoutine;

    void Start()
    {
        generateButton.onClick.AddListener(OnGenerateClicked);
        mapTexture = new Texture2D(mapWidth, mapHeight);
        mapTexture.filterMode = FilterMode.Point;
        mapDisplay.texture = mapTexture;
    }

    void OnGenerateClicked()
    {
        if (playbackRoutine != null) StopCoroutine(playbackRoutine);

        int seed = string.IsNullOrEmpty(seedInput.text)
            ? Guid.NewGuid().GetHashCode()
            : seedInput.text.GetHashCode();

        var rng = new System.Random(seed);
        var grid = MapGenerator.Generate(mapWidth, mapHeight, rng);
        currentSim = new WorldSim(grid, mapWidth, mapHeight, rng, factionCount);

        loreText.text = $"Seed used: {seed}\n\nSimulating history...";
        generateButton.interactable = false; // prevent spam-clicking mid-animation

        DrawMap(currentSim);

        playbackRoutine = StartCoroutine(PlaybackSimulation(seed));
    }

    IEnumerator PlaybackSimulation(int seed)
    {
        for (int i = 0; i < numTicks; i++)
        {
            currentSim.Tick();
            DrawMap(currentSim);
            yield return new WaitForSeconds(secondsPerTick);
        }

        // simulation finished, now build and show the full chronicle
        string chronicle = LoreTemplater.BuildChronicle(currentSim.History, currentSim.Factions);
        loreText.text = string.IsNullOrEmpty(chronicle)
            ? $"Seed used: {seed}\n\n(no significant events occurred)"
            : $"Seed used: {seed}\n\n{chronicle}";

        generateButton.interactable = true; // re-enable once done
    }

    void DrawMap(WorldSim sim)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Cell c = sim.Grid[x, y];
                Color col = c.OwnerFactionId >= 0
                    ? sim.Factions[c.OwnerFactionId].Color
                    : ElevationToColor(c.Elevation);
                mapTexture.SetPixel(x, y, col);
            }
        }
        mapTexture.Apply();
    }

    Color ElevationToColor(float e)
    {
        if (e < MapGenerator.WaterLevel) return new Color(0.1f, 0.3f, 0.6f);
        if (e < 0.45f) return new Color(0.8f, 0.75f, 0.5f);
        if (e < 0.7f) return new Color(0.2f, 0.6f, 0.2f);
        if (e < 0.85f) return new Color(0.5f, 0.4f, 0.3f);
        return Color.white;
    }
}