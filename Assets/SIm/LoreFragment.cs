using System.Collections.Generic;

public static class LoreFragments
{

    public static readonly Dictionary<ArchetypeType, Dictionary<ActionType, List<string>>> Fragments = new() {

        { ArchetypeType.MilitaryRemnant, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "Armored columns of {actor} rolled out at dawn, declaring the territory of {target} a zone of military necessity.",
                "Operating under protocols written for a government that no longer existed, {actor} launched a full offensive against {target}.",
                "Sirens wailed across the outposts of {target} as {actor}'s remaining battalions advanced under radio silence."
            }},
            { ActionType.Alliance, new List<string> {
                "The command staff of {actor} signed a joint-defense accord with {target}, folding their patrol routes into a single perimeter.",
                "{actor} extended military protection to {target} in exchange for supplies, an arrangement both sides called an alliance."
            }},
            { ActionType.BreakPeace, new List<string> {
                "Citing operational security, {actor} tore up its ceasefire with {target} and moved its checkpoints forward overnight.",
                "The generals of {actor} decided {target} was a liability rather than a partner, and the guns spoke again."
            }},
            { ActionType.Destroyed, new List<string> {
                "The last garrison of {actor} ran out of ammunition and fuel; {target} walked through the wire unopposed.",
                "Command structure broken and supply lines cut, {actor} surrendered its final stronghold to {target}."
            }},
            { ActionType.Trade, new List<string> {
                "Quartermasters of {actor} began exchanging fuel and ammunition with {target}, rations moving one way and spare parts the other."
            }},
        }},

        { ArchetypeType.SurvivorEnclave, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "Pushed past endurance, the people of {actor} took up scavenged weapons and struck at {target}'s outposts.",
                "The watchtowers of {actor} emptied as its defenders marched out, resolved to end the threat from {target}."
            }},
            { ActionType.Alliance, new List<string> {
                "Around a shared fire, the elders of {actor} and {target} agreed their people would stand together or not at all.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "Even the patient leaders of {actor} could bear it no longer; the truce with {target} was abandoned.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The gates of {actor} were breached at last by {target}; its people scattered into the wastes.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} cautiously opened its gates to caravans from {target}, trading seed grain and clean water under armed watch."
            }},
        }},

        { ArchetypeType.CorporateState, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The board of {actor} approved a hostile acquisition of {target} — enforced by private security contractors.",
                "Asset-recovery teams from {actor} crossed into {target}'s territory, the operation filed internally as a mergers dispute.",
                "For {actor}, the war against {target} was a line item: projected losses acceptable, projected returns excellent."
            }},
            { ActionType.Alliance, new List<string> {
                "A partnership agreement was executed between {actor} and {target}, complete with exclusivity clauses and profit sharing.",
                "{actor} and {target} announced a strategic merger of interests, pooling logistics and security assets."
            }},
            { ActionType.BreakPeace, new List<string> {
                "A better offer arrived; {actor} voided its contract with {target} without notice.",
                "The accountants of {actor} determined the treaty with {target} no longer served shareholder interests."
            }},
            { ActionType.Destroyed, new List<string> {
                "The ledgers of {actor} ran red; unpaid contractors walked off the walls, and {target} claimed the remaining assets.",
                "{actor} filed for dissolution the hard way — its facilities stripped and absorbed by {target}."
            }},
            { ActionType.Trade, new List<string> {
                "{actor} and {target} signed a supply contract, convoys moving medicine and parts on a fixed schedule.",
                "A lucrative exchange opened between {actor} and {target}, margins carefully audited on both sides."
            }},
        }},

        { ArchetypeType.ResearchCollective, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "Reluctantly, the researchers of {actor} turned their prototypes on {target}; the data, they told themselves, would survive.",
                "{actor} deployed weapons no one else could rebuild, and {target}'s defenses failed in ways their engineers could not explain."
            }},
            { ActionType.Alliance, new List<string> {
                "{actor} and {target} agreed to share findings and facilities, their combined archives the largest since the collapse.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "The council of {actor} concluded that {target} could not be trusted with what they had discovered, and cooperation ended.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The laboratories of {actor} burned; what {target} could not understand, it destroyed, and decades of work were lost.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} traded filtration units and medical know-how to {target} in return for raw materials and field data."
            }},
        }},

        { ArchetypeType.RaiderSyndicate, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "War rigs of {actor} came down on {target} at night, taking fuel, food, and anyone who could work.",
                "The crews of {actor} hit {target}'s supply lines before word could spread, leaving burning depots behind them."
            }},
            { ActionType.Alliance, new List<string> {
                "The bosses of {actor} and {target} cut a deal over shared spoils, sealed with fuel, weapons, and blood.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "The truce meant nothing to {actor}; their crews hit {target} without warning, as everyone had always expected.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The war rigs of {actor} were burned or taken, its bosses strung up by {target}, its name already fading from the roads.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} grudgingly traded salvage and fuel with {target}, though their scouts kept mapping its defenses."
            }},
        }},

        { ArchetypeType.ZealousCongregation, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The faithful of {actor} declared {target} a den of the unclean and marched to purify it.",
                "Hymns rose over the ruins as {actor} launched its crusade against {target}, scripture painted on their armor.",
                "The preachers of {actor} named the plague a judgment and {target} its cause; the faithful needed no further reason."
            }},
            { ActionType.Alliance, new List<string> {
                "A covenant was sworn between {actor} and {target}, their union proclaimed as providence.",
                "The clergy of {actor} declared {target} redeemed, and the two flocks stood together."
            }},
            { ActionType.BreakPeace, new List<string> {
                "The synod of {actor} denounced {target} as heretics, and the covenant was cast into the fire.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The chapels of {actor} burned and its preachers fell silent, its flock scattered by {target}.",
            }},
            { ActionType.Trade, new List<string> {
                "Pilgrims of {actor} exchanged goods with {target}, though the clergy watched the unbelievers warily."
            }},
        }},

        { ArchetypeType.DoomsdayCult, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "The end was coming anyway — {actor} chose to bring it to {target} first.",
                "Ash-marked followers of {actor} walked into {target}'s territory without fear, for their prophets had promised them the world's last days."
            }},
            { ActionType.Alliance, new List<string> {
                "A pact few understood was struck: {target} allied with {actor}, setting aside dread for survival.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "The prophets of {actor} received a new revelation, and the peace with {target} was discarded as a relic of doubt.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The last shrine of {actor} was pulled down by {target}; whether its prophecy died with it, none could say.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} traded strange salvage and preserved stores with {target}, who dealt with them out of necessity, not trust."
            }},
        }},

        { ArchetypeType.FortifiedFreehold, new Dictionary<ActionType, List<string>> {
            { ActionType.Attack, new List<string> {
                "Only after the vote was counted did the militia of {actor} march on {target}, their charter amended for war.",
                "The council of {actor} exhausted every appeal before sending its citizen levies against {target}."
            }},
            { ActionType.Alliance, new List<string> {
                "The council of {actor} ratified a formal compact with {target}, posted on every notice board in the settlement.",
            }},
            { ActionType.BreakPeace, new List<string> {
                "Only after {target}'s repeated provocations did the council of {actor} vote to end the peace.",
            }},
            { ActionType.Destroyed, new List<string> {
                "The walls of {actor} were pulled down by {target}, its charter burned — though its laws outlived its borders.",
            }},
            { ActionType.Trade, new List<string> {
                "{actor} established regulated trade with {target}, tariffs and weights posted at the gate."
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
