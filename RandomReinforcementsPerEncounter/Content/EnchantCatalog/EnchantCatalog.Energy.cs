using RandomReinforcementsPerEncounter.Domain.Models;
using System;
using static RandomReinforcementsPerEncounter.Config.Ids.BlueprintGuids;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        // Progresión de daño por tier (tal cual tu patrón)
        private static EnchantDef MakeEnergyDamage(
            string seed,
            string affixName,
            string descriptor,
            string prefab)
        {
            
            (int dice, int sides)[] DamageTierOneHanded = {(1,3),(2,3),(2,4),(2,6),(2,8),(2,10)};
            (int dice, int sides)[] DamageTierTwoHanded = {(1,6),(3,3),(3,4),(3,6),(3,8),(3,10)};
            int chance = 10;

            return new EnchantDef
            {
                Seed = seed,
                Name = affixName,
                AffixDisplay = affixName,
                Desc = descriptor,                 // "fire", "cold", etc. (lo enlaza AutoLinker en la factory)
                Affix = AffixKind.Prefix,
                Type = EnchantType.EnergyDamage,
                Chance = chance,

                ApplyToBothHeadsOnDouble = true,

                DamageMapOneHanded = DamageTierOneHanded,
                DamageMapTwoHanded = DamageTierTwoHanded,
                DamagePrefab = prefab,
            };
        }

        // Instancias (si prefieres exponerlas individualmente)
        internal static readonly EnchantDef Flaming = MakeEnergyDamage("fire", "Flaming", "fire", EnchantsPrefabs.Flaming);
        internal static readonly EnchantDef Frost = MakeEnergyDamage("cold", "Frost", "cold", EnchantsPrefabs.Frost);
        internal static readonly EnchantDef Shock = MakeEnergyDamage("electricity", "Shock", "electricity", EnchantsPrefabs.Shock);
        internal static readonly EnchantDef Thundering = MakeEnergyDamage("sonic", "Thundering", "sonic", EnchantsPrefabs.Sonic);
        internal static readonly EnchantDef Corrosive = MakeEnergyDamage("acid", "Corrosive", "acid", EnchantsPrefabs.Corrosive);
        internal static readonly EnchantDef Unholy = MakeEnergyDamage("unholy", "Unholy", "negative damage", EnchantsPrefabs.Unholy);
        internal static readonly EnchantDef Holy = MakeEnergyDamage("holy", "Holy", "holy", EnchantsPrefabs.Holy);
    }
}
