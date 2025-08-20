using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Config.Ids;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class PriceRefs
    {
        public static BlueprintItemEnchantment PriceT0 => _p20 ??= Get("price_20");
        public static BlueprintItemEnchantment PriceT1 => _p40 ??= Get("price_40");
        public static BlueprintItemEnchantment PriceT2 => _p80 ??= Get("price_80");
        public static BlueprintItemEnchantment PriceT3 => _p160 ??= Get("price_160");
        public static BlueprintItemEnchantment PriceT4 => _p320 ??= Get("price_320");
        public static BlueprintItemEnchantment PriceT5 => _p640 ??= Get("price_640");
        public static BlueprintItemEnchantment PriceT6 => _p1280 ??= Get("price_1280");

        private static BlueprintItemEnchantment _p20, _p40, _p80, _p160, _p320, _p640, _p1280;

        private static BlueprintItemEnchantment Get(string key)
            => ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(GuidUtil.EnchantGuid(key));
    }
}
