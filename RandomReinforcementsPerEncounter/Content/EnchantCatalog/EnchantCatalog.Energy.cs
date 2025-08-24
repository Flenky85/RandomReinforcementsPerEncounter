using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

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
            
            (int dice, int sides)[] DamageTierOneHanded = {(1,4),(2,3),(2,4),(2,6),(2,8),(2,10)};
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

        internal static readonly EnchantDef Flaming = MakeEnergyDamage(Seed.fire, "Flaming", "fire", BlueprintGuids.Flaming);
        internal static readonly EnchantDef Frost = MakeEnergyDamage(Seed.cold, "Frost", "cold", BlueprintGuids.Frost);
        internal static readonly EnchantDef Shock = MakeEnergyDamage(Seed.electricity, "Shock", "electricity", BlueprintGuids.Shock);
        internal static readonly EnchantDef Thundering = MakeEnergyDamage(Seed.sonic, "Thundering", "sonic", BlueprintGuids.Sonic);
        internal static readonly EnchantDef Corrosive = MakeEnergyDamage(Seed.acid, "Corrosive", "acid", BlueprintGuids.Corrosive);
        internal static readonly EnchantDef Unholy = MakeEnergyDamage(Seed.unholy, "Unholy", "negative damage", BlueprintGuids.Unholy);
        internal static readonly EnchantDef Holy = MakeEnergyDamage(Seed.holy, "Holy", "holy", BlueprintGuids.Holy);

    }
}
