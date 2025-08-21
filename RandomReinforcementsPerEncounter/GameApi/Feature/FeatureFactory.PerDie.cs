using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System.Collections.Generic;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    public static partial class FeatureFactory
    {
        // +X por dado a hechizos con descriptor elemental/cura
        public static List<BlueprintFeatureReference> RegisterSpellsPerDieBonusTiersFor(
            List<FeatureTierConfig> tiers,
            string nameRoot,
            bool spellsOnly = true,
            bool useContextBonus = true
        )
        {
            var result = new List<BlueprintFeatureReference>();

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int perDie = ClampBonus(t.Bonus);

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, nameRoot, ArtifactKind.Feature);

                var anyEnergy =
                    SpellDescriptor.Fire |
                    SpellDescriptor.Cold |
                    SpellDescriptor.Acid |
                    SpellDescriptor.Electricity |
                    SpellDescriptor.Sonic |
                    SpellDescriptor.Force |
                    SpellDescriptor.Cure;

                var featRef = FeatureConfigurator
                    .New(keys.bpName, t.AssetId)
                    .SetDisplayName(keys.locName)
                    .SetDescription(keys.descKey)
                    .SetHideInUI(true)
                    .AddDraconicBloodlineArcana(
                        spellDescriptor: anyEnergy,
                        spellsOnly: spellsOnly,
                        useContextBonus: useContextBonus,
                        value: ContextValues.Constant(perDie) // +X por dado
                    )
                    .Configure()
                    .ToReference<BlueprintFeatureReference>();

                result.Add(featRef);
            }

            return result;
        }
    }
}
