using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using RandomReinforcementsPerEncounter.GameApi.Clones;
using RandomReinforcementsPerEncounter.GameApi.Enchantments;
using RandomReinforcementsPerEncounter.GameApi.Weapons;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.Patches.Bootstrap
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class BlueprintsCacheInitPatch
    {
        private static bool _initialized;

        [HarmonyPostfix]
        private static void Postfix()
        {
            if (_initialized) return;
            _initialized = true;

            new CloneDeathWatcher();
            new AreaUnloadWatcher();
            MainJoinCombatHandler.Init();
            new GameObject("RRE_BlueprintRegistrar").AddComponent<BlueprintRegistrar>();
        }

        private sealed class BlueprintRegistrar : MonoBehaviour
        {
            private System.Collections.IEnumerator Start()
            {
                yield return null; // esperar un frame tras Init

                try
                {
                    FeatureRegister.RegisterAll();
                    EnchantRegister.RegisterAll();
                    WeaponRegistry.Create_SawtoothSabre_Standard();
                    WeaponRegistry.BuildAllOversizedFromList();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[RRE] Error registering blueprints: " + ex);
                }
                Destroy(gameObject); // limpiar
            }
        }
    }
}

