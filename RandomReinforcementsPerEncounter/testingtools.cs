using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using UnityEngine;

//Testear si esta cargada y aplicar
//RandomReinforcementsPerEncounter.TestingTools.CheckEnchantmentInCache("flaming.t6");
//RandomReinforcementsPerEncounter.TestingTools.ApplyEnchantmentToMain("frost.t3");


namespace RandomReinforcementsPerEncounter
{
    public static class TestingTools
    {
        /// <summary>
        /// Comprueba si un enchant con este ID está en caché.
        /// </summary>
        public static void CheckEnchantmentInCache(string enchantId)
        {
            try
            {
                var gidStr = GuidUtil.FromString(enchantId).ToString();
                var bp = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
                Debug.Log(bp != null
                    ? $"[RRE-Test] Enchant '{enchantId}' está en caché ✅"
                    : $"[RRE-Test] Enchant '{enchantId}' NO está en caché ❌");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[RRE-Test] Error comprobando enchant '{enchantId}': {ex}");
            }
        }

        /// <summary>
        /// Aplica el enchant al arma de la mano principal del protagonista.
        /// </summary>
        public static void ApplyEnchantmentToMain(string enchantId)
        {
            try
            {
                var gidStr = GuidUtil.FromString(enchantId).ToString();
                var ench = ResourcesLibrary.TryGetBlueprint<BlueprintWeaponEnchantment>(gidStr);
                if (ench == null)
                {
                    Debug.LogError($"[RRE-Test] Enchant '{enchantId}' no encontrado en caché.");
                    return;
                }

                var unit = Game.Instance?.Player?.MainCharacter.Value;
                var weapon = unit?.Body?.PrimaryHand?.Weapon;
                if (weapon == null)
                {
                    Debug.LogError("[RRE-Test] No hay arma en la mano principal.");
                    return;
                }

                weapon.AddEnchantment(ench, null);
                Debug.Log($"[RRE-Test] Enchant '{enchantId}' aplicado al arma principal.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[RRE-Test] Error aplicando enchant '{enchantId}': {ex}");
            }
        }
    }
}
