using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                                       // LocalizationTool
using Kingmaker.RuleSystem;                                      // DiceType, DiceFormula
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Enchantments.Factory.Util; // FactoryMaps, FactoryText
using RandomReinforcementsPerEncounter.GameApi.Localization;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    public static partial class EnchantFactory
    {
        public static void RegisterDamageTiersFor(
            List<EnchantTierConfig> tiers,
            string name,
            string nameRoot,          // ej: "Flaming"
            string description,       // ej: "fire"
            string prefab             // ej: guid FX (opcional)
        )
        {
            var energy = FactoryMaps.MapEnergyType(description);

            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];

                // saneo rápido
                int rolls = t.DiceCount > 0 ? t.DiceCount : 1;
                int sides = t.DiceSide > 0 ? t.DiceSide : 6;
                var diceT = FactoryMaps.MapDiceType(sides);

                var keys = KeyBuilder.BuildTierKeys(nameRoot, i + 1, name, ArtifactKind.Enchant);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var descKey = keys.descKey;
                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    FactoryText.BuildEnergyDescription(rolls, sides, description)
                );
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
