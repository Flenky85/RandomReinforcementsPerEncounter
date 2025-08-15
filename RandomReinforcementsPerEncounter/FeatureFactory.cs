using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
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
    public static class FeatureFactory
    {
        public class FeatureTierConfig
        {
            public string AssetId { get; set; }
            public int Bonus { get; set; }
        }

        public static void RegisterSpellsDCTiersFor(
            List<FeatureTierConfig> tiers,
            string nameRoot
        )
        {
            var result = new List<BlueprintFeatureReference>();

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int plus = t.Bonus <= 0 ? 1 : t.Bonus;

                string bpName = $"RRE_{nameRoot.Replace(' ', '_')}_T{i + 1}_Feature";

                FeatureConfigurator
                    .New(bpName, t.AssetId)
                    .SetDisplayName("Hidden") 
                    .SetDescription("Hidden")
                    .SetHideInUI(true)
                    .AddIncreaseSpellDC(
                        bonusDC: plus,
                        descriptor: ModifierDescriptor.UntypedStackable
                    )
                    .Configure()
                    .ToReference<BlueprintFeatureReference>();
            }
        }
    }
}
