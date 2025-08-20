using HarmonyLib;
using RandomReinforcementsPerEncounter.GameApi.Chest;
using RandomReinforcementsPerEncounter.State;
using TurnBased.Controllers;

namespace RandomReinforcementsPerEncounter.Patches.Combat
{
    [HarmonyPatch(typeof(CombatController), nameof(CombatController.HandleCombatEnd))]
    internal static class CombatEndPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            CombatFlags.ReinforcementsSpawned = false;
            ChestService.TrySpawnDefaultChestAt(LootContext.ChestPosition);
        }
    }
}
