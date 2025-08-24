namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class PriceIds
    {
        private const string Root = "price";

        /// Devuelve el GUID (string) para "price_{valor}".
        public static string ForValue(int value)
            => IdGenerators.EnchantId($"{Root}_{value}").ToString();
    }
}
