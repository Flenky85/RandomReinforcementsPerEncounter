using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Components;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        /// <summary>
        /// Crea enchants ocultos por tier para ajustar el precio (no consumen budget).
        /// Genera: RRE_Price_20, RRE_Price_40, RRE_Price_80, ...
        /// </summary>
        public static void RegisterWeaponPriceForTiers(
            List<EnchantTierConfig> tiers,
            int baseDelta = 20,
            string bpPrefix = "RRE_Price_")
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int delta = baseDelta << i; // 20, 40, 80, 160, ...

                string bpName = $"{bpPrefix}{delta}";

                WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)   // GUID viene desde TierConfig
                    .SetEnchantmentCost(0)    // no consume “budget” de bonos
                    .SetHiddenInUI(true)      // oculto en la UI
                    .AddComponent<RRE_PriceDeltaComponent>(c => { c.Delta = delta; })
                    .Configure();
            }
        }
    }
}
