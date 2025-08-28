using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using Kingmaker.RuleSystem;                                     
using RandomReinforcementsPerEncounter.Config.Localization;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Utils;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        public static void RegisterDamageTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,          
            string description,      
            string prefab,            
            string affix
        )
        {
            var energy = FactoryMaps.MapEnergyType(description);

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];

                int rolls = t.DiceCount > 0 ? t.DiceCount : 1;
                int sides = t.DiceSide > 0 ? t.DiceSide : 6;
                var diceT = DiceMapper.MapDiceType(sides);

                var desc = FactoryText.BuildEnergyDescription(rolls, sides, description);

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant, affix, desc);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var locDesc = keys.locDesc;
                var locPrefix = keys.locAffix;

                var cfg = WeaponEnchantmentConfigurator
                    .New(bpName, t.AssetId)
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .SetPrefix(locPrefix);

                if (!string.IsNullOrEmpty(prefab))
                    cfg = cfg.SetWeaponFxPrefab(prefab);

                cfg.AddWeaponEnergyDamageDice(
                        element: energy,
                        energyDamageDice: new DiceFormula(rolls, diceT)
                    )
                   .Configure();
            }
        }
    }
}
