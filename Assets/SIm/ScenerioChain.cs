using System.Collections.Generic;

public class ScenarioChain
{
    public string Theme;
    public string WorldSummary;
    public List<string> Stages;
    public List<string> FactionNames;

    public static List<ScenarioChain> All = new List<ScenarioChain> {
        new ScenarioChain {
            Theme = "The Great Collapse",
            WorldSummary = "A rapidly spreading outbreak causes governments to collapse. Isolated military commands become independent powers while survivors establish fortified settlements. As food, fuel, and medicine become scarce, conflicts erupt over resources. Eventually, neighboring factions begin trading to improve their chances of survival.",
            Stages = new List<string> { "Government Collapse", "Military Fragmentation", "Independent Survivor Settlements", "Resource War", "Trade Networks" },
            FactionNames = new List<string> { "Northern Command", "Haven City", "River Coalition", "Iron Dominion", "Frontier Republic" }
        },
        new ScenarioChain {
            Theme = "Corporate Ascension",
            WorldSummary = "A failed pharmaceutical experiment triggers the apocalypse. Governments collapse while major corporations seal themselves inside secure facilities. Years later, corporate city-states emerge and compete through research, technology, and trade while searching for a cure—or an advantage.",
            Stages = new List<string> { "Pharmaceutical Disaster", "Government Collapse", "Corporate City-States", "Scientific Divide", "Trade Agreements" },
            FactionNames = new List<string> { "Atlas Industries", "Helix Biotech", "NovaTech", "Preservation Council", "New Horizon Labs" }
        },
        new ScenarioChain {
            Theme = "Rise of the Warlords",
            WorldSummary = "With no functioning government, competition over food and supplies turns violent. Criminal organizations evolve into powerful warlords controlling trade routes and territory. Over time, these syndicates transform into organized kingdoms ruled through fear and military strength.",
            Stages = new List<string> { "Government Collapse", "Resource War", "Criminal Empires", "Territorial Expansion", "Regional Kingdoms" },
            FactionNames = new List<string> { "Highway Kings", "Black Market Syndicate", "Crimson Brokers", "Iron Cartel", "Dust Raiders" }
        },
        new ScenarioChain {
            Theme = "Faith After the Fall",
            WorldSummary = "After civilization collapses, isolated communities develop different beliefs about the outbreak. Religious factions grow in influence, leading to ideological conflicts and holy wars. Eventually, alliances are formed to unite against common enemies and ensure long-term survival.",
            Stages = new List<string> { "Government Collapse", "Independent Survivor Settlements", "Religious Divide", "Holy Wars", "Peace Through Alliances" },
            FactionNames = new List<string> { "Church of Purity", "Children of Dawn", "Order of Ash", "The Redeemed", "Apostate Covenant" }
        },
        new ScenarioChain {
            Theme = "Rebuilding Civilization",
            WorldSummary = "The initial collapse leaves scattered military units and survivor settlements isolated. Rather than competing endlessly, scientists, engineers, and community leaders cooperate to restore agriculture, medicine, and communication. Trade flourishes, alliances strengthen, and the foundations of a new civilization begin to emerge.",
            Stages = new List<string> { "Government Collapse", "Military Fragmentation", "Independent Survivor Settlements", "Scientific Cooperation", "Trade Networks", "Regional Alliances" },
            FactionNames = new List<string> { "Republic of Haven", "River Alliance", "Institute of Hope", "Iron Legion", "Green Valley Collective" }
        }
    };

    public static ScenarioChain PickRandom(System.Random rng) => All[rng.Next(All.Count)];
}