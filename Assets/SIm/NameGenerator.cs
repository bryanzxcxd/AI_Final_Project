using System.Collections.Generic;

public static class NameGenerator
{
    static readonly string[] Prefixes = { "Val", "Kar", "Thal", "Or", "Bran", "Sil", "Dor", "Gal", "Wyn", "Az" };
    static readonly string[] Suffixes = { "dor", "heim", "garde", "mont", "shire", "wick", "fell", "reach", "haven", "ath" };

    public static string GenerateKingdomName(System.Random rng)
    {
        string prefix = Prefixes[rng.Next(Prefixes.Length)];
        string suffix = Suffixes[rng.Next(Suffixes.Length)];
        return prefix + suffix;
    }
}