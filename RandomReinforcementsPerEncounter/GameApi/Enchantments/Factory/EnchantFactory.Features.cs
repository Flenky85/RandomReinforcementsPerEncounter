using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                                       
using Kingmaker.Blueprints;                                      
using RandomReinforcementsPerEncounter.Config.Localization;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Utils;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        public static void RegisterWeaponFeaturesTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,     
            string description,
            string AffixDisplay, 
            AffixKind affix
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                var desc = FactoryText.BuildStackableBonusDescription(t.BonusDescription, description);
                
                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant, AffixDisplay, desc);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var locPrefix = keys.locAffix;
                var locDesc = keys.locDesc;

                var featureRef = BlueprintTool.GetRef<BlueprintFeatureReference>(t.Feat);

                var cfg = WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)
                    .SetEnchantName(locName)
                    .SetDescription(locDesc);

                
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
