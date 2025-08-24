using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                                       // BlueprintTool, LocalizationTool
using Kingmaker.Blueprints;                                      // BlueprintFeatureReference
using RandomReinforcementsPerEncounter.Config.Ids;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Utils;
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        public static void RegisterWeaponFeaturesTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,      // p.ej. "Spell DC"
            string description,
            string AffixDisplay,  // texto del afijo (puede ser null si usas 'name')
            AffixKind affix
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];

                // estilo unificado
                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant, AffixDisplay);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var descKey = keys.descKey;
                var locPrefix = keys.locAffix;

                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    FactoryText.BuildStackableBonusDescription(t.BonusDescription, description)
                );

                var featureRef = BlueprintTool.GetRef<BlueprintFeatureReference>(t.Feat);

                var cfg = WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)
                    .SetEnchantName(locName)
                    .SetDescription(locDesc);

                // prefix/suffix según 'affix' (normalizado)
                if (affix == AffixKind.Prefix)
                    cfg.SetPrefix(locPrefix);
                else
                    cfg.SetSuffix(locPrefix);

                cfg.AddUnitFeatureEquipment(featureRef)
                   .Configure();
            }
        }
    }
}
