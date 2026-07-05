using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class LoreTemplater
{

    public static string BuildChronicle(List<SimEvent> history, List<Faction> factions)
    {
        var sb = new StringBuilder();

        foreach (var evt in history)
        {
            string line = DescribeEvent(evt, factions);
            if (!string.IsNullOrEmpty(line))
            {
                sb.AppendLine($"Year {evt.Tick}: {line}");
            }
        }

        return sb.ToString();
    }

    static string DescribeEvent(SimEvent evt, List<Faction> factions)
    {
        Faction actor = factions.FirstOrDefault(f => f.Id == evt.FactionId);
        Faction target = evt.TargetFactionId.HasValue
            ? factions.FirstOrDefault(f => f.Id == evt.TargetFactionId.Value)
            : null;

        if (actor == null) return null;
        //HELP put ur take on things here
        switch (evt.Action)
        {
            case ActionType.Expand:
                return null; // too frequent/mundane to log every tick, skip for readability

            case ActionType.Consolidate:
                return null; // also mundane, skip

            case ActionType.Attack:
                return target != null
                    ? Pick(new[] {
                        $"The armies of {actor.Name} marched upon {target.Name}, igniting open war.",
                        $"{actor.Name} launched an assault against the territories of {target.Name}.",
                        $"War broke out as {actor.Name} struck at the borders of {target.Name}."
                    }, evt)
                    : null;

            case ActionType.Trade:
                return target != null
                    ? Pick(new[] {
                        $"Merchants of {actor.Name} and {target.Name} opened new trade routes, enriching both realms.",
                        $"A trade agreement was struck between {actor.Name} and {target.Name}."
                    }, evt)
                    : null;

            case ActionType.Alliance:
                return target != null
                    ? $"{actor.Name} and {target.Name} forged an alliance, swearing mutual support."
                    : null;

            case ActionType.DefensiveAlliance:
                return target != null
                    ? $"{actor.Name} and {target.Name} signed a defensive pact against common threats."
                    : null;

            case ActionType.PeaceTreaty:
                return target != null
                    ? $"After years of conflict, {actor.Name} and {target.Name} signed a peace treaty."
                    : null;

            case ActionType.BreakPeace:
                return target != null
                    ? $"{actor.Name} broke the peace with {target.Name}, reigniting old hostilities."
                    : null;

            default:
                return null;
        }
    }

    // simple deterministic pick based on tick+faction id, so re-running the same seed gives the same lore text
    static string Pick(string[] options, SimEvent evt)
    {
        int index = (evt.Tick * 31 + evt.FactionId) % options.Length;
        return options[index];
    }
}