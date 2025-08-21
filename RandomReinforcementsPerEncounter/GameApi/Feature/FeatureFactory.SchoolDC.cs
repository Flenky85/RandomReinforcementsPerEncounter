using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System.Collections.Generic;
using RandomReinforcementsPerEncounter.Domain.Models;


namespace RandomReinforcementsPerEncounter
{
    public static partial class FeatureFactory
    {
        // +DC a una escuela concreta
        public static List<BlueprintFeatureReference> RegisterSpellSchoolDCTiersFor(
            List<FeatureTierConfig> tiers,
            string nameRoot,
            SpellSchool school
        )
        {
            var result = new List<BlueprintFeatureReference>();

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int bonus = ClampBonus(t.Bonus);

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, nameRoot, ArtifactKind.Feature);

                var featRef = FeatureConfigurator
                    .New(keys.bpName, t.AssetId)
                    .SetDisplayName(keys.locName)
                    .SetDescription(keys.descKey)
                    .SetHideInUI(true)
                    .AddIncreaseSpellSchoolDC(
                        bonusDC: bonus,
                        descriptor: ModifierDescriptor.UntypedStackable,
                        school: school
                    )
                    .Configure()
                    .ToReference<BlueprintFeatureReference>();

                result.Add(featRef);
            }

            return result;
        }
    }
}
