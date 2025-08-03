using HarmonyLib;
using TurnBased.Controllers;

namespace RandomReinforcementsPerEncounter
{
    [HarmonyPatch(typeof(CombatController), "HandleCombatEnd")]
    public static class CombatEndPatch
    {
        static void Postfix()
        {
            CombatFlags.ReinforcementsSpawned = false;
            ChestSpawn.SpawnLootChest("7cc4a05acaf44ea59357843c8161b081", ChestSpawn.storedChestPosition.Value);
        }
    }
}




