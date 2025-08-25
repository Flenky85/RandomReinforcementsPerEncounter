using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Components;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        public static void RegisterWeaponPriceForTiers(
            List<EnchantTierConfig> tiers,
            int baseDelta = 200,
            string bpPrefix = "RRE_Price_")
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int delta = baseDelta << i; 

                string bpName = $"{bpPrefix}{delta}";

                WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)   
                    .SetEnchantmentCost(0)    
                    .SetHiddenInUI(true)      
                    .AddComponent<RRE_PriceDeltaComponent>(c => { c.Delta = delta; })
                    .Configure();
            }
        }
    }
}
