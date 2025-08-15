using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter
{
    internal class EnchantRegister
    {

        // Prefabs
        private const string PREFAB_CRIMSON_MOON = "907d3c215c9522d4e8a3b763f1b32935"; // CrimsonMoonEnchantment (blood)
        private const string PREFAB_UNHOLY = "e098bc50ec458af4cb7c2d828db0ce18"; // Unholy
        private const string PREFAB_CORROSIVE = "bfafef74d59950242915a8e294e6fac0"; // Corrosive
        private const string PREFAB_FLAMING = "91e5a56dd421a2941984a36a2af164b6"; // Flaming
        private const string PREFAB_FROST = "e9930f40a35b67c418e78a98c601c93b"; // Frost
        private const string PREFAB_SHOCK = "1d1465ffa2699644ba8dfac48cb33195"; // Shock
        private const string PREFAB_VICIOUS = "d01d77862c68852449895718902c8599"; // Vicious (Thorns)
        private const string PREFAB_HOLY = "d739a9e236ba6164ab854b356bfb6ed5"; // Holy
        private const string PREFAB_SONIC = "d31b9df8d99674742a161eb3faa07f3f"; // Sonic
        private const string PREFAB_SPEED = "eee18332ea407bb4ea9bf2aa5f9ddf90"; // Speed (wind green)
        private const string PREFAB_BRILLIANT_ENERGY = "fdc7f8f37d3f8da42be2a1d35a617001"; // BrilliantEnergy (magic blue)
        private const string PREFAB_AXIOMATIC = "9c950f7e0624df24ca74d9b01a3a2cfa"; // Axiomatic (yellow circles)
        private const string PREFAB_AGILE = "a4eba3360cc5b5d4ba2fe1036ce0cc8c"; // Agile (wind blue)
        private const string PREFAB_GHOST = "d7b9bfb16264e4d4aad2abef2c80f835"; // Ghost
        private const string PREFAB_ANARCHIC = "57315bc1e1f62a741be0efde688087e9"; // Anarchic (oxido)


        public static void RegisterAll()
        {
            //////////////////////////////////////////////////////////////////////////////////
            //                            Debuff onlyHit                                    //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("shaken.t6").ToString(), DC = 26 }
                },
                nameRoot: "Shaken",
                description: "shaken",
                condition: "Shaken",
                buff: "25ec6cb6ab1845c48a95f9c20b034220",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyHit
            ); 
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("blindness.t6").ToString(), DC = 26 }
                },
                nameRoot: "Blindness",
                description:"blinded",
                condition: "Blind",
                buff: "0ec36e7596a4928489d2049e1e1c76a7",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("dazzled.t6").ToString(), DC = 26 }
                },
                nameRoot: "Dazzled",
                description: "dazzled",
                condition: "Dazzled",
                buff: "df6d1020da07524423afbae248845ecc",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sickened.t6").ToString(), DC = 26 }
                },
                nameRoot: "Sickened",
                description: "sickened",
                condition: "Sickened",
                buff: "4e42460798665fd4cb9143ffa7ada323",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("staggered.t6").ToString(), DC = 26 }
                },
                nameRoot: "Staggered",
                description: "staggered",
                condition: "Staggered",
                buff: "df3950af5a783bd4d91ab73eb8fa0fd3",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fatigue.t6").ToString(), DC = 26 }
                },
                nameRoot: "Fatigue",
                description: "fatigued",
                condition: "Fatigued",
                buff: "e6f2fc5d73d88064583cb828801172f4",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("confusion.t6").ToString(), DC = 26 }
                },
                nameRoot: "Confusion",
                description: "confused",
                condition: "Confusion",
                buff: "886c7407dc623dc499b9f1465ff382df",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("entangled.t6").ToString(), DC = 26 }
                },
                nameRoot: "Entangled",
                description: "entangled",
                condition: "Entangled",
                buff: "f7f6260726117cf4b90a6086b05d2e38",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Reflex,
                activation: ActivationType.onlyHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t1").ToString(), DC = 11 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t2").ToString(), DC = 14 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t3").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t4").ToString(), DC = 20 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t5").ToString(), DC = 23 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("slowed.t6").ToString(), DC = 26 }
                },
                nameRoot: "Slowed",
                description: "slowed",
                condition: "Slowed",
                buff: "488e53ede2802ff4da9372c6a494fb66",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Reflex,
                activation: ActivationType.onlyHit
            );
            //////////////////////////////////////////////////////////////////////////////////
            //                         Debuff onlyOnFirstHit                                //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("frightened.t6").ToString(), DC = 33 }
                },
                nameRoot: "Frightened",
                description: "frightened",
                condition: "Frightened",
                buff: "f08a7239aa961f34c8301518e71d4cdf",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("stunned.t6").ToString(), DC = 33 }
                },
                nameRoot: "Stunned",
                description: "stunned",
                condition: "Stunned",
                buff: "09d39b38bb7c6014394b6daced9bacd3",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("daze.t6").ToString(), DC = 33 }
                },
                nameRoot: "Daze",
                description: "dazed",
                condition: "Daze",
                buff: "d2e35b870e4ac574d9873b36402487e5",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sleep.t6").ToString(), DC = 33 }
                },
                nameRoot: "Sleep",
                description: "asleep",
                condition: "Sleep",
                buff: "c9937d7846aa9ae46991e9f298be644a",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("paralyzed.t6").ToString(), DC = 33 }
                },
                nameRoot: "Paralyzed",
                description: "paralyzed",
                condition: "Paralyzed",
                buff: "4d5a2e4c34d24acca575c10003cf8fbc",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("exhausted.t6").ToString(), DC = 33 }
                },
                nameRoot: "Exhausted",
                description: "exhausted",
                condition: "Exhausted",
                buff: "46d1b9cc3d0fd36469a471b047d773a2",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("nauseated.t6").ToString(), DC = 33 }
                },
                nameRoot: "Nauseated",
                description: "nauseated",
                condition: "Nauseated",
                buff: "956331dba5125ef48afe41875a00ca0e",
                durationDiceCount: 1,
                durationDiceSides: 3,
                savingThrowType: SavingThrowType.Fortitude,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("prone.t6").ToString(), DC = 33 }
                },
                nameRoot: "Prone",
                description: "prone",
                condition: "Prone",
                buff: "24cf3deb078d3df4d92ba24b176bda97",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Reflex,
                activation: ActivationType.onlyOnFirstHit
            );
            EnchantFactory.RegisterDebuffTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t1").ToString(), DC = 13 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t2").ToString(), DC = 17 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t3").ToString(), DC = 21 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t4").ToString(), DC = 25 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t5").ToString(), DC = 29 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("domination.t6").ToString(), DC = 33 }
                },
                nameRoot: "Domination",
                description: "dominated",
                condition: "Domination",
                buff: "c0f4e1c24c9cd334ca988ed1bd9d201f",
                durationDiceCount: 1,
                durationDiceSides: 1,
                savingThrowType: SavingThrowType.Will,
                activation: ActivationType.onlyOnFirstHit
            );

            //////////////////////////////////////////////////////////////////////////////////
            //                            Extra Damage                                      //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("fire.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Flaming",
                description: "fire",
                prefab: PREFAB_FLAMING
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("cold.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Frost",
                description: "cold",
                prefab: PREFAB_FROST
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("electricity.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Shock",
                description: "electricity",
                prefab: PREFAB_SHOCK
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("sonic.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Thundering",
                description: "sonic",
                prefab: PREFAB_SONIC
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("acid.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Corrosive",
                description: "acid",
                prefab: PREFAB_CORROSIVE
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("unholy.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Unholy",
                description: "negative damage",
                prefab: PREFAB_UNHOLY
            );
            EnchantFactory.RegisterDamageTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t1").ToString(), DiceCount = 1, DiceSide = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t2").ToString(), DiceCount = 1, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t3").ToString(), DiceCount = 1, DiceSide = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t4").ToString(), DiceCount = 2, DiceSide = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t5").ToString(), DiceCount = 2, DiceSide = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("holy.t6").ToString(), DiceCount = 2, DiceSide = 10 }
                },
                nameRoot: "Holy",
                description: "holy",
                prefab: PREFAB_HOLY
            );


            //////////////////////////////////////////////////////////////////////////////////
            //                                Stats Bonus                                   //
            //////////////////////////////////////////////////////////////////////////////////
            
            //Base Stats
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statSTR.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Strength",
                description: "strength",
                encyclopedia: "Strength",
                stat: StatType.Strength
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statDEX.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Dexterity",
                description: "dexterity",
                encyclopedia: "Dexterity",
                stat: StatType.Dexterity
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCON.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Constitution",
                description: "constitution",
                encyclopedia: "Constitution",
                stat: StatType.Constitution
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statINT.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Intelligence",
                description: "intelligence",
                encyclopedia: "Intelligence",
                stat: StatType.Intelligence
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statWIS.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Wisdom",
                description: "wisdom",
                encyclopedia: "Wisdom",
                stat: StatType.Wisdom
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("statCHA.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Charisma",
                description: "charisma",
                encyclopedia: "Charisma",
                stat: StatType.Charisma
            );

            //Saves
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveFOR.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Fortitude",
                description: "saving throw fortitude",
                encyclopedia: "Saving_Throw",
                stat: StatType.SaveFortitude
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveWIL.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Will",
                description: "saving throw will",
                encyclopedia: "Saving_Throw",
                stat: StatType.SaveWill
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("saveREF.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "Reflex",
                description: "saving throw reflex",
                encyclopedia: "Saving_Throw",
                stat: StatType.SaveReflex
            );

            //Skills
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillMOB.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Mobility",
                description: "mobility",
                encyclopedia: "Mobility",
                stat: StatType.SkillMobility
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillATH.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Athletics",
                description: "athletics",
                encyclopedia: "Athletics",
                stat: StatType.SkillAthletics
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillARC.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Knowledge Arcana",
                description: "knowledge arcana",
                encyclopedia: "Knowledge_Arcana",
                stat: StatType.SkillKnowledgeArcana
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillWOR.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Knowledge World",
                description: "knowledge world",
                encyclopedia: "Knowledge_World",
                stat: StatType.SkillKnowledgeWorld
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillNAT.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Lore Nature",
                description: "lore nature",
                encyclopedia: "Lore_Nature",
                stat: StatType.SkillLoreNature
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillREL.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Lore Religion",
                description: "lore religion",
                encyclopedia: "Lore_Religion",
                stat: StatType.SkillLoreReligion
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERC.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Perception",
                description: "perception",
                encyclopedia: "Perception",
                stat: StatType.SkillPerception
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillPERS.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Persuasion",
                description: "persuasion",
                encyclopedia: "Persuasion",
                stat: StatType.SkillPersuasion
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillSTE.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Stealth",
                description: "stealth",
                encyclopedia: "Stealth",
                stat: StatType.SkillStealth
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillTHI.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Trickery",
                description: "Trickery",
                encyclopedia: "Trickery",
                stat: StatType.SkillThievery
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("skillUMD.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Use Magic Device",
                description: "use magic device",
                encyclopedia: "Use_Magic_Device",
                stat: StatType.SkillUseMagicDevice
            );

            //others
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t1").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t2").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t3").ToString(), Bonus = 6 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t4").ToString(), Bonus = 8 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t5").ToString(), Bonus = 10 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("initiative.t6").ToString(), Bonus = 12 }
                },
                nameRoot: "Initiative",
                description: "initiative",
                encyclopedia: "Initiative",
                stat: StatType.Initiative
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMB.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "CMB",
                description: "combat maneuver bonus",
                encyclopedia: "CMB",
                stat: StatType.AdditionalCMB
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t2").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t3").ToString(), Bonus = 3 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t4").ToString(), Bonus = 4 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t5").ToString(), Bonus = 5 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("CMD.t6").ToString(), Bonus = 6 }
                },
                nameRoot: "CMD",
                description: "combat maneuver defense",
                encyclopedia: "CMD",
                stat: StatType.AdditionalCMD
            );
            EnchantFactory.RegisterWeaponStatsTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t1").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t2").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t3").ToString(), Bonus = 1 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t4").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t5").ToString(), Bonus = 2 },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("casterLevel.t6").ToString(), Bonus = 2 }
                },
                nameRoot: "Caster Level",
                description: "caster level",
                encyclopedia: "Caster_Level",
                stat: StatType.BonusCasterLevel
            );

            //////////////////////////////////////////////////////////////////////////////////
            //                                  Caster                                      //
            //////////////////////////////////////////////////////////////////////////////////
            EnchantFactory.RegisterWeaponFeaturesTiersFor(
                new List<TierConfig>
                {
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t1").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t1").ToString() },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t2").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t1").ToString() },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t3").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t2").ToString() },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t4").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t2").ToString() },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t5").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t3").ToString() },
                    new TierConfig { AssetId = GuidUtil.EnchantGuid("spellDC.t6").ToString(), Feat = GuidUtil.FeatureGuid("spellDC.t3").ToString() }
                },
                nameRoot: "Spell DC",
                description: "spell DC",
                encyclopedia: "DC"
            );

        }
    }
}
