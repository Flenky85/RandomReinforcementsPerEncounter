using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantDebug
    {
        public static void DumpEnchant(string guid)
        {
            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(guid);
            if (bp == null) { Debug.LogError("[RRE][Dump] Not found: " + guid); return; }

            Debug.Log("====== [RRE][Dump] " + guid + " ======");
            Debug.Log("Unity name: " + bp.name + "  | Type: " + bp.GetType().FullName);

            // Props visibles
            DumpProps(bp);
            // Campos internos
            DumpFields(bp);

            // Descripción “visible”
            try { Debug.Log("[RRE][Dump] bp.Description: " + GetAsString(bp, "Description")); } catch { }
            try { Debug.Log("[RRE][Dump] field m_Description: " + GetFieldString(bp, "m_Description")); } catch { }

            // EnchantName / Prefix / Suffix
            try { Debug.Log("[RRE][Dump] EnchantName: " + GetAsString(bp, "EnchantName")); } catch { }
            try { Debug.Log("[RRE][Dump] m_EnchantName: " + GetFieldString(bp, "m_EnchantName")); } catch { }
            try { Debug.Log("[RRE][Dump] Prefix: " + GetAsString(bp, "Prefix")); } catch { }
            try { Debug.Log("[RRE][Dump] m_Prefix: " + GetFieldString(bp, "m_Prefix")); } catch { }
            try { Debug.Log("[RRE][Dump] Suffix: " + GetAsString(bp, "Suffix")); } catch { }
            try { Debug.Log("[RRE][Dump] m_Suffix: " + GetFieldString(bp, "m_Suffix")); } catch { }

            // Componentes
            var comps = bp.ComponentsArray;
            Debug.Log("[RRE][Dump] Components: " + (comps?.Length ?? 0));
            if (comps != null)
            {
                for (int i = 0; i < comps.Length; i++)
                {
                    var c = comps[i];
                    var cname = c == null ? "<null>" : c.GetType().FullName;
                    Debug.Log($"[RRE][Dump]  [{i}] {cname}");
                    if (c is WeaponEnergyDamageDice we)
                    {
                        Debug.Log($"[RRE][Dump]   -> WeaponEnergyDamageDice: {we.EnergyDamageDice.ToString()}  type={we.Element}");
                    }
                }
            }

            Debug.Log("====== [/RRE][Dump] ======");
        }

        private static void DumpProps(object o)
        {
            var t = o.GetType();
            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var p in props)
            {
                try
                {
                    var vt = p.PropertyType.FullName;
                    object v = null;
                    if (p.CanRead) v = p.GetValue(o, null);
                    var vs = ValuePreview(v);
                    Debug.Log($"[RRE][Dump][prop] {p.Name} : {vt} = {vs}");
                }
                catch { }
            }
        }

        private static void DumpFields(object o)
        {
            var t = o.GetType();
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var f in fields)
            {
                try
                {
                    var vt = f.FieldType.FullName;
                    var v = f.GetValue(o);
                    var vs = ValuePreview(v);
                    Debug.Log($"[RRE][Dump][field] {f.Name} : {vt} = {vs}");
                }
                catch { }
            }
        }

        private static string ValuePreview(object v)
        {
            if (v == null) return "null";
            var s = v.ToString();
            if (s.Length > 120) s = s.Substring(0, 120) + "...";
            return s.Replace("\n", "\\n");
        }

        private static string GetAsString(object o, string propName)
        {
            var pi = o.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (pi == null) return "<no prop>";
            var v = pi.GetValue(o, null);
            return LocalizedOrStringToText(v);
        }

        private static string GetFieldString(object o, string field)
        {
            var fi = o.GetType().GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fi == null) return "<no field>";
            var v = fi.GetValue(o);
            return LocalizedOrStringToText(v);
        }

        private static string LocalizedOrStringToText(object v)
        {
            if (v == null) return "null";
            var tname = v.GetType().FullName;
            if (tname == "System.String") return (string)v;

            if (tname == "Kingmaker.Localization.LocalizedString")
            {
                // Intenta leer m_LoadedString y/o m_Key
                var fiLoaded = v.GetType().GetField("m_LoadedString", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var fiKey = v.GetType().GetField("m_Key", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var loaded = fiLoaded != null ? (fiLoaded.GetValue(v) as string) : null;
                var key = fiKey != null ? (fiKey.GetValue(v) as string) : null;
                return $"[LS] key='{key}' loaded='{loaded}'";
            }

            return $"<{tname}>";
        }
    }

}
