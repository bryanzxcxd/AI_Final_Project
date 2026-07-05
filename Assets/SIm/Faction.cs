using System.Collections.Generic;
using UnityEngine;

public enum RelationState { Neutral, Peace, War, Alliance, DefensivePact, Vassal }

public class Faction
{
    public int Id;
    public string Name;
    public Color Color;
    public HashSet<(int x, int y)> Territory = new HashSet<(int, int)>();
    public (int x, int y) Capital;
    public bool IsAlive = true;

    public float Aggression;
    public float Wealth;
    public float Stability;

    public Archetype Archetype;

    public Dictionary<int, RelationState> Relations = new Dictionary<int, RelationState>();
    public Dictionary<int, float> Animosity = new Dictionary<int, float>();
    public Dictionary<int, float> EconomicTies = new Dictionary<int, float>();

    public float Strength => Territory.Count * (1f + Wealth * 0.1f);

    public float GetAnimosity(int otherId) => Animosity.GetValueOrDefault(otherId, 0f);
    public float GetEconomicTies(int otherId) => EconomicTies.GetValueOrDefault(otherId, 0f);
}