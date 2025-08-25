using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using RandomReinforcementsPerEncounter.Config.Localization;
using RandomReinforcementsPerEncounter.Domain.Models;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    public static partial class FeatureFactory
    {
        public static List<BlueprintFeatureReference> RegisterSpellSchoolCLTiersFor(
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

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, nameRoot, ArtifactKind.Feature, nameRoot, nameRoot);
                var locDesc = keys.locDesc;

                var featRef = FeatureConfigurator
                    .New(keys.bpName, t.AssetId)
                    .SetDisplayName(keys.locName)
                    .SetDescription(locDesc)
                    .SetHideInUI(true)
                    .AddIncreaseSpellSchoolCasterLevel(
                        bonusLevel: bonus,
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
