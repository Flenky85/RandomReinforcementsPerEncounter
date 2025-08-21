using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                                       // BlueprintTool, LocalizationTool
using Kingmaker.Blueprints;                                      // BlueprintFeatureReference
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Util; // FactoryText
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    public static partial class EnchantFactory
    {
        public static void RegisterWeaponFeaturesTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,      // p.ej. "Spell DC"
            string description
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var descKey = keys.descKey;
                var locPrefix = keys.locPrefix;

                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    FactoryText.BuildStackableBonusDescription(t.BonusDescription, description)
                );

                var featureRef = BlueprintTool.GetRef<BlueprintFeatureReference>(t.Feat);

                WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .SetPrefix(locPrefix)
                    .AddUnitFeatureEquipment(featureRef)
                    .Configure();
            }
        }
    }
}
