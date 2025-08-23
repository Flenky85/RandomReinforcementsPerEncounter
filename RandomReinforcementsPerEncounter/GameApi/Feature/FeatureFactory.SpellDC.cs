using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System;
using System.Collections.Generic;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    public static partial class FeatureFactory
    {
        private static int ClampBonus(int v) => Math.Max(1, v);

        // Spell DC (global): +X DC a todos los hechizos
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

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, nameRoot, ArtifactKind.Feature, nameRoot);

                var featRef = FeatureConfigurator
                    .New(keys.bpName, t.AssetId)
                    .SetDisplayName(keys.locName)   // mantenemos locs aunque esté oculto
                    .SetDescription(keys.descKey)
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
