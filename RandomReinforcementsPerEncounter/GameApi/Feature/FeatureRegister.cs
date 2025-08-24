using System.Collections.Generic;
using Kingmaker.Blueprints.Classes.Spells;
using RandomReinforcementsPerEncounter.Config.Ids.Generators;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static class FeatureRegister
    {
        private static bool _done;


        private static readonly int[] SpellDcBonuses = { 1, 2, 3 };
        private static readonly int[] SpellDieBonuses = { 1, 2, 3 };

        private static readonly int[] SchoolClBonuses = { 1, 2, 3 };
        private static readonly int[] SchoolDcBonuses = { 1, 2, 3 };
        

        public static void RegisterAll()
        {
            if (_done) return; // idempotente
            _done = true;

            // --- Spell DC / Die Bonus ---
            FeatureFactory.RegisterSpellsDCTiersFor(BuildTiers(Seed.spellDC, SpellDcBonuses),nameRoot: "Spell DC");
            FeatureFactory.RegisterSpellsPerDieBonusTiersFor(BuildTiers(Seed.spellDieBonus, SpellDieBonuses),nameRoot: "Spell Die Bonus");

            // --- School CL ---
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.divinationCL, SchoolClBonuses), "Divination Caster Level", SpellSchool.Divination);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.enchantmentCL, SchoolClBonuses), "Enchantment Caster Level", SpellSchool.Enchantment);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.evocationCL, SchoolClBonuses), "Evocation Caster Level", SpellSchool.Evocation);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.conjurationCL, SchoolClBonuses), "Conjuration Caster Level", SpellSchool.Conjuration);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.abjurationCL, SchoolClBonuses), "Abjuration Caster Level", SpellSchool.Abjuration);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.illusionCL, SchoolClBonuses), "Illusion Caster Level", SpellSchool.Illusion);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.transmutationCL, SchoolClBonuses), "Transmutation Caster Level", SpellSchool.Transmutation);
            FeatureFactory.RegisterSpellSchoolCLTiersFor(BuildTiers(Seed.necromancyCL, SchoolClBonuses), "Necromancy Caster Level", SpellSchool.Necromancy);

            // --- School DC ---
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.divinationDC, SchoolDcBonuses), "Divination DC", SpellSchool.Divination);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.enchantmentDC, SchoolDcBonuses), "Enchantment DC", SpellSchool.Enchantment);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.evocationDC, SchoolDcBonuses), "Evocation DC", SpellSchool.Evocation);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.conjurationDC, SchoolDcBonuses), "Conjuration DC", SpellSchool.Conjuration);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.abjurationDC, SchoolDcBonuses), "Abjuration DC", SpellSchool.Abjuration);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.illusionDC, SchoolDcBonuses), "Illusion DC", SpellSchool.Illusion);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.transmutationDC, SchoolDcBonuses), "Transmutation DC", SpellSchool.Transmutation);
            FeatureFactory.RegisterSpellSchoolDCTiersFor(BuildTiers(Seed.necromancyDC, SchoolDcBonuses), "Necromancy DC", SpellSchool.Necromancy);

        }

        /// Construye tantos tiers como elementos tenga 'bonuses'.
        private static List<FeatureTierConfig> BuildTiers(string keyRoot, int[] bonuses)
        {
            var list = new List<FeatureTierConfig>(bonuses.Length);
            for (int i = 0; i < bonuses.Length; i++)
            {
                int tier = i + 1;
                list.Add(new FeatureTierConfig
                {
                    AssetId = FeatureIds.ForTier(keyRoot, tier),
                    Bonus = bonuses[i]
                });
            }
            return list;
        }
    }
}
