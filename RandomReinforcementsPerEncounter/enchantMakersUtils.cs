using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Reflection;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.ElementsSystem;           // ActionList, GameAction (por si luego lo usas)
using System.Collections.Generic;         // List<>
using Newtonsoft.Json;

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantMakerUtils
    {
        public static bool BlueprintExists(string gidStr)
            => ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr) != null;

        public static BlueprintWeaponEnchantment MakeCloneWithGuid(BlueprintWeaponEnchantment baseBp, string name, BlueprintGuid gid)
        {
            var bp = ShallowClone(baseBp);
            bp.name = "RRE_" + name.Replace(' ', '_');
            TrySetGuidDeep(bp, gid);
            return bp;
        }

        private static void TryClearField(object obj, string fieldName)
        {
            var fi = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fi != null)
            {
                try
                {
                    var ft = fi.FieldType;
                    object val = null;
                    if (ft.IsArray) val = Array.CreateInstance(ft.GetElementType(), 0);
                    fi.SetValue(obj, val);
                }
                catch { /* ignore */ }
            }
        }

        // Copia componentes y re-asigna Owner; permite mutarlos con un delegado
        public static void CopyComponentsWithOwnerRemap(
            BlueprintWeaponEnchantment from,
            BlueprintWeaponEnchantment to,
            Func<BlueprintComponent, BlueprintComponent> mutator = null)
        {
            var compsBase = from.ComponentsArray;
            if (compsBase == null) { to.ComponentsArray = null; return; }

            var compsNew = new BlueprintComponent[compsBase.Length];
            for (int i = 0; i < compsBase.Length; i++)
            {
                var cClone = ShallowClone(compsBase[i]);
                if (mutator != null) cClone = mutator(cClone);

                var fiOwner = cClone?.GetType().GetField("OwnerBlueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fiOwner != null) { try { fiOwner.SetValue(cClone, to); } catch { } }

                compsNew[i] = cClone;
            }
            to.ComponentsArray = compsNew;
        }

        public static void EnableAndRegister(BlueprintWeaponEnchantment bp, BlueprintGuid gid)
        {
            try
            {
                var onEnable = typeof(BlueprintScriptableObject).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                onEnable?.Invoke(bp, null);
            }
            catch { }

            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(gid, bp);
        }
        // --- Clonado superficial genérico ---
        public static T ShallowClone<T>(T source) where T : class
        {
            if (source == null) return null;
            var mwc = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)mwc.Invoke(source, null);
        }

        // --- GUID: set profundo en el blueprint clonado ---
        public static bool TrySetGuidDeep(object obj, BlueprintGuid value)
        {
            var t = obj.GetType();
            while (t != null)
            {
                var pi = t.GetProperty("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (pi != null && pi.CanWrite)
                {
                    try { pi.SetValue(obj, value, null); return true; } catch { }
                }

                var fi = t.GetField("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi != null)
                {
                    try { fi.SetValue(obj, value); return true; } catch { }
                }

                var fi2 = t.GetField("m_AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi2 != null)
                {
                    try { fi2.SetValue(obj, value); return true; } catch { }
                }

                t = t.BaseType;
            }
            return false;
        }

        // --- Localización: escribir en el pack y apuntar LocalizedString nuevas ---
        public static void SetEnchantTextViaPack(BlueprintWeaponEnchantment bp, string gidStr, string name, string prefix, string suffix, string desc)
        {
            if (LocalizationManager.CurrentPack == null)
                LocalizationManager.WaitForInit();

            PutAndPointLS(bp, "m_EnchantName", "rre." + gidStr + ".name", name);
            PutAndPointLS(bp, "m_Prefix", "rre." + gidStr + ".pref", prefix ?? "");
            PutAndPointLS(bp, "m_Suffix", "rre." + gidStr + ".suff", suffix ?? "");
            PutAndPointLS(bp, "m_Description", "rre." + gidStr + ".desc", desc ?? "");
        }

        private static void PutAndPointLS(object owner, string fieldName, string key, string text)
        {
            if (LocalizationManager.CurrentPack != null)
                LocalizationManager.CurrentPack.PutString(key, text ?? "");

            var field = GetFieldDeep(owner.GetType(), fieldName);
            if (field == null) return;

            var lsType = field.FieldType; // Kingmaker.Localization.LocalizedString
            var newLs = System.Activator.CreateInstance(lsType);

            var keyProp = lsType.GetProperty("Key", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var procProp = lsType.GetProperty("ShouldProcess", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var sharedFi = lsType.GetField("Shared", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            try { if (keyProp != null) keyProp.SetValue(newLs, key, null); } catch { }
            try { if (procProp != null) procProp.SetValue(newLs, true, null); } catch { }
            try { if (sharedFi != null) sharedFi.SetValue(newLs, null); } catch { }

            field.SetValue(owner, newLs);
        }

        private static FieldInfo GetFieldDeep(System.Type t, string name)
        {
            while (t != null)
            {
                var fi = t.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fi != null) return fi;
                t = t.BaseType;
            }
            return null;
        }

        // --- Texto de dados para descripciones ---
        public static string DiceToText(DiceFormula d)
        {
            var s = d.ToString();
            if (!string.IsNullOrEmpty(s)) return s;

            // Fallback defensivo por si cambia la implementación
            try
            {
                var t = d.GetType();
                var rolls = (int)(t.GetField("m_Rolls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(d) ?? 1);
                var dice = t.GetField("m_Dice", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(d);
                var dieS = dice != null ? dice.ToString().TrimStart('D') : "6";
                return rolls.ToString() + "d" + dieS;
            }
            catch { return "1d6"; }
        }
        public static BlueprintBuff MakeBuffCloneWithGuid(BlueprintBuff baseBp, string name, BlueprintGuid gid)
        {
            var bp = ShallowClone(baseBp);

            bp.name = "RRE_" + name.Replace(' ', '_');
            bp.ComponentsArray = Array.Empty<BlueprintComponent>();

            TryClearField(bp, "PrototypeLink");
            TryClearField(bp, "m_Overrides");

            TrySetGuidDeep(bp, gid);
            return bp;
        }

        public static void CopyBuffComponentsWithOwnerRemap(
            BlueprintBuff from,
            BlueprintBuff to,
            Func<BlueprintComponent, BlueprintComponent> mutator = null)
        {
            var compsBase = from.ComponentsArray;
            if (compsBase == null) { to.ComponentsArray = null; return; }

            var compsNew = new BlueprintComponent[compsBase.Length];
            for (int i = 0; i < compsBase.Length; i++)
            {
                var cClone = ShallowClone(compsBase[i]);
                if (mutator != null) cClone = mutator(cClone);

                var fiOwner = cClone?.GetType().GetField("OwnerBlueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fiOwner != null) { try { fiOwner.SetValue(cClone, to); } catch { } }

                compsNew[i] = cClone;
            }
            to.ComponentsArray = compsNew;
        }

        public static void RegisterBuff(BlueprintBuff bp, BlueprintGuid gid)
        {
            try
            {
                var onEnable = typeof(BlueprintScriptableObject)
                    .GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                onEnable?.Invoke(bp, null);
            }
            catch { }

            ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(gid, bp);
        }
    }
}
