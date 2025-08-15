using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using UnityEngine;


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
            new GameObject("RRE_BlueprintRegistrar").AddComponent<BlueprintRegistrar>();
        }
        private class BlueprintRegistrar : MonoBehaviour
        {
            private System.Collections.IEnumerator Start()
            {
                yield return null; // esperar a que termine Init

                try
                {
                    FeatureRegister.RegisterAll();
                    EnchantRegister.RegisterAll();
                    Debug.Log("[RRE] Enhchants tiers init done.");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[RRE] Error registering enhchants tiers: " + ex);
                }

                Destroy(gameObject); // limpiar
            }
        }
    }
}
