using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class LootUtils
    {
        public static BlueprintItemEnchantment TryLoadEnchant(BlueprintGuid guid)
            => ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(guid);

        public static BlueprintItemEnchantment TryLoadEnchant(string id)
        {
            try
            {
                var guid = BlueprintGuid.Parse(id);
                return ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(guid);
            }
            catch { return null; }
        }
    }
}
