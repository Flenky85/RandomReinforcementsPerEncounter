using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
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
        public static List<BlueprintFeatureReference> RegisterSpellsPerDieBonusTiersFor(
            List<FeatureTierConfig> tiers,
            string nameRoot,
            bool spellsOnly = true,                      // solo spells (no SLAs) por defecto
            bool useContextBonus = true                  // usa ContextValue (recomendado)
        )
        {
            var result = new List<BlueprintFeatureReference>();

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int perDie = t.Bonus <= 0 ? 1 : t.Bonus;

                var keys = KeyBuilderUtils.BuildKeys(nameRoot, i + 1, nameRoot);
                /*
                string bpName = $"RRE_{nameRoot.Replace(' ', '_')}_T{i + 1}_Feature";
                var nameKey = $"RRE.{nameRoot}.T{i + 1}.Name";
                var descKey = $"RRE.{nameRoot}.T{i + 1}.Desc";

                var locName = LocalizationTool.CreateString(nameKey, $"{nameRoot} (T{i + 1})");*/

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
                        value: ContextValues.Constant(perDie)        // +X por dado
                    )
                    .Configure()
                    .ToReference<BlueprintFeatureReference>();

                result.Add(featRef);
            }

            return result;
        }
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
                int bonus = t.Bonus <= 0 ? 1 : t.Bonus;

                string bpName = $"RRE_{nameRoot.Replace(' ', '_')}_T{i + 1}_Feature";

                var featRef = FeatureConfigurator
                    .New(bpName, t.AssetId)
                    .SetDisplayName("Hidden")
                    .SetDescription("Hidden")
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
                int bonus = t.Bonus <= 0 ? 1 : t.Bonus;

                string bpName = $"RRE_{nameRoot.Replace(' ', '_')}_T{i + 1}_Feature";

                var featRef = FeatureConfigurator
                    .New(bpName, t.AssetId)
                    .SetDisplayName("Hidden")
                    .SetDescription("Hidden")
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
