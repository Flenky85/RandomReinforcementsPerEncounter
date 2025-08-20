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
            List<TierConfig> tiers,
            string name,
            string nameRoot,   // p.ej. "Strength"
            string description,
            StatType stat
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int plus = t.Bonus <= 0 ? 1 : t.Bonus;

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var descKey = keys.descKey;

                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    FactoryText.BuildStackableBonusDescription(plus, description)
                );
                var locPrefix = keys.locPrefix;

                WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .SetPrefix(locPrefix)
                    .AddStatBonusEquipment(
                        descriptor: ModifierDescriptor.UntypedStackable,
                        stat: stat,
                        value: plus
                    )
                    .Configure();
            }
        }
    }
}
