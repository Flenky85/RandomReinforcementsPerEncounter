using HarmonyLib;
using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.GameApi;
using RandomReinforcementsPerEncounter.State;
using TurnBased.Controllers;

namespace RandomReinforcementsPerEncounter
{
    [HarmonyPatch(typeof(CombatController), "HandleCombatEnd")]
    public static class CombatEndPatch
    {
        static void Postfix()
        {
            CombatFlags.ReinforcementsSpawned = false;

            ChestService.SpawnLootChest(ChestIds.DefaultLootChest, LootContext.ChestPosition.Value);
        }
    }
}




