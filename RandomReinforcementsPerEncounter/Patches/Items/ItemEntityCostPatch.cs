using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.Config.Settings;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Components;
using System.Linq;
using UnityEngine;

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

            if (extra <= 0) return;

            float mult = Mathf.Clamp(ModSettings.Instance.GenItemValuePct, 1f, 1000f) / 100f;
            int scaledExtra = Mathf.Max(0, Mathf.RoundToInt(extra * mult));

            __result += scaledExtra;
        }
    }
}
