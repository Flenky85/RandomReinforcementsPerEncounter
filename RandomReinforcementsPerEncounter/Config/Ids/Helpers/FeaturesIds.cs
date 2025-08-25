namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class FeatureIds
    {
        public static string ForTier(string keyRoot, int tier)
            => IdGenerators.FeatureId($"{keyRoot}.t{tier}").ToString();
    }
}
