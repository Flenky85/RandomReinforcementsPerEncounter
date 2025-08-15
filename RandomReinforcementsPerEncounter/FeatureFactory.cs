using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter
{
    internal class FeatureFactory
    {
        public static void RegisterSpellsDCTiersFor(
            List<DebuffTierConfig> tiers,
            string nameRoot,                 
            string type                 

        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int plus = t.Bonus <= 0 ? 1 : t.Bonus;

                var featGuid = GuidUtil.FromString(t.Seed);

                FeatureConfigurator
                    .New("RRE_DC_Bonus_1", GuidUtil.CreateNew().ToString())
                    .SetDisplayName("Hidden") // No visible en UI
                    .SetDescription("Hidden")
                    .SetHideInUI(true)
                    .AddIncreaseSpellDC(
                        bonusDC: 1,
                        descriptor: ModifierDescriptor.Enhancement
                    )
                    .Configure()
                    .ToReference<BlueprintFeatureReference>();
            }
        }
        public class FeatureTierConfig
        {
            public string Seed { get; set; }
            public int Bonus { get; set; }
        }
    }
}
