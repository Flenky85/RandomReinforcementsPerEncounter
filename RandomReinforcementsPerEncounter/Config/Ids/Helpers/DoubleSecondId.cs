namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class DoubleSecondIds
    {
        public static string ForVariant(string root)
            => IdGenerators.WeaponId($"{root}:Second").ToString();
    }
}