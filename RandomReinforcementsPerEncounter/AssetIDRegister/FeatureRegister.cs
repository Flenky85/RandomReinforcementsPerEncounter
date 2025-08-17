using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RandomReinforcementsPerEncounter.FeatureFactory;

namespace RandomReinforcementsPerEncounter
{
    internal class FeatureRegister
    {
        public static void RegisterAll()
        {
            //SpellsDC
            FeatureFactory.RegisterSpellsDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Spell DC"
            );

            //SpellsPerDieBonus
            FeatureFactory.RegisterSpellsPerDieBonusTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDieBonus.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDieBonus.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("spellDieBonus.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Spell Die Bonus"
            );

            //SpellSchoolCL
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("divinationCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("divinationCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("divinationCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Divination Caster Level",
                SpellSchool.Divination
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("enchantmentCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("enchantmentCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("enchantmentCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Enchantment Caster Level",
                SpellSchool.Enchantment
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("evocationCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("evocationCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("evocationCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Evocation Caster Level",
                SpellSchool.Evocation
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("conjurationCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("conjurationCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("conjurationCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Conjuration Caster Level",
                SpellSchool.Conjuration
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("abjurationCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("abjurationCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("abjurationCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Abjuration Caster Level",
                SpellSchool.Abjuration
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("illusionCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("illusionCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("illusionCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Illusion Caster Level",
                SpellSchool.Illusion
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("transmutacionCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("transmutacionCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("transmutacionCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Transmutation Caster Level",
                SpellSchool.Transmutation
            );
            FeatureFactory.RegisterSpellSchoolCLTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("necromancyCL.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("necromancyCL.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("necromancyCL.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Necromancy Caster Level",
                SpellSchool.Necromancy
            );

            //spellSchoolDC
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("divinationDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("divinationDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("divinationDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Divination DC",
                SpellSchool.Divination
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("enchantmentDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("enchantmentDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("enchantmentDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Enchantment DC",
                SpellSchool.Enchantment
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("evocationDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("evocationDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("evocationDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Evocation DC",
                SpellSchool.Evocation
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("conjurationDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("conjurationDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("conjurationDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Conjuration DC",
                SpellSchool.Conjuration
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("abjurationDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("abjurationDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("abjurationDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Abjuration DC",
                SpellSchool.Abjuration
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("illusionDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("illusionDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("illusionDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Illusion DC",
                SpellSchool.Illusion
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("transmutacionDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("transmutacionDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("transmutacionDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Transmutation DC",
                SpellSchool.Transmutation
            );
            FeatureFactory.RegisterSpellSchoolDCTiersFor(
                new List<FeatureTierConfig>
                {
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("necromancyDC.t1").ToString(), Bonus = 1 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("necromancyDC.t2").ToString(), Bonus = 2 },
                    new FeatureTierConfig { AssetId = GuidUtil.FeatureGuid("necromancyDC.t3").ToString(), Bonus = 3 }
                },
                nameRoot: "Necromancy DC",
                SpellSchool.Necromancy
            );

        }
    }
}
