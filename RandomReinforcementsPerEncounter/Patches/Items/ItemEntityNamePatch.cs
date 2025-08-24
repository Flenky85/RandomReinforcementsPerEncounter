using HarmonyLib;
using Kingmaker.Items;

namespace RandomReinforcementsPerEncounter.Patches.Items
{
    [HarmonyPatch(typeof(ItemEntity), nameof(ItemEntity.Name), MethodType.Getter)]
    internal static class ItemEntityNamePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ItemEntity __instance, ref string __result)
        {
            ItemNameFormatter.TryDecorateName(__instance, ref __result);
        }
    }
}
