public enum StageMood { Turmoil, Expansion, Conflict, Commerce, Diplomacy }

public class Stage
{
    public string Name;
    public StageMood Mood;

    public Stage(string name, StageMood mood)
    {
        Name = name;
        Mood = mood;
    }
}

public class StageModifiers
{
    public float Expand = 1f;
    public float Consolidate = 1f;
    public float Attack = 1f;
    public float Trade = 1f;
    public float Alliance = 1f;
    public float DefensiveAlliance = 1f;
    public float PeaceTreaty = 1f;
    public float BreakPeace = 1f;

    public static StageModifiers Get(StageMood mood)
    {
        switch (mood)
        {
            case StageMood.Turmoil:
                return new StageModifiers { Expand = 1.4f, Consolidate = 1.3f, Attack = 0.8f, Trade = 0.5f, Alliance = 0.5f, DefensiveAlliance = 0.8f, PeaceTreaty = 1f, BreakPeace = 0.8f };
            case StageMood.Expansion:
                return new StageModifiers { Expand = 1.5f, Consolidate = 0.8f, Attack = 1f, Trade = 0.8f, Alliance = 0.7f, DefensiveAlliance = 0.9f, PeaceTreaty = 1f, BreakPeace = 1f };
            case StageMood.Conflict:
                return new StageModifiers { Expand = 0.7f, Consolidate = 0.8f, Attack = 1.6f, Trade = 0.4f, Alliance = 0.6f, DefensiveAlliance = 1.2f, PeaceTreaty = 0.5f, BreakPeace = 1.5f };
            case StageMood.Commerce:
                return new StageModifiers { Expand = 0.7f, Consolidate = 1f, Attack = 0.5f, Trade = 1.7f, Alliance = 1.2f, DefensiveAlliance = 1f, PeaceTreaty = 1.4f, BreakPeace = 0.4f };
            case StageMood.Diplomacy:
                return new StageModifiers { Expand = 0.7f, Consolidate = 1f, Attack = 0.4f, Trade = 1.2f, Alliance = 1.7f, DefensiveAlliance = 1.5f, PeaceTreaty = 1.6f, BreakPeace = 0.3f };
            default:
                return new StageModifiers();
        }
    }
}
