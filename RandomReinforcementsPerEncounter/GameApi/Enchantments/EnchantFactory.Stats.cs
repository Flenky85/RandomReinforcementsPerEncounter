using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                                       // LocalizationTool
using Kingmaker.EntitySystem.Stats;                              // StatType
using Kingmaker.Enums;                                           // ModifierDescriptor
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Util; // FactoryText
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    public static partial class EnchantFactory
    {
        public static void RegisterWeaponStatsTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,
            string description,
            StatType stat,
            string suffix,                 // texto del afijo (puede ser null si usas 'name')
            AffixKind affix = AffixKind.Suffix
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];

                var keys = KeyBuilder.BuildTierKeys(
                    nameRoot: nameRoot,
                    tierIndex: i + 1,
                    name: name,
                    kind: ArtifactKind.Enchant,
                    suffixName: suffix // si null, BuildTierKeys usará 'name'
                );

                var locDesc = LocalizationTool.CreateString(
                    keys.descKey,
                    FactoryText.BuildStackableBonusDescription(t.Bonus, description)
                );

                var cfg = WeaponEnchantmentConfigurator
                    .New(keys.bpName, t.AssetId)
                    .SetEnchantName(keys.locName)
                    .SetDescription(locDesc);

                // ← Aquí aplicamos según el tipo de afijo
                if (affix == AffixKind.Prefix)
                    cfg.SetPrefix(keys.locAffix);
                else
                    cfg.SetSuffix(keys.locAffix);

                cfg.AddStatBonusEquipment(
                        descriptor: ModifierDescriptor.UntypedStackable,
                        stat: stat,
                        value: t.Bonus
                    )
                    .Configure();
            }
        }

    }
}
