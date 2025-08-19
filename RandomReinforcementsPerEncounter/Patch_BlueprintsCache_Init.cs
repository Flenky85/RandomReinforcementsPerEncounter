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
        private static readonly string[] MaterialIdsOrdered = {
            "d8e1ebc1062d8cc42abff78783856b0d", // Oversized
            "b38844e2bffbac48b63036b66e735be", // MasterWork
            "e6a7a2b6f26b488783c612add1e9a8bd", // Druchite
            "e5990dc76d2a613409916071c898eee8", // ColdIron
            "0ae8fc9f2e255584faf4d14835224875", // Mithral
            "ab39e7d59dd12f4429ffef5dca88dc7b", // Adamantine
            "c3209eb058d471548928a200d70765e0"  // Composite
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

            // 2) MATERIALES: añade TODOS los que estén en el orden definido
            var materialPrefixes = new List<string>();
            foreach (var id in MaterialIdsOrdered)
            {
                var mat = enchBps.FirstOrDefault(bp =>
                    bp.AssetGuid.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));
                if (mat == null) continue;

                var text = GetPrefixLikeText(mat);
                if (!string.IsNullOrWhiteSpace(text))
                    materialPrefixes.Add(text);
            }

            // 3) ENCANTAMIENTO PRINCIPAL:
            //    uno cuyo internal name empiece por "RRE_" y NO tenga RRE_PriceDeltaComponent
            var mainEnchant = enchBps
                .Where(bp => (bp.name?.StartsWith("RRE_", StringComparison.OrdinalIgnoreCase) ?? false)
                             && bp.GetComponent<RRE_PriceDeltaComponent>() == null)
                .OrderByDescending(bp => bp.EnchantmentCost)                 // criterio “mejor”
                .ThenBy(bp => bp.AssetGuid.ToString(), StringComparer.Ordinal)
                .FirstOrDefault();

            var mainPrefix = GetPrefixLikeText(mainEnchant);

            // 4) Construcción del nombre final
            var prefixes = new List<string>();
            prefixes.AddRange(materialPrefixes);
            if (!string.IsNullOrWhiteSpace(mainPrefix))
                prefixes.Add(mainPrefix);

            if (prefixes.Count == 0) return;

            var finalPrefix = string.Join(" ", prefixes).Trim();
            if (string.IsNullOrEmpty(finalPrefix)) return;

            // Evitar repetir si ya arranca exactamente igual
            if (!__result.StartsWith(finalPrefix + " ", StringComparison.OrdinalIgnoreCase))
                __result = $"{finalPrefix} {__result}";
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
    }




    /*
    [HarmonyPatch(typeof(ItemEntity), "get_Name")]
    static class Patch_ItemEntity_get_Name
    {
        // Mapa GUID -> prioridad (0 es la más alta)
        private static Dictionary<string, int> _prio;
        private static void EnsurePrio()
        {
            if (_prio != null) return;
            var ordered = new[]
            {
                //"d8e1ebc1062d8cc42abff78783856b0d", // Oversized
                "b38844e2bffbac48b63036b66e735be", // MasterWork
                "e6a7a2b6f26b488783c612add1e9a8bd", // Druchite
                "e5990dc76d2a613409916071c898eee8", // ColdIron
                "0ae8fc9f2e255584faf4d14835224875", // Mithral
                "ab39e7d59dd12f4429ffef5dca88dc7b", // Adamantine
                "c3209eb058d471548928a200d70765e0"  // Composite
            };
            _prio = ordered
                .Select((id, idx) => new { id = id.ToLowerInvariant(), idx })
                .ToDictionary(x => x.id, x => x.idx);
        }

        static void Postfix(ItemEntity __instance, ref string __result)
        {
            if (__instance == null || __instance.Blueprint == null) return;
            if (!__instance.IsIdentified) return;             // por si algún caso no está identificado
            if (string.IsNullOrEmpty(__result)) return;

            EnsurePrio();

            // Si quieres restringir además a tus ítems RRE_, descomenta:
            // var bpName = __instance.Blueprint.name ?? string.Empty;
            // if (!bpName.StartsWith("RRE_", StringComparison.Ordinal)) return;

            // 1) Buscar entre LOS TUSOS: enchants que tengan RRE_PriceDeltaComponent
            //    y que además estén en la lista de prioridad
            var chosen = __instance.Enchantments?
                .Select(e => e?.Blueprint)
                .Where(bp => bp != null
                             && bp.GetComponent<RRE_PriceDeltaComponent>() != null) // <- tu misma señal que en el coste
                .Select(bp => new
                {
                    bp,
                    id = bp.AssetGuid.ToString().ToLowerInvariant(),
                    hasPrio = _prio.TryGetValue(bp.AssetGuid.ToString().ToLowerInvariant(), out var _),
                    prio = _prio.TryGetValue(bp.AssetGuid.ToString().ToLowerInvariant(), out var p) ? p : int.MaxValue
                })
                .Where(x => x.hasPrio)
                .OrderBy(x => x.prio)
                .Select(x => x.bp)
                .FirstOrDefault();

            if (chosen == null) return;

            // 2) Determinar el texto del prefijo
            //    - Primero bp.Prefix si existe
            //    - Si no, m_EnchantName (en armas), y si tampoco, el name interno
            string prefix = null;

            // algunos enchants tienen Prefix/Suffix directamente en BlueprintItemEnchantment
            if (!string.IsNullOrWhiteSpace(chosen.Prefix))
                prefix = chosen.Prefix;
            else if (chosen is BlueprintWeaponEnchantment bwe && !string.IsNullOrWhiteSpace(bwe.m_EnchantName?.ToString()))
                prefix = bwe.m_EnchantName.ToString();
            else
                prefix = chosen.name;

            prefix = prefix?.Trim();
            if (string.IsNullOrEmpty(prefix)) return;

            // 3) Evitar duplicados (si ya empieza por ese prefijo)
            if (__result.StartsWith(prefix + " ", StringComparison.OrdinalIgnoreCase))
                return;

            __result = $"{prefix} {__result}";
        }
    }*/
}
