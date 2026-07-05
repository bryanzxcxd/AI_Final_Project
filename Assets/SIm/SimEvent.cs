using UnityEngine;

public class SimEvent
{
    public int Tick;
    public ActionType Action;
    public int FactionId;
    public int? TargetFactionId;
    public string Description; // filled in later by the lore templater
}