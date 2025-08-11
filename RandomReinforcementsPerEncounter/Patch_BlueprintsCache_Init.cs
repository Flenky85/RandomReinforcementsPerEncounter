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
                yield return null;

                try
                {
                    var cache = (BlueprintsCache)AccessTools
                        .Property(typeof(BlueprintsCache), "Instance")
                        .GetValue(null);

                    var clone = EnchantMaker.CloneCorrosive1d8_Safe();
                    if (clone == null)
                    {
                        Debug.LogError("[RRE] Clone es null.");
                        yield break;
                    }

                    cache.AddCachedBlueprint(clone.AssetGuid, clone);
                    Debug.Log("[RRE] OK: clon corrosive registrado: " + clone.AssetGuid);
                }
                catch (Exception ex)
                {
                    Debug.LogError("[RRE] EX al registrar clon: " + ex);
                }

                Destroy(gameObject);
            }
        }
    }
}
