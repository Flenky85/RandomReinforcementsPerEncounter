using System;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using UnityEngine;
using RandomReinforcementsPerEncounter.Config.Ids;


namespace RandomReinforcementsPerEncounter.Patches
{
    [HarmonyPatch(typeof(BlueprintsCache), "Init")]
    internal static class BlueprintsCache_Init_Patch
    {
        static void Postfix()
        {
            try
            {
              
                var plannedEnchants = new string[] {  };
                foreach (var id in plannedEnchants)
                {
                    var g = IdGenerators.EnchantId(id);
                    string msg;
                    if (GuidCollisionGuard.TryDetectCollision(g, "enchant", id, out msg))
                    {
                        Debug.LogError(msg);
                        RandomReinforcementsPerEncounter.Main.FailMod(msg);
                        return; 
                    }
                }

#if DEBUG
                // Prueba forzada con un blueprint vanilla existente (no lanzar)
                var bp = LootUtils.TryLoadEnchant(LootRefs.GetWeaponEnchantIdForTier(1));
                if (bp != null)
                {
                    string msg;
                    if (GuidCollisionGuard.TryDetectCollision(bp.AssetGuid, "test", "force_collision_lootref_t1", out msg))
                    {
                        Debug.LogError(msg);
                        RandomReinforcementsPerEncounter.Main.FailMod(msg);
                        return;
                    }
                }
#endif
            }
            catch (Exception ex)
            {

                Debug.LogError("[RRE] " + ex);
                RandomReinforcementsPerEncounter.Main.FailMod("RRE failed during collision check: " + ex.Message);

            }
        }
    }
}
