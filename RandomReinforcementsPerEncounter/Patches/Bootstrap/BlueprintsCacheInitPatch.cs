using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using RandomReinforcementsPerEncounter.GameApi.Clones;
using RandomReinforcementsPerEncounter.GameApi.Enchantments;
using RandomReinforcementsPerEncounter.GameApi.Weapons;

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

            try
            {
                FeatureRegister.RegisterAll();
                EnchantRegister.RegisterAll();
                WeaponRegistry.Create_SawtoothSabre_Standard();
                WeaponRegistry.BuildAllOversizedFromList();
            }
            catch (System.Exception ex)
            {
                Main.ModEntry.Logger.Error($"[RRE] Failed to register blueprints: {ex}");
            }
        }
    }

}

