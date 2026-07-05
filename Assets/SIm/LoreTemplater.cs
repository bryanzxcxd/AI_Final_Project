using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class LoreTemplater
{

    static readonly HashSet<ActionType> ConflictActions = new HashSet<ActionType> {
        ActionType.Attack, ActionType.BreakPeace, ActionType.Destroyed, ActionType.Rebellion
    };

    public static string BuildChronicle(List<SimEvent> history, List<Faction> factions, int totalTicks, ScenarioChain chain)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"=== {chain.Theme} ===");
        sb.AppendLine(chain.WorldSummary);
        sb.AppendLine();

        var enrichedHistory = InsertEraEvents(history, totalTicks);

        foreach (var evt in enrichedHistory)
        {
            string line = DescribeEvent(evt, factions);
            if (!string.IsNullOrEmpty(line))
            {
                if (evt.Action == ActionType.StageTransition)
                {
                    sb.AppendLine();
                    sb.AppendLine($"--- {line} ---");
                }
                else
                {
                    sb.AppendLine($"Year {evt.Tick}: {line}");
                }
            }
        }
        return sb.ToString();
    }

    static List<SimEvent> InsertEraEvents(List<SimEvent> history, int totalTicks)
    {
        const int goldenAgeThreshold = 20;

        var conflictTicks = history.Where(e => ConflictActions.Contains(e.Action)).Select(e => e.Tick).OrderBy(t => t).ToList();
        var result = new List<SimEvent>(history);
        int lastConflictTick = 0;

        foreach (int tick in conflictTicks)
        {
            int gap = tick - lastConflictTick;
            if (gap >= goldenAgeThreshold)
            {
                int announceTick = lastConflictTick + gap / 2;
                result.Add(new SimEvent { Tick = announceTick, Action = ActionType.GoldenAge, FactionId = -1, Description = gap.ToString() });
            }
            lastConflictTick = tick;
        }

        int finalGap = totalTicks - lastConflictTick;
        if (finalGap >= goldenAgeThreshold)
        {
            int announceTick = lastConflictTick + finalGap / 2;
            result.Add(new SimEvent { Tick = announceTick, Action = ActionType.GoldenAge, FactionId = -1, Description = finalGap.ToString() });
        }

        return result.OrderBy(e => e.Tick).ToList();
    }

    static string DescribeEvent(SimEvent evt, List<Faction> factions)
    {
        if (evt.Action == ActionType.StageTransition) return evt.Description;

        if (evt.Action == ActionType.GoldenAge)
        {
            int years = int.Parse(evt.Description);
            return Pick(new[] {
                $"For {years} years, the known realms knew no significant conflict — a golden age of peace and prosperity.",
                $"A long era of calm settled over the land, lasting {years} years without major war.",
                $"The world entered a period of relative harmony, undisturbed by war for {years} years."
            }, evt);
        }

        Faction actor = factions.FirstOrDefault(f => f.Id == evt.FactionId);
        Faction target = evt.TargetFactionId.HasValue ? factions.FirstOrDefault(f => f.Id == evt.TargetFactionId.Value) : null;
        if (actor == null) return null;

        string causeClause = BuildCauseClause(evt, factions);

        switch (evt.Action)
        {
            case ActionType.Expand:
            case ActionType.Consolidate:
                return null;

            case ActionType.Attack:
                if (target == null) return null;
                {
                    string fragment = LoreFragments.TryGetFragment(actor.Archetype.Type, ActionType.Attack, evt);
                    string line = fragment != null ? LoreFragments.Format(fragment, actor, target) : $"War broke out as {actor.Name} struck at the borders of {target.Name}.";
                    return causeClause + line;
                }

            case ActionType.Trade:
                if (target == null) return null;
                {
                    string fragment = LoreFragments.TryGetFragment(actor.Archetype.Type, ActionType.Trade, evt);
                    return fragment != null ? LoreFragments.Format(fragment, actor, target)
                        : $"Merchants of {actor.Name} and {target.Name} opened new trade routes, enriching both realms.";
                }

            case ActionType.TradeMilestone:
                return target != null
                    ? Pick(new[] {
                        $"The trade partnership between {actor.Name} and {target.Name} flourished into a deep economic bond.",
                        $"Merchant guilds of {actor.Name} and {target.Name} grew intertwined, their fortunes now linked."
                    }, evt)
                    : null;

            case ActionType.Alliance:
                if (target == null) return null;
                {
                    string fragment = LoreFragments.TryGetFragment(actor.Archetype.Type, ActionType.Alliance, evt);
                    string line = fragment != null ? LoreFragments.Format(fragment, actor, target) : $"{actor.Name} and {target.Name} forged an alliance, swearing mutual support.";
                    return causeClause + line;
                }

            case ActionType.DefensiveAlliance:
                return target != null ? $"{actor.Name} and {target.Name} signed a defensive pact against common threats." : null;

            case ActionType.PeaceTreaty:
                return target != null ? $"After years of conflict, {actor.Name} and {target.Name} signed a peace treaty." : null;

            case ActionType.BreakPeace:
                if (target == null) return null;
                {
                    string fragment = LoreFragments.TryGetFragment(actor.Archetype.Type, ActionType.BreakPeace, evt);
                    string line = fragment != null ? LoreFragments.Format(fragment, actor, target) : $"{actor.Name} broke the peace with {target.Name}, reigniting old hostilities.";
                    return causeClause + line;
                }

            case ActionType.Destroyed:
                {
                    string fragment = target != null ? LoreFragments.TryGetFragment(actor.Archetype.Type, ActionType.Destroyed, evt) : null;
                    if (fragment != null) return LoreFragments.Format(fragment, actor, target);
                    return target != null
                        ? $"The realm of {actor.Name} fell at last, conquered utterly by {target.Name}."
                        : $"The realm of {actor.Name} collapsed and was erased from history.";
                }

            case ActionType.Rebellion:
                string formerOwnerName = target != null ? target.Name : "a fallen people";
                return causeClause + $"the descendants of {formerOwnerName} rose up against {actor.Name}, reclaiming their ancestral land.";

            default:
                return null;
        }
    }

    static string BuildCauseClause(SimEvent evt, List<Faction> factions)
    {
        if (evt.CausedBy == null) return "";
        Faction causeActor = factions.FirstOrDefault(f => f.Id == evt.CausedBy.FactionId);
        if (causeActor == null) return "";

        return evt.CausedBy.Action switch
        {
            ActionType.PeaceTreaty => $"Still bitter over the broken peace of Year {evt.CausedBy.Tick}, ",
            ActionType.BreakPeace => $"Tensions unresolved since Year {evt.CausedBy.Tick} finally boiled over, and ",
            ActionType.Attack => $"A conflict rooted in the war of Year {evt.CausedBy.Tick} resurfaced as ",
            ActionType.Destroyed => $"Long after their homeland fell in Year {evt.CausedBy.Tick}, ",
            ActionType.Trade => $"Following years of profitable trade since Year {evt.CausedBy.Tick}, ",
            ActionType.TradeMilestone => $"Bound by deep economic ties forged in Year {evt.CausedBy.Tick}, ",
            _ => ""
        };
    }

    static string Pick(string[] options, SimEvent evt)
    {
        int index = (evt.Tick * 31 + evt.FactionId) % options.Length;
        return options[index];
    }
}