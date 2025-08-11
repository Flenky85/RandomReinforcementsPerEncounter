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
            
            //Chest type
            //7cc4a05acaf44ea59357843c8161b081
            //a3f0ed9a361b4e5eb84e1ab8abd77a67
            //1ccbdc2361534a8d99e4043b8b345e72
            ChestSpawn.SpawnLootChest("1ccbdc2361534a8d99e4043b8b345e72", LootContext.ChestPosition.Value);
            //ItemLogger.LogItems();
        }
    }
}




