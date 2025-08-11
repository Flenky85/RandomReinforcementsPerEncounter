using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;


namespace RandomReinforcementsPerEncounter
{
    [HarmonyPatch(typeof(BlueprintsCache), "Init")]
    public static class Patch_BlueprintsCache_Init
    {
        static bool Initialized;
        static void Postfix()
        {
            //MonsterLogger.LogMonsters(BossList.Monsters);
            //BlueprintInspector.LogBlueprintDetails(BossList.Monsters);
            //BlueprintInspector.DumpBlueprintsRaw(BossList.Monsters);
            
            if (Initialized) return;
            Initialized = true;

            new CloneDeathWatcher();
            MainJoinCombatHandler.Init();
        }
    }
}
