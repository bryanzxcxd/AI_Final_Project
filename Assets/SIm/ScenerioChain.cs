using System.Collections.Generic;

public class FactionSeed
{
    public string Name;
    public ArchetypeType Archetype;

    public FactionSeed(string name, ArchetypeType archetype)
    {
        Name = name;
        Archetype = archetype;
    }
}

public class ScenarioChain
{
    public string Theme;
    public string WorldSummary;
    public List<Stage> Stages;
    public List<FactionSeed> FactionSeeds;

    public static List<ScenarioChain> All = new List<ScenarioChain> {
        new ScenarioChain {
            Theme = "The Great Collapse",
            WorldSummary = "A rapidly spreading outbreak causes governments to collapse. Isolated military commands become independent powers while survivors establish fortified settlements. As food, fuel, and medicine become scarce, conflicts erupt over resources. Eventually, neighboring factions begin trading to improve their chances of survival.",
            Stages = new List<Stage> {
                new Stage("Government Collapse", StageMood.Turmoil),
                new Stage("Military Fragmentation", StageMood.Turmoil),
                new Stage("Independent Survivor Settlements", StageMood.Expansion),
                new Stage("Resource War", StageMood.Conflict),
                new Stage("Trade Networks", StageMood.Commerce)
            },
            FactionSeeds = new List<FactionSeed> {
                new FactionSeed("Northern Command", ArchetypeType.MilitaryRemnant),
                new FactionSeed("Haven City", ArchetypeType.SurvivorEnclave),
                new FactionSeed("River Coalition", ArchetypeType.FortifiedFreehold),
                new FactionSeed("Iron Dominion", ArchetypeType.RaiderSyndicate),
                new FactionSeed("Frontier Republic", ArchetypeType.FortifiedFreehold)
            }
        },
        new ScenarioChain {
            Theme = "Corporate Ascension",
            WorldSummary = "A failed pharmaceutical experiment triggers the apocalypse. Governments collapse while major corporations seal themselves inside secure facilities. Years later, corporate city-states emerge and compete through research, technology, and trade while searching for a cure - or an advantage.",
            Stages = new List<Stage> {
                new Stage("Pharmaceutical Disaster", StageMood.Turmoil),
                new Stage("Government Collapse", StageMood.Turmoil),
                new Stage("Corporate City-States", StageMood.Expansion),
                new Stage("Scientific Divide", StageMood.Conflict),
                new Stage("Trade Agreements", StageMood.Commerce)
            },
            FactionSeeds = new List<FactionSeed> {
                new FactionSeed("Atlas Industries", ArchetypeType.CorporateState),
                new FactionSeed("Helix Biotech", ArchetypeType.ResearchCollective),
                new FactionSeed("NovaTech", ArchetypeType.CorporateState),
                new FactionSeed("Preservation Council", ArchetypeType.SurvivorEnclave),
                new FactionSeed("New Horizon Labs", ArchetypeType.ResearchCollective)
            }
        },
        new ScenarioChain {
            Theme = "Rise of the Warlords",
            WorldSummary = "With no functioning government, competition over food and supplies turns violent. Criminal organizations evolve into powerful warlords controlling trade routes and territory. Over time, these syndicates transform into organized kingdoms ruled through fear and military strength.",
            Stages = new List<Stage> {
                new Stage("Government Collapse", StageMood.Turmoil),
                new Stage("Resource War", StageMood.Conflict),
                new Stage("Criminal Empires", StageMood.Conflict),
                new Stage("Territorial Expansion", StageMood.Expansion),
                new Stage("Regional Kingdoms", StageMood.Commerce)
            },
            FactionSeeds = new List<FactionSeed> {
                new FactionSeed("Highway Kings", ArchetypeType.RaiderSyndicate),
                new FactionSeed("Black Market Syndicate", ArchetypeType.CorporateState),
                new FactionSeed("Crimson Brokers", ArchetypeType.CorporateState),
                new FactionSeed("Iron Cartel", ArchetypeType.MilitaryRemnant),
                new FactionSeed("Dust Raiders", ArchetypeType.RaiderSyndicate)
            }
        },
        new ScenarioChain {
            Theme = "Faith After the Fall",
            WorldSummary = "After civilization collapses, isolated communities develop different beliefs about the outbreak. Religious factions grow in influence, leading to ideological conflicts and holy wars. Eventually, alliances are formed to unite against common enemies and ensure long-term survival.",
            Stages = new List<Stage> {
                new Stage("Government Collapse", StageMood.Turmoil),
                new Stage("Independent Survivor Settlements", StageMood.Expansion),
                new Stage("Religious Divide", StageMood.Conflict),
                new Stage("Holy Wars", StageMood.Conflict),
                new Stage("Peace Through Alliances", StageMood.Diplomacy)
            },
            FactionSeeds = new List<FactionSeed> {
                new FactionSeed("Church of Purity", ArchetypeType.ZealousCongregation),
                new FactionSeed("Children of Dawn", ArchetypeType.ZealousCongregation),
                new FactionSeed("Order of Ash", ArchetypeType.DoomsdayCult),
                new FactionSeed("The Redeemed", ArchetypeType.SurvivorEnclave),
                new FactionSeed("Apostate Covenant", ArchetypeType.DoomsdayCult)
            }
        },
        new ScenarioChain {
            Theme = "Rebuilding Civilization",
            WorldSummary = "The initial collapse leaves scattered military units and survivor settlements isolated. Rather than competing endlessly, scientists, engineers, and community leaders cooperate to restore agriculture, medicine, and communication. Trade flourishes, alliances strengthen, and the foundations of a new civilization begin to emerge.",
            Stages = new List<Stage> {
                new Stage("Government Collapse", StageMood.Turmoil),
                new Stage("Military Fragmentation", StageMood.Turmoil),
                new Stage("Independent Survivor Settlements", StageMood.Expansion),
                new Stage("Scientific Cooperation", StageMood.Diplomacy),
                new Stage("Trade Networks", StageMood.Commerce),
                new Stage("Regional Alliances", StageMood.Diplomacy)
            },
            FactionSeeds = new List<FactionSeed> {
                new FactionSeed("Republic of Haven", ArchetypeType.FortifiedFreehold),
                new FactionSeed("River Alliance", ArchetypeType.CorporateState),
                new FactionSeed("Institute of Hope", ArchetypeType.ResearchCollective),
                new FactionSeed("Iron Legion", ArchetypeType.MilitaryRemnant),
                new FactionSeed("Green Valley Collective", ArchetypeType.SurvivorEnclave)
            }
        }
    };

    public static ScenarioChain PickRandom(System.Random rng) => All[rng.Next(All.Count)];
}
