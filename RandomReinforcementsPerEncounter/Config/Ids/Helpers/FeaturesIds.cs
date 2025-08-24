namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class FeatureIds
    {
        /// GUID (string) para una feature con raíz y tier: "<keyRoot>.t{tier}"
        public static string ForTier(string keyRoot, int tier)
            => IdGenerators.FeatureId($"{keyRoot}.t{tier}").ToString();
    }
}
