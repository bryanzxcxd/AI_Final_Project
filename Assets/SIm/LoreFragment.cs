using System.Collections.Generic;

public static class LoreFragments
{

    public static readonly Dictionary<ArchetypeType, Dictionary<ActionType, List<string>>> Fragments = new() {

        { ArchetypeType.ImperialDominion, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The Imperial Legions marched forth, their eagle standards gleaming, as {actor} declared {target} to be the next province of the Eternal Empire.",
                "With the blessing of the Emperor, the armies of {actor} crossed the border, determined to bring civilization to the people of {target}.",
                "Imperial war-horns sounded across the plains as {actor}'s forces advanced on {target}, their destiny written in blood and iron."
            }},
            { ActionType.Alliance, new List<string> {
                "The Emperor and the leaders of {target} signed a treaty of mutual benefit, uniting their realms against common enemies.",
                "A strategic alliance was forged between {actor} and {target}, each seeing the other as a useful tool."
            }},
            { ActionType.BreakPeace, new List<string> {
                "In a stunning act of treachery, {actor} ordered its legions to turn on {target}, their former allies.",
                "The Empire showed its true nature as it betrayed {target}, proving that power is the only loyalty."
            }},
            { ActionType.Destroyed, new List<string> {
                "The Eternal Empire finally fell; {actor}'s legions broken, its provinces divided among the victors.",
                "After centuries of dominance, {actor} was conquered by {target}, its glory reduced to memory."
            }},
            { ActionType.Trade, new List<string> {
                "Imperial trade caravans began regular exchange with {target}, though {actor} viewed it as tribute in all but name.",
            }},
        }},

        { ArchetypeType.MerchantCoalition, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The Merchant Coalition hired the finest mercenaries gold could buy, launching a war of profit against {target}.",
                "Economics turned to warfare as {actor}'s trade fleet struck at {target}'s shipping routes.",
                "A war of acquisition — {actor} launched a campaign against {target}, seeing only the bottom line."
            }},
            { ActionType.Alliance, new List<string> {
                "A commercial alliance was signed between {actor} and {target}, promising mutual profit and shared trade routes.",
                "A trade consortium was formed between {actor} and {target}, pooling resources for greater collective profit."
            }},
            { ActionType.BreakPeace, new List<string> {
                "The merchants of {actor} sold out their allies in {target}, revealing trade secrets for gold.",
                "A better deal was offered; {actor} broke its treaty with {target} without hesitation."
            }},
            { ActionType.Destroyed, new List<string> {
                "The coffers of {actor} ran dry; unpaid mercenaries turned on their masters, and {target} picked over the ruins.",
                "Economic collapse destroyed {actor}; {target} laughed all the way to the bank."
            }},
            { ActionType.Trade, new List<string> {
                "Merchant guilds of {actor} and {target} opened new routes, silk and spice flowing freely between their ports.",
                "A lucrative trade agreement was struck between {actor} and {target}."
            }},
        }},

        { ArchetypeType.HolyTheocracy, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "A crusade was declared! The faithful of {actor} rose up to purge the heathens of {target}.",
                "The High Pontiff blessed the armies as {actor} marched to cleanse {target} of heresy.",
                "The saints wept as {actor} launched a holy war against {target}, their faith their shield and sword."
            }},
            { ActionType.Alliance, new List<string> {
                "A pact of faith was sworn; {actor} and {target} united in holy purpose.",
                "Divine providence brought {actor} and {target} together; their alliance was written into scripture."
            }},
            { ActionType.BreakPeace, new List<string> {
                "The Holy Synod of {actor} declared {target} to be heretics, breaking their covenant in the name of God.",
                "The Inquisition of {actor} denounced {target} as heretics, and the treaty was cast into the fire."
            }},
            { ActionType.Destroyed, new List<string> {
                "The faith was broken; {actor}'s gods proved silent as the temples burned and the priests were slaughtered.",
                "The last High Pontiff of {actor} died in the ashes of the cathedral, conquered by {target}."
            }},
            { ActionType.Trade, new List<string> {
                "Pilgrims and merchants of {actor} and {target} began exchanging goods, though the clergy watched the heathens warily."
            }},
        }},

        { ArchetypeType.NorthernKhanate, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The warbands of {actor} descended upon {target} in a brutal raid, steel and fire in the night.",
                "Riders of {actor} swept across the plains, striking {target} before word could spread.",
            }},
            { ActionType.Alliance, new List<string> {
                "The khans of {actor} and {target} swore a blood-oath, uniting their warbands under a single banner.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "The truce meant nothing to {actor}; their riders struck {target} without warning, as the old ways demanded.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The horde of {actor} was scattered and broken, its khans slain by {target}, its name forgotten by the steppe.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} grudgingly exchanged horses and furs with {target}, though they still eyed their settlements with hunger."
            }},
        }},

        { ArchetypeType.ForestConfederacy, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The ancient wood itself seemed to rise against {target} as {actor} fought to reclaim their sacred groves.",
                "{actor}'s rangers struck from the shadows of the Greenwood, defending their borders against {target}.",
            }},
            { ActionType.Alliance, new List<string> {
                "The Voice of the Forest spoke, and {actor} allied with {target} against a shared threat to the old woods.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "Even the patient elders of {actor} could bear it no longer; the peace with {target} was renounced.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The Great Greenwood fell silent as {actor} was destroyed by {target}; the ancient trees would not sing again.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} cautiously traded herbs and timber with {target}, wary of outsiders within their sacred woods."
            }},
        }},

        { ArchetypeType.DwarvenMountainholds, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The forges of {actor} rang with war as their legions marched to settle an old grudge with {target}.",
                "{actor}'s shield-clans descended from the mountain halls, their axes thirsty for {target}'s blood.",
            }},
            { ActionType.Alliance, new List<string> {
                "The High King of {actor} clasped hands with {target}, an alliance sealed in mithril and stone.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "An insult to clan honor could not go unanswered; {actor} broke their peace with {target}.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The mountain halls of {actor} fell silent at last, their gates broken by {target} after a siege for the ages.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor}'s master smiths traded fine steel and gems with {target}, enriching both holds."
            }},
        }},

        { ArchetypeType.UndeadKingdom, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The tireless armies of {actor}, neither hungry nor afraid, laid siege to {target}.",
                "{actor}'s legions of the dead marched without rest upon {target}, indifferent to loss.",
            }},
            { ActionType.Alliance, new List<string> {
                "A pragmatic pact was struck; {target} allied with {actor}, setting aside their fear of the dead for mutual gain.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "{actor} required more subjects for its eternal dominion, and the peace with {target} was discarded.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The Lich-King's realm of {actor} was finally unmade by {target}; even undeath had its end.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} exchanged strange relics and rare metals with {target}, who dealt with them out of necessity, not trust."
            }},
        }},

        { ArchetypeType.FreeholdRepublic, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "Reluctantly, the Senate of {actor} authorized war against {target} after diplomacy failed.",
                "The citizen-soldiers of {actor} marched to defend their principles against {target}.",
            }},
            { ActionType.Alliance, new List<string> {
                "The Senate of {actor} ratified a formal alliance with {target}, sealed by treaty and law.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "Only after {target}'s repeated provocations did {actor}'s Senate vote to end the peace.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The Republic of {actor} fell at last, its Senate dissolved by {target}, its ideals outliving its borders.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} established fair and lawful trade with {target}, a model of mutual benefit."
            }},
        }},
    };

    public static string TryGetFragment(ArchetypeType archetype, ActionType action, SimEvent evt)
    {
        if (!Fragments.TryGetValue(archetype, out var actionMap)) return null;
        if (!actionMap.TryGetValue(action, out var options) || options.Count == 0) return null;

        int index = (evt.Tick * 31 + evt.FactionId) % options.Count;
        return options[index];
    }

    public static string Format(string template, Faction actor, Faction target)
    {
        string result = template.Replace("{actor}", actor?.Name ?? "an unknown power");
        result = result.Replace("{target}", target != null ? target.Name : "their enemies");
        return result;
    }
}