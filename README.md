# Emergent Chronicles — Build & Run Instructions

AI Final Project, Group 24
Neo Jun Rong Bryan · See Wei Xuan · Low Jian Yuan Ethan · Vishal Velmurugan

A procedural world-history generator: utility-based faction agents act on a
generated map, and a template layer renders the resulting causally-linked
event history as a readable chronicle.

## Running the prebuilt demo

1. Open the `demo` folder.
2. Run `AI_Final_Project.exe` (Windows). No installation required; all
   assets are included in the folder.

### Using the demo

- **Seed field** (top left): optional. Type any text to reproduce a specific
  history; leave empty for a random one. The same seed always produces the
  same map, events, and chronicle on any machine.
- **Generate**: creates a world and plays its 200-year history in real time.
  The map (left) redraws as territory changes hands; the chronicle (right)
  is written live as events happen. Clicking Generate during playback
  restarts with a new world.
- **Skip >>**: fast-forwards the rest of the run and shows the final
  chronicle immediately. Only active during playback.
- **Legend** (below the map): each faction in its map color with its
  archetype; struck through when destroyed.

Suggested seed: `1651195363` — a war that ends in a peace treaty, grows
into a trade partnership, and ends with the two old enemies allied (this is
the run shown in Figure 2 of the paper).

After each completed run the demo writes `Output/latest_chronicle.txt`
(and, if the `Log Decisions` option is enabled on the GameManager object,
`Output/latest_decisions.txt` with per-tick utility scores) next to the
executable. These files are overwritten on every run.

## Building from source

Requirements:

- **Unity 6000.5.2f1** (any 6000.5.x should work). Install via Unity Hub.
  No paid assets or external packages are required beyond what the Unity
  Package Manager restores automatically (URP, TextMeshPro, Input System).

Steps:

1. In Unity Hub, choose **Add project from disk** and select the `source`
   folder (the one containing `Assets/`, `Packages/`, `ProjectSettings/`).
2. Open the project with Unity 6000.5.2f1. First import takes a few
   minutes while the Library folder is rebuilt.
3. Open the scene `Assets/Scenes/Main Ui.unity`.
4. **File → Build Profiles**: select the Windows profile (a Linux profile
   is also included at `Assets/Settings/Build Profiles/`). For other
   platforms, create a new profile — no settings need to change.
5. Set the build output folder to `Builds/<Platform>/` and click **Build**.
6. Run the produced executable. The `Output/` folder with chronicle
   exports is created next to the executable on first completed run.

To play in the editor instead of building: open `Main Ui.unity` and press
Play.

## Source layout

- `Assets/Sim/` — simulation: map generation, faction agents and utility
  scoring, scenario chains and stages, causal event log, lore templates.
- `Assets/Presentation/` — `UiController.cs`: UI wiring, playback loop,
  live chronicle streaming, legend, output export.
- `Assets/Scenes/Main Ui.unity` — the only scene.
