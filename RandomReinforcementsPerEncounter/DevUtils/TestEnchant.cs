using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System.Reflection;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantMakerold
    {
        public static BlueprintWeaponEnchantment CloneCorrosive1d8()
        {
            // GUID del corrosive 1d6 original (según tu dump)
            var baseGuid = "633b38ff1d11de64a91d490c683ab1c8";
            var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(baseGuid);
            if (baseBp == null)
            {
                Debug.LogError("[RRE] No se pudo cargar el Corrosive base.");
                return null;
            }

            // Crear instancia nueva por Activator
            var bp = (BlueprintWeaponEnchantment)System.Activator.CreateInstance(typeof(BlueprintWeaponEnchantment));

            // Copiar todos los campos públicos y privados del original al nuevo
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var field in typeof(BlueprintWeaponEnchantment).GetFields(flags))
            {
                try
                {
                    var value = field.GetValue(baseBp);
                    field.SetValue(bp, value);
                }
                catch { }
            }

            // Cambiar nombre Unity
            bp.name = "MOD_Corrosive_1d8";
            // Cambiar GUID
            var newGuid = GuidUtil.FromString("corrosive.1d8");
            var fiGuid = typeof(BlueprintWeaponEnchantment).GetField("m_AssetGuid", flags);
            if (fiGuid != null) fiGuid.SetValue(bp, newGuid);

            // Reasignar OwnerBlueprint en todos los componentes para que apunten al nuevo BP
            if (bp.ComponentsArray != null)
            {
                foreach (var comp in bp.ComponentsArray)
                {
                    var fiOwner = comp.GetType().GetField("OwnerBlueprint", flags);
                    if (fiOwner != null) fiOwner.SetValue(comp, bp);
                }
            }

            return bp;
        }
        public static BlueprintWeaponEnchantment CloneCorrosive1d8_Safe()
        {
            const string baseGuid = "633b38ff1d11de64a91d490c683ab1c8"; // corrosive 1d6 vanilla
            var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(baseGuid);
            if (baseBp == null)
            {
                Debug.LogError("[RRE] Base Corrosive no encontrado.");
                return null;
            }

            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var mwc = typeof(object).GetMethod("MemberwiseClone", flags);
            if (mwc == null)
            {
                Debug.LogError("[RRE] No se pudo obtener MemberwiseClone.");
                return null;
            }

            // 1) Clonado shallow del objeto completo (misma clase, nuevo objeto)
            var bp = (BlueprintWeaponEnchantment)mwc.Invoke(baseBp, null);

            // 2) Nombre Unity único y nuevo GUID
            bp.name = "MOD_Corrosive_1d8";
            var newGuid = GuidUtil.FromString("corrosive.1d8");

            // set GUID (propiedad o campo)
            var setOk = false;
            try { typeof(BlueprintScriptableObject).GetProperty("AssetGuid", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(bp, newGuid, null); setOk = true; } catch { }
            if (!setOk) { try { typeof(BlueprintScriptableObject).GetField("m_AssetGuid", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(bp, newGuid); setOk = true; } catch { } }
            if (!setOk) Debug.LogWarning("[RRE] No se pudo establecer AssetGuid por reflexión (continuo).");

            // 3) Reasignar OwnerBlueprint en componentes (evita referencias al original)
            var comps = bp.ComponentsArray;
            if (comps != null)
            {
                for (int i = 0; i < comps.Length; i++)
                {
                    var fiOwner = comps[i].GetType().GetField("OwnerBlueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (fiOwner != null) { try { fiOwner.SetValue(comps[i], bp); } catch { } }
                }
            }

            // 4) NO cambiamos dados ni textos: clon idéntico al 1d6 (solo cambia GUID y name)
            return bp;
        }


    }
}
