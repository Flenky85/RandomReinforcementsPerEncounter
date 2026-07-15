using HarmonyLib;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.Config.Settings;

namespace RandomReinforcementsPerEncounter.Patches.Items
{
    [HarmonyPatch(typeof(ItemEntity), nameof(ItemEntity.Name), MethodType.Getter)]
    internal static class ItemEntityNamePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ItemEntity __instance, ref string __result)
        {
            if (ModSettings.Instance.ShowGeneratedItemNames)
            {
                ItemNameFormatter.TryDecorateName(__instance, ref __result);
            }
            else
            {
                ItemNameFormatter.TryUseBaseName(__instance, ref __result);
            }
        }
    }
}