using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_InputField seedInput;
    public Button generateButton;
    public Button skipButton;
    public RawImage mapDisplay;
    public TMP_Text loreText;
    public TMP_Text legendText;

    int mapWidth = 64;
    int mapHeight = 64;
    int numTicks = 200;
    public float secondsPerTick = 0.15f;
    public bool exportChronicle = true;
    public bool logDecisions = false;

    WorldSim currentSim;
    Texture2D mapTexture;
    Coroutine playbackRoutine;
    StringBuilder liveChronicle;
    int lastHistoryIndex;
    bool skipRequested;
    ScrollRect chronicleScroll;

    void Start()
    {
        generateButton.onClick.AddListener(OnGenerateClicked);
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipToEnd);
            skipButton.interactable = false;
        }
        mapTexture = new Texture2D(mapWidth, mapHeight);
        mapTexture.filterMode = FilterMode.Point;
        mapDisplay.texture = mapTexture;
        ClearMap();
        chronicleScroll = loreText.GetComponentInParent<ScrollRect>(true);
    }

    void ClearMap()
    {
        var fill = new Color32(16, 20, 25, 255);
        var pixels = new Color32[mapWidth * mapHeight];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = fill;
        mapTexture.SetPixels32(pixels);
        mapTexture.Apply();
    }

    void OnGenerateClicked()
    {
        if (playbackRoutine != null) StopCoroutine(playbackRoutine);

        int seed = string.IsNullOrEmpty(seedInput.text)
            ? Guid.NewGuid().GetHashCode()
            : Fnv1aHash(seedInput.text);

        var rng = new System.Random(seed);
        var grid = MapGenerator.Generate(mapWidth, mapHeight, rng);
        var chain = ScenarioChain.PickRandom(rng);

        currentSim = new WorldSim(grid, mapWidth, mapHeight, rng, chain.FactionSeeds.Count, chain);
        currentSim.SetTotalTicks(numTicks);

        liveChronicle = new StringBuilder();
        liveChronicle.AppendLine($"Seed used: {seed}");
        liveChronicle.AppendLine();
        liveChronicle.AppendLine($"=== {chain.Theme} ===");
        liveChronicle.AppendLine(chain.WorldSummary);
        loreText.text = liveChronicle.ToString();

        lastHistoryIndex = 0;
        skipRequested = false;
        FactionBehavior.TraceLog = logDecisions ? new StringBuilder() : null;

        RefreshLegend();
        DrawMap(currentSim);

        if (skipButton != null) skipButton.interactable = true;
        playbackRoutine = StartCoroutine(PlaybackSimulation(seed));
    }

    // Fast-forwards the remaining ticks of the current playback.
    public void SkipToEnd()
    {
        skipRequested = true;
        if (skipButton != null) skipButton.interactable = false;
    }

    IEnumerator PlaybackSimulation(int seed)
    {
        for (int i = 0; i < numTicks; i++)
        {
            currentSim.Tick();

            if (!skipRequested)
            {
                AppendNewEvents();
                DrawMap(currentSim);
                yield return new WaitForSeconds(secondsPerTick);
            }
        }

        DrawMap(currentSim);

        string chronicle = LoreTemplater.BuildChronicle(currentSim.History, currentSim.Factions, numTicks, currentSim.Chain);
        loreText.text = $"Seed used: {seed}\n\n{chronicle}";
        RefreshLegend();
        if (chronicleScroll != null)
        {
            Canvas.ForceUpdateCanvases();
            chronicleScroll.verticalNormalizedPosition = 1f;
        }

        playbackRoutine = null;
        if (skipButton != null) skipButton.interactable = false;

        WriteOutputFiles(seed, chronicle);
    }

    void WriteOutputFiles(int seed, string chronicle)
    {
        if (!exportChronicle && FactionBehavior.TraceLog == null) return;
        try
        {
            string dir = System.IO.Path.Combine(Application.dataPath, "..", "Output");
            System.IO.Directory.CreateDirectory(dir);
            if (exportChronicle)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(dir, "latest_chronicle.txt"),
                    $"Seed: {seed}\n\n{chronicle}");
            }
            if (FactionBehavior.TraceLog != null)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(dir, "latest_decisions.txt"),
                    $"Seed: {seed}\n\n{FactionBehavior.TraceLog}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Could not write output files: {e.Message}");
        }
    }

    void RefreshLegend()
    {
        if (legendText == null || currentSim == null) return;
        var sb = new StringBuilder();
        foreach (var f in currentSim.Factions)
        {
            string hex = ColorUtility.ToHtmlStringRGB(f.Color);
            string entry = $"<color=#{hex}>{f.Name}</color>  <color=#6B7684>{f.Archetype.DisplayName}</color>";
            sb.AppendLine(f.IsAlive ? entry : $"<s>{entry}</s>");
        }
        legendText.text = sb.ToString();
    }

    void AppendNewEvents()
    {
        bool changed = false;
        for (; lastHistoryIndex < currentSim.History.Count; lastHistoryIndex++)
        {
            string line = LoreTemplater.DescribeLive(currentSim.History[lastHistoryIndex], currentSim.Factions);
            if (line != null)
            {
                liveChronicle.AppendLine(line);
                changed = true;
            }
        }
        if (changed)
        {
            loreText.text = liveChronicle.ToString();
            RefreshLegend();
            if (chronicleScroll != null)
            {
                Canvas.ForceUpdateCanvases();
                chronicleScroll.verticalNormalizedPosition = 0f;
            }
        }
    }

    static int Fnv1aHash(string s)
    {
        unchecked
        {
            uint hash = 2166136261;
            foreach (char c in s)
            {
                hash ^= c;
                hash *= 16777619;
            }
            return (int)hash;
        }
    }

    void DrawMap(WorldSim sim)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Cell c = sim.Grid[x, y];
                Color col = c.OwnerFactionId >= 0 ? sim.Factions[c.OwnerFactionId].Color : ElevationToColor(c.Elevation);
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
