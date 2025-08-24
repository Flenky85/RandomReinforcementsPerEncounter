namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class DoubleSecondIds
    {
        /// <summary>
        /// GUID (string) para una variante de arma: "<root>:<variant>"
        /// </summary>
        public static string ForVariant(string root)
            => IdGenerators.WeaponId($"{root}:Second").ToString();
    }
}