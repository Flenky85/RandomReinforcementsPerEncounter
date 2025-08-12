using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons; // BlueprintWeaponEnchantment
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    //llamarlo
    //RandomReinforcementsPerEncounter.DevDumps.DumpWeaponEnchantsMinimal();
    //RandomReinforcementsPerEncounter.DevDumps.DumpWeaponEnchantLinks();

    public static class DevDumps
    {
        public static void DumpWeaponEnchantsMinimal()
        {
            try
            {
                var list = GetAllWeaponEnchantments();
                var sb = new StringBuilder();
                sb.AppendLine("BlueprintName,AssetGuid,Description,Name,Prefix,Suffix");

                foreach (var bp in list)
                {
                    string bpName = bp?.name ?? "";
                    string guid = GetGuid(bp);

                    string desc = GetStringPropOrLoc(bp, "Description", "m_Description");
                    string name = GetStringPropOrLoc(bp, "Name", "m_EnchantName");
                    string prefix = GetStringPropOrLoc(bp, "Prefix", "m_Prefix");
                    string suffix = GetStringPropOrLoc(bp, "Suffix", "m_Suffix");

                    sb.AppendLine(string.Join(",", Csv(bpName), Csv(guid), Csv(desc), Csv(name), Csv(prefix), Csv(suffix)));
                }

                var path = GetDefaultDumpPath("WOTR_WeaponEnchantments_MIN.csv");
                File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
                Debug.Log($"[DevDumps] ✅ CSV escrito: {path} (count={list.Count})");
            }
            catch (Exception ex)
            {
                Debug.LogError("[DevDumps] ❌ " + ex);
            }
        }

        // ===== helpers =====

        private static string GetDefaultDumpPath(string fileName)
        {
            try
            {
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (!string.IsNullOrEmpty(desktop)) return Path.Combine(desktop, fileName);
            }
            catch { }
            try { return Path.Combine(Application.persistentDataPath, fileName); }
            catch { return fileName; }
        }

        private static string Csv(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Replace("\"", "\"\"");
            if (s.IndexOfAny(new[] { ',', '\n', '\r', '"' }) >= 0) return "\"" + s + "\"";
            return s;
        }

        private static string GetGuid(SimpleBlueprint bp)
        {
            if (bp == null) return "";
            try
            {
                var prop = typeof(SimpleBlueprint).GetProperty("AssetGuidThreadSafe", BindingFlags.Instance | BindingFlags.Public);
                if (prop != null) return prop.GetValue(bp, null)?.ToString() ?? "";
            }
            catch { }
            try
            {
                var fld = typeof(SimpleBlueprint).GetField("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fld != null) return fld.GetValue(bp)?.ToString() ?? "";
            }
            catch { }
            return "";
        }

        // Intenta propiedad string (Description/Name/Prefix/Suffix). Si no, cae a LocalizedString (m_Description, etc.)
        private static string GetStringPropOrLoc(object obj, string propName, string locFieldName)
        {
            if (obj == null) return "";
            // 1) Propiedad pública tipo string
            try
            {
                var prop = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public);
                if (prop != null && prop.PropertyType == typeof(string))
                {
                    var val = prop.GetValue(obj, null) as string;
                    if (!string.IsNullOrEmpty(val)) return val;
                }
            }
            catch { }

            // 2) Campo LocalizedString (m_*)
            try
            {
                var fld = obj.GetType().GetField(locFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fld != null)
                {
                    var loc = fld.GetValue(obj);
                    var txt = SafeLocToString(loc);
                    if (!string.IsNullOrEmpty(txt)) return txt;
                }
            }
            catch { }

            return "";
        }

        private static string SafeLocToString(object localizedString)
        {
            try { return localizedString?.ToString() ?? ""; } catch { return ""; }
        }

        /// Enumeración robusta (sin referenciar BlueprintsCache en compile-time)
        private static List<BlueprintWeaponEnchantment> GetAllWeaponEnchantments()
        {
            var result = new List<BlueprintWeaponEnchantment>();

            // Intento 1: Kingmaker.Blueprints.JsonSystem.BlueprintsCache dentro de ResourcesLibrary
            try
            {
                var resType = typeof(ResourcesLibrary);
                var asm = resType.Assembly;
                var cacheType = asm.GetType("Kingmaker.Blueprints.JsonSystem.BlueprintsCache");
                if (cacheType != null)
                {
                    object cacheInstance = null;
                    foreach (var f in resType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (cacheType.IsAssignableFrom(f.FieldType))
                        {
                            cacheInstance = f.GetValue(null);
                            if (cacheInstance != null) break;
                        }
                    }
                    if (cacheInstance != null)
                    {
                        var fldLoaded = cacheType.GetField("m_LoadedBlueprints", BindingFlags.Instance | BindingFlags.NonPublic);
                        var dict = fldLoaded?.GetValue(cacheInstance) as IEnumerable;
                        if (dict != null)
                        {
                            foreach (var kv in dict)
                            {
                                var valProp = kv.GetType().GetProperty("Value");
                                var value = valProp?.GetValue(kv, null);
                                if (value == null) continue;

                                var bpField = value.GetType().GetField("Blueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                                var sb = bpField?.GetValue(value) as SimpleBlueprint;
                                if (sb is BlueprintWeaponEnchantment bwe) result.Add(bwe);
                            }
                            if (result.Count > 0) return result;
                        }
                    }
                }
            }
            catch { }

            // Intento 2: diccionario interno en ResourcesLibrary (s_Blueprints)
            try
            {
                var libType = typeof(ResourcesLibrary);
                var fld = libType.GetField("s_Blueprints", BindingFlags.Static | BindingFlags.NonPublic);
                var dict = fld?.GetValue(null) as IDictionary;
                if (dict != null)
                {
                    foreach (DictionaryEntry de in dict)
                        if (de.Value is BlueprintWeaponEnchantment bwe) result.Add(bwe);
                }
            }
            catch { }

            return result;
        }
        public static void DumpWeaponEnchantLinks()
        {
            try
            {
                var weapons = GetAllBlueprintItemWeapons();
                var sb = new StringBuilder();
                sb.AppendLine(string.Join(",",
                    "WeaponAssetGuid",
                    "WeaponName",
                    "WeaponDescription",
                    "EnchantAssetGuid",
                    "EnchantName",
                    "EnchantDescription"
                ));

                foreach (var w in weapons)
                {
                    var wGuid = GetGuid(w);
                    var wName = GetStringPropOrLoc(w, "Name", "m_DisplayName");      // algunas builds usan DisplayName
                    if (string.IsNullOrEmpty(wName)) wName = GetStringPropOrLoc(w, "m_DisplayName", "m_DisplayName");
                    var wDesc = GetStringPropOrLoc(w, "Description", "m_Description");

                    // Enchantments: List<BlueprintItemEnchantment>
                    var enchList = GetWeaponEnchantments(w);
                    if (enchList == null || enchList.Count == 0)
                    {
                        // si no tiene encantamientos, salta
                        continue;
                    }

                    foreach (var e in enchList)
                    {
                        if (e == null) continue;
                        var eGuid = GetGuid(e);
                        // para nombre del encantamiento: Name/EnchantName
                        var eName = GetStringPropOrLoc(e, "Name", "m_EnchantName");
                        if (string.IsNullOrEmpty(eName)) eName = GetStringPropOrLoc(e, "EnchantName", "m_EnchantName");
                        var eDesc = GetStringPropOrLoc(e, "Description", "m_Description");

                        sb.AppendLine(string.Join(",",
                            Csv(wGuid),
                            Csv(wName),
                            Csv(wDesc),
                            Csv(eGuid),
                            Csv(eName),
                            Csv(eDesc)
                        ));
                    }
                }

                var path = GetDefaultDumpPath("WOTR_Weapon_EnchantLinks.csv");
                File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
                Debug.Log($"[DevDumps] ✅ CSV escrito: {path} (weapons={weapons.Count})");
            }
            catch (Exception ex)
            {
                Debug.LogError("[DevDumps] ❌ " + ex);
            }
        }

        // ===== helpers existentes / reutilizados =====
        /*
        private static string GetDefaultDumpPath(string fileName)
        {
            try { var d = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); if (!string.IsNullOrEmpty(d)) return Path.Combine(d, fileName); } catch { }
            try { return Path.Combine(Application.persistentDataPath, fileName); } catch { return fileName; }
        }*/
        /*
        private static string Csv(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Replace("\"", "\"\"");
            if (s.IndexOfAny(new[] { ',', '\n', '\r', '"' }) >= 0) return "\"" + s + "\"";
            return s;
        }
        *//*
        private static string GetGuid(SimpleBlueprint bp)
        {
            if (bp == null) return "";
            try
            {
                var prop = typeof(SimpleBlueprint).GetProperty("AssetGuidThreadSafe", BindingFlags.Instance | BindingFlags.Public);
                if (prop != null) return prop.GetValue(bp, null)?.ToString() ?? "";
            }
            catch { }
            try
            {
                var fld = typeof(SimpleBlueprint).GetField("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fld != null) return fld.GetValue(bp)?.ToString() ?? "";
            }
            catch { }
            return "";
        }*/

        // Lee string directo (prop) o LocalizedString (campo m_*)
        /*private static string GetStringPropOrLoc(object obj, string propName, string locFieldName)
        {
            if (obj == null) return "";
            // 1) propiedad pública string
            try
            {
                var prop = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public);
                if (prop != null && prop.PropertyType == typeof(string))
                {
                    var val = prop.GetValue(obj, null) as string;
                    if (!string.IsNullOrEmpty(val)) return val;
                }
            }
            catch { }

            // 2) campo LocalizedString
            try
            {
                var fld = obj.GetType().GetField(locFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fld != null)
                {
                    var loc = fld.GetValue(obj);
                    var txt = loc?.ToString();
                    if (!string.IsNullOrEmpty(txt)) return txt;
                }
            }
            catch { }

            return "";
        }*/

        private static List<BlueprintItemWeapon> GetAllBlueprintItemWeapons()
        {
            var result = new List<BlueprintItemWeapon>();

            // Intento 1: BlueprintsCache (instancia guardada en ResourcesLibrary), vía reflexión
            try
            {
                var resType = typeof(ResourcesLibrary);
                var asm = resType.Assembly;
                var cacheType = asm.GetType("Kingmaker.Blueprints.JsonSystem.BlueprintsCache");
                if (cacheType != null)
                {
                    object cacheInstance = null;
                    foreach (var f in resType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (cacheType.IsAssignableFrom(f.FieldType))
                        {
                            cacheInstance = f.GetValue(null);
                            if (cacheInstance != null) break;
                        }
                    }
                    var fldLoaded = cacheType?.GetField("m_LoadedBlueprints", BindingFlags.Instance | BindingFlags.NonPublic);
                    var dict = fldLoaded?.GetValue(cacheInstance) as IEnumerable;
                    if (dict != null)
                    {
                        foreach (var kv in dict)
                        {
                            var valProp = kv.GetType().GetProperty("Value");
                            var value = valProp?.GetValue(kv, null);
                            if (value == null) continue;

                            var bpField = value.GetType().GetField("Blueprint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                            var sb = bpField?.GetValue(value) as SimpleBlueprint;
                            if (sb is BlueprintItemWeapon w) result.Add(w);
                        }
                        if (result.Count > 0) return result;
                    }
                }
            }
            catch { }

            // Intento 2: diccionario interno en ResourcesLibrary (s_Blueprints)
            try
            {
                var libType = typeof(ResourcesLibrary);
                var fld = libType.GetField("s_Blueprints", BindingFlags.Static | BindingFlags.NonPublic);
                var dict = fld?.GetValue(null) as IDictionary;
                if (dict != null)
                {
                    foreach (DictionaryEntry de in dict)
                        if (de.Value is BlueprintItemWeapon w) result.Add(w);
                }
            }
            catch { }

            return result;
        }

        // Obtiene la lista de encantamientos de un arma, tolerando variantes de campo/propiedad
        private static List<BlueprintItemEnchantment> GetWeaponEnchantments(BlueprintItemWeapon weapon)
        {
            if (weapon == null) return new List<BlueprintItemEnchantment>();

            // 1) Propiedad pública "Enchantments" (List<BlueprintItemEnchantment>)
            try
            {
                var prop = weapon.GetType().GetProperty("Enchantments", BindingFlags.Instance | BindingFlags.Public);
                if (prop != null)
                {
                    var list = prop.GetValue(weapon, null) as IEnumerable;
                    if (list != null)
                    {
                        var res = new List<BlueprintItemEnchantment>();
                        foreach (var e in list) if (e is BlueprintItemEnchantment be) res.Add(be);
                        return res;
                    }
                }
            }
            catch { }

            // 2) Campo privado "m_Enchantments"
            try
            {
                var fld = weapon.GetType().GetField("m_Enchantments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fld != null)
                {
                    var list = fld.GetValue(weapon) as IEnumerable;
                    if (list != null)
                    {
                        var res = new List<BlueprintItemEnchantment>();
                        foreach (var e in list) if (e is BlueprintItemEnchantment be) res.Add(be);
                        return res;
                    }
                }
            }
            catch { }

            return new List<BlueprintItemEnchantment>();
        }
    }

}
