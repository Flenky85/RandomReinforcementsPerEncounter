using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Components;
using System.Linq;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter.Patches.Items
{
    [HarmonyPatch(typeof(ItemEntity), nameof(ItemEntity.Cost), MethodType.Getter)]
    internal static class ItemEntityCostPatch
    {
        [HarmonyPostfix]
        private static void Postfix(ItemEntity __instance, ref int __result)
        {
            int extra = __instance.Enchantments?
                .Select(e => e?.Blueprint?.GetComponent<RRE_PriceDeltaComponent>())
                .Where(c => c != null)
                .Sum(c => c.Delta) ?? 0;

            __result += extra;
        }
    }
}
