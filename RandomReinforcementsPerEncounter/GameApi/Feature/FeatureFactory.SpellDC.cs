using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using RandomReinforcementsPerEncounter.Config.Localization;
using RandomReinforcementsPerEncounter.Domain.Models;
using System;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    public static partial class FeatureFactory
    {
        private static int ClampBonus(int v) => Math.Max(1, v);

        public static List<BlueprintFeatureReference> RegisterSpellsDCTiersFor(
            List<FeatureTierConfig> tiers,
            string nameRoot
        )
        {
            var result = new List<BlueprintFeatureReference>();

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int plus = ClampBonus(t.Bonus);

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, nameRoot, ArtifactKind.Feature, nameRoot, nameRoot);
                var locDesc = keys.locDesc;

                var featRef = FeatureConfigurator
                    .New(keys.bpName, t.AssetId)
                    .SetDisplayName(keys.locName)   
                    .SetDescription(locDesc)
                    .SetHideInUI(true)
                    .AddIncreaseSpellDC(
                        bonusDC: plus,
                        descriptor: ModifierDescriptor.UntypedStackable
                    )
                    .Configure()
                    .ToReference<BlueprintFeatureReference>();

                result.Add(featRef);
            }

            return result;
        }
    }
}
