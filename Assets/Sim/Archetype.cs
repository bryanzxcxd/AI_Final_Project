public enum ArchetypeType
{
    MilitaryRemnant, SurvivorEnclave, CorporateState, ResearchCollective,
    RaiderSyndicate, ZealousCongregation, DoomsdayCult, FortifiedFreehold
}

public class Archetype
{
    public ArchetypeType Type;
    public string DisplayName;
    public float AggressionMultiplier;
    public float DiplomacyMultiplier;
    public float WarStubbornness;
    public float BetrayalChance;

    public static Archetype Get(ArchetypeType type)
    {
        switch (type)
        {
            case ArchetypeType.MilitaryRemnant:
                return new Archetype { Type = type, DisplayName = "Military Remnant", AggressionMultiplier = 1.4f, DiplomacyMultiplier = 0.6f, WarStubbornness = 0.6f, BetrayalChance = 0.02f };
            case ArchetypeType.SurvivorEnclave:
                return new Archetype { Type = type, DisplayName = "Survivor Enclave", AggressionMultiplier = 0.3f, DiplomacyMultiplier = 0.8f, WarStubbornness = 0.7f, BetrayalChance = 0.0f };
            case ArchetypeType.CorporateState:
                return new Archetype { Type = type, DisplayName = "Corporate State", AggressionMultiplier = 0.5f, DiplomacyMultiplier = 1.6f, WarStubbornness = 0.1f, BetrayalChance = 0.05f };
            case ArchetypeType.ResearchCollective:
                return new Archetype { Type = type, DisplayName = "Research Collective", AggressionMultiplier = 0.4f, DiplomacyMultiplier = 1.3f, WarStubbornness = 0.2f, BetrayalChance = 0.0f };
            case ArchetypeType.RaiderSyndicate:
                return new Archetype { Type = type, DisplayName = "Raider Syndicate", AggressionMultiplier = 1.6f, DiplomacyMultiplier = 0.3f, WarStubbornness = 0.3f, BetrayalChance = 0.1f };
            case ArchetypeType.ZealousCongregation:
                return new Archetype { Type = type, DisplayName = "Zealous Congregation", AggressionMultiplier = 1.3f, DiplomacyMultiplier = 0.4f, WarStubbornness = 0.9f, BetrayalChance = 0.0f };
            case ArchetypeType.DoomsdayCult:
                return new Archetype { Type = type, DisplayName = "Doomsday Cult", AggressionMultiplier = 1.2f, DiplomacyMultiplier = 0.5f, WarStubbornness = 0.95f, BetrayalChance = 0.01f };
            case ArchetypeType.FortifiedFreehold:
                return new Archetype { Type = type, DisplayName = "Fortified Freehold", AggressionMultiplier = 0.6f, DiplomacyMultiplier = 1.2f, WarStubbornness = 0.5f, BetrayalChance = 0.0f };
            default:
                return new Archetype { Type = type, DisplayName = "Unknown", AggressionMultiplier = 1f, DiplomacyMultiplier = 1f, WarStubbornness = 0.5f, BetrayalChance = 0.02f };
        }
    }
}
