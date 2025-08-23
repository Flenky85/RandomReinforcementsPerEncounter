using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Config.Ids;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class PriceRefs
    {
        
        private static BlueprintItemEnchantment _p200, _p400, _p800, _p1600, _p3200, _p6400;

        public static BlueprintItemEnchantment PriceT1 => _p200 ??= Get("price_200");
        public static BlueprintItemEnchantment PriceT2 => _p400 ??= Get("price_400");
        public static BlueprintItemEnchantment PriceT3 => _p800 ??= Get("price_800");
        public static BlueprintItemEnchantment PriceT4 => _p1600 ??= Get("price_1600");
        public static BlueprintItemEnchantment PriceT5 => _p3200 ??= Get("price_3200");
        public static BlueprintItemEnchantment PriceT6 => _p6400 ??= Get("price_6400");

        private static BlueprintItemEnchantment Get(string key)
            => ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(GuidUtil.EnchantGuid(key));
    }
}
