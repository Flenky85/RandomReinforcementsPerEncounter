using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.JsonSystem; // tipos: BlueprintsCache
using System.Reflection;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class RREConsole
    {
        public static void RegisterCorrosive1d8_ViaResLib()
        {
            try
            {
                var gid = GuidUtil.EnchantGuid("corrosive.1d8");
                var gidStr = gid.ToString();

                // ¿ya está?
                var already = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
                if (already != null) { Debug.Log("[RRE] Ya estaba en cache: " + gidStr); return; }

                // base corrosive 1d6 (tu GUID)
                const string baseGuid = "633b38ff1d11de64a91d490c683ab1c8";
                var baseBp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(baseGuid);
                if (baseBp == null) { Debug.LogError("[RRE] Corrosive base no encontrado."); return; }

                // clon shallow seguro
                var mwc = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                var bp = (BlueprintWeaponEnchantment)mwc.Invoke(baseBp, null);
                bp.name = "MOD_Corrosive_1d8";

                // (opcional) alinear el AssetGuid interno; si falla, no pasa nada
                TrySetGuidDeep(bp, gid);

                // IMPORTANT: AddCachedBlueprint (de instancia) a través de ResourcesLibrary.BlueprintsCache
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(gid, bp);

                Debug.Log("[RRE] Registrado clon via ResourcesLibrary.BlueprintsCache.AddCachedBlueprint");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[RRE] RegisterCorrosive1d8_ViaResLib EX: " + ex);
            }
        }

        private static bool TrySetGuidDeep(object obj, BlueprintGuid value)
        {
            var t = obj.GetType();
            while (t != null)
            {
                var pi = t.GetProperty("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (pi != null && pi.CanWrite) { try { pi.SetValue(obj, value, null); return true; } catch { } }
                var fi = t.GetField("AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi != null) { try { fi.SetValue(obj, value); return true; } catch { } }
                var fi2 = t.GetField("m_AssetGuid", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fi2 != null) { try { fi2.SetValue(obj, value); return true; } catch { } }
                t = t.BaseType;
            }
            return false;
        }
        // Comprueba si el enchant está en caché
        public static void CheckCorrosive1d8()
        {
            var gidStr = GuidUtil.EnchantGuid("corrosive.1d8").ToString();
            var bp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
            Debug.Log(bp != null ? "[RRE] Enchant corrosive.1d8 está en caché" : "[RRE] Enchant corrosive.1d8 NO está en caché");
        }

        // Aplica el enchant al arma de la mano principal del protagonista
        public static void ApplyCorrosive1d8ToMain()
        {
            var gidStr = GuidUtil.EnchantGuid("corrosive.1d8").ToString();
            var ench = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
            if (ench == null) { Debug.LogError("[RRE] Enchant no encontrado. Llama antes a RegisterCorrosive1d8_ViaResLib()."); return; }

            var unit = Game.Instance?.Player?.MainCharacter.Value;
            var weapon = unit?.Body?.PrimaryHand?.Weapon;
            if (weapon == null) { Debug.LogError("[RRE] No hay arma en la mano principal."); return; }

            weapon.AddEnchantment(ench, null);
            Debug.Log("[RRE] Enchant corrosive.1d8 aplicado al arma principal.");
        }

        // (Opcional) Quita el enchant de la caché si quieres limpiar
        public static void UnregisterCorrosive1d8()
        {
            var gid = GuidUtil.EnchantGuid("corrosive.1d8");
            Kingmaker.Blueprints.ResourcesLibrary.BlueprintsCache.RemoveCachedBlueprint(gid);
            Debug.Log("[RRE] Enchant corrosive.1d8 eliminado de la caché.");
        }

        // (Comodín) Registrar y aplicar de una
        public static void RegisterAndApplyCorrosive1d8ToMain()
        {
            RegisterCorrosive1d8_ViaResLib();
            ApplyCorrosive1d8ToMain();
        }
    }
}
