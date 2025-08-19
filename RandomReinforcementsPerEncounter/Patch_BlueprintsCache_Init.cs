using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using System;
using System.Collections.Generic;
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

    [HarmonyPatch(typeof(ItemEntity), "get_Name")]
    static class Patch_ItemEntity_get_Name_ComboRRE
    {
        // Materiales/componentes: se añaden TODOS los que estén
        private static readonly string[] MaterialIds = {
            "b38844e2bffbac48b63036b66e735be", // MasterWork
            "e6a7a2b6f26b488783c612add1e9a8bd", // Druchite
            "e5990dc76d2a613409916071c898eee8", // ColdIron
            "0ae8fc9f2e255584faf4d14835224875", // Mithral
            "ab39e7d59dd12f4429ffef5dca88dc7b", // Adamantine
            "c3209eb058d471548928a200d70765e0"  // Composite
        };

        private static readonly string[] WeaponEnhancementIds = {
            "d42fc23b92c640846ac137dc26e000d4", // Weapon +1
            "eb2faccc4c9487d43b3575d7e77ff3f5", // Weapon +2
            "80bb8a737579e35498177e1e3c75899b", // Weapon +3
            "783d7d496da6ac44f9511011fc5f1979", // Weapon +4
            "bdba267e951851449af552aa9f9e3992", // Weapon +5
            "0326d02d2e24d254a9ef626cc7a3850f", // Weapon +6
        };

        static void Postfix(ItemEntity __instance, ref string __result)
        {
            if (__instance == null || !__instance.IsIdentified || string.IsNullOrEmpty(__result))
                return;

            var enchBps = __instance.Enchantments?
                .Select(e => e?.Blueprint)
                .Where(bp => bp != null)
                .ToList();
            if (enchBps == null || enchBps.Count == 0) return;

            // 1) GATILLO: ¿hay al menos un enchant con tu componente de precio?
            bool hasRRE = enchBps.Any(bp => bp.GetComponent<RRE_PriceDeltaComponent>() != null);
            if (!hasRRE) return;

            // 2) PREFIJOS: materiales (todos) + un RRE_ principal
            var prefixes = new List<string>();

            // Materiales
            foreach (var id in MaterialIds)
            {
                var mat = enchBps.FirstOrDefault(bp => bp.AssetGuid.ToString()
                    .Equals(id, StringComparison.OrdinalIgnoreCase));
                if (mat == null) continue;

                var text = GetPrefixLikeText(mat);
                if (!string.IsNullOrWhiteSpace(text))
                    prefixes.Add(text);
            }

            // RRE principal
            var mainEnchant = enchBps
                .Where(bp => (bp.name?.StartsWith("RRE_", StringComparison.OrdinalIgnoreCase) ?? false)
                             && bp.GetComponent<RRE_PriceDeltaComponent>() == null)
                .OrderByDescending(bp => bp.EnchantmentCost)
                .ThenBy(bp => bp.AssetGuid.ToString(), StringComparer.Ordinal)
                .FirstOrDefault();

            var mainPrefix = GetPrefixLikeText(mainEnchant);
            if (!string.IsNullOrWhiteSpace(mainPrefix))
                prefixes.Add(mainPrefix);

            // Aplicar prefijos si hay
            var finalPrefix = prefixes.Count > 0 ? string.Join(" ", prefixes).Trim() : null;
            if (!string.IsNullOrWhiteSpace(finalPrefix) &&
                !__result.StartsWith(finalPrefix + " ", StringComparison.OrdinalIgnoreCase))
            {
                __result = $"{finalPrefix} {__result}";
            }

            // 3) SUFIJO: enhancement de arma (solo uno por arma)
            // (Opcional) limitar a armas: if (!(__instance is ItemEntityWeapon)) return;
            BlueprintItemEnchantment weaponEnh = null;
            foreach (var id in WeaponEnhancementIds)
            {
                weaponEnh = enchBps.FirstOrDefault(bp => bp.AssetGuid.ToString()
                    .Equals(id, StringComparison.OrdinalIgnoreCase));
                if (weaponEnh != null) break;
            }

            var suffix = GetSuffixLikeText(weaponEnh);
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                // Evitar duplicados si ya contiene el sufijo
                if (__result.IndexOf(" " + suffix, StringComparison.OrdinalIgnoreCase) < 0)
                    __result = $"{__result} {suffix}";
            }
        }

        // Prefiere Prefix; si no, m_EnchantName (arma); si no, name interno
        private static string GetPrefixLikeText(BlueprintItemEnchantment bp)
        {
            if (bp == null) return null;

            if (!string.IsNullOrWhiteSpace(bp.Prefix))
                return bp.Prefix.Trim();

            if (bp is BlueprintWeaponEnchantment bwe)
            {
                var t = bwe.m_EnchantName?.ToString();
                if (!string.IsNullOrWhiteSpace(t))
                    return t.Trim();
            }

            return bp.name?.Trim();
        }

        // Para Enhancement: Suffix explícito o "+{cost}" como fallback
        private static string GetSuffixLikeText(BlueprintItemEnchantment bp)
        {
            if (bp == null) return null;

            if (!string.IsNullOrWhiteSpace(bp.Suffix))
                return bp.Suffix.Trim();

            // Enhancement1..6 suelen no traer Suffix -> formateamos "+N"
            var cost = bp.EnchantmentCost;
            if (cost > 0 && (bp.name?.StartsWith("Enhancement", StringComparison.OrdinalIgnoreCase) ?? false))
                return $"+{cost}";

            // Último recurso: no añadir nada raro
            return null;
        }
    }
}
