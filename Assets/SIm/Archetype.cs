public enum ArchetypeType
{
    ImperialDominion, MerchantCoalition, HolyTheocracy, NorthernKhanate,
    ForestConfederacy, DwarvenMountainholds, UndeadKingdom, FreeholdRepublic
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
            case ArchetypeType.ImperialDominion:
                return new Archetype { Type = type, DisplayName = "Imperial Dominion", AggressionMultiplier = 1.4f, DiplomacyMultiplier = 0.6f, WarStubbornness = 0.6f, BetrayalChance = 0.02f };
            case ArchetypeType.MerchantCoalition:
                return new Archetype { Type = type, DisplayName = "Merchant Coalition", AggressionMultiplier = 0.5f, DiplomacyMultiplier = 1.6f, WarStubbornness = 0.1f, BetrayalChance = 0.05f };
            case ArchetypeType.HolyTheocracy:
                return new Archetype { Type = type, DisplayName = "Holy Theocracy", AggressionMultiplier = 1.3f, DiplomacyMultiplier = 0.4f, WarStubbornness = 0.9f, BetrayalChance = 0.0f };
            case ArchetypeType.NorthernKhanate:
                return new Archetype { Type = type, DisplayName = "Northern Khanate", AggressionMultiplier = 1.6f, DiplomacyMultiplier = 0.3f, WarStubbornness = 0.3f, BetrayalChance = 0.1f };
            case ArchetypeType.ForestConfederacy:
                return new Archetype { Type = type, DisplayName = "Forest Confederacy", AggressionMultiplier = 0.3f, DiplomacyMultiplier = 0.8f, WarStubbornness = 0.7f, BetrayalChance = 0.0f };
            case ArchetypeType.DwarvenMountainholds:
                return new Archetype { Type = type, DisplayName = "Dwarven Mountainholds", AggressionMultiplier = 0.4f, DiplomacyMultiplier = 0.9f, WarStubbornness = 0.8f, BetrayalChance = 0.03f };
            case ArchetypeType.UndeadKingdom:
                return new Archetype { Type = type, DisplayName = "Undead Kingdom", AggressionMultiplier = 1.2f, DiplomacyMultiplier = 0.5f, WarStubbornness = 0.95f, BetrayalChance = 0.01f };
            case ArchetypeType.FreeholdRepublic:
                return new Archetype { Type = type, DisplayName = "Freehold Republic", AggressionMultiplier = 0.6f, DiplomacyMultiplier = 1.3f, WarStubbornness = 0.2f, BetrayalChance = 0.0f };
            default:
                return new Archetype { Type = type, DisplayName = "Unknown", AggressionMultiplier = 1f, DiplomacyMultiplier = 1f, WarStubbornness = 0.5f, BetrayalChance = 0.02f };
        }
    }
}