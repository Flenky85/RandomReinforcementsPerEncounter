using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using System;
using System.Linq;
using UnityEngine;
using static RandomReinforcementsPerEncounter.EnchantFactory;


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
                    WeaponRegistry.Create_SawtoothSabre_Standard();
                    WeaponRegistry.BuildAllOversizedFromList();
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

    [HarmonyPatch(typeof(ItemEntity), "get_Cost")]
    static class Patch_ItemEntity_get_Cost
    {
        static void Postfix(ItemEntity __instance, ref int __result)
        {
            int extra = __instance.Enchantments?
                .Select(e => e?.Blueprint?.GetComponent<RRE_PriceDeltaComponent>())
                .Where(c => c != null)
                .Sum(c => c.Delta) ?? 0;

            __result += extra;
        }
    }
}
