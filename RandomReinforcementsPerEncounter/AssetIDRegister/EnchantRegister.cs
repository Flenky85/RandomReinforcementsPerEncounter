using Kingmaker.EntitySystem.Stats;
using RandomReinforcementsPerEncounter.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using static RandomReinforcementsPerEncounter.Config.Ids.BlueprintGuids;
using static RandomReinforcementsPerEncounter.EnchantFactory;

namespace RandomReinforcementsPerEncounter
{
    // -----------------------------
    // Modelos dados por ti
    // -----------------------------
    public enum EnchantType
    {
        OnHit,
        OnlyOnFirstHit,
        EnergyDamage,
        StatsBonus,
        SavesBonus,
        SkillsBonus,
        Maneuver,
        CasterLevel,
        Caster,
        SchoolCL,
        SchoolDC
    }

    public class EnchantData
    {
        public string[] AssetIDT1;
        public string[] AssetIDT2;
        public string[] AssetIDT3;
        public string[] AssetIDT4;
        public string[] AssetIDT5;
        public string[] AssetIDT6;
        public int Value;
        public EnchantType Type;
        public Handedness Hand;
    }

    public static class EnchantList
    {
        public static readonly List<EnchantData> Item = new List<EnchantData>();
    }

    // -----------------------------
    // Catálogo único de raíces (roots) + registro y loot
    // -----------------------------
    internal static class EnchantCatalog
    {
        // helper local para la seed por variante (.one/.two/.dbl)
        private static string RootWithHand(string root, Handedness hand)
        {
            return hand switch
            {
                Handedness.OneHanded => root + ".one",
                Handedness.TwoHanded => root + ".two",
                Handedness.Double => root + ".dbl",
                _ => root + ".one",
            };
        }

        // Nuevo Id con mano: respeta tu patrón original pero añade el sufijo antes del .t{tier}
        private static string Id(string root, int tier, Handedness hand)
            => GuidUtil.EnchantGuid($"{RootWithHand(root, hand)}.t{tier}").ToString();


        // Debuff DCs (solo desarrollador; separadas por 1H / 2H)
        private static readonly int[] DebuffDCOnlyHit_1H = { 11, 15, 19, 24, 28, 32 };
        private static readonly int[] DebuffDCOnlyHit_2H = { 13, 17, 21, 26, 30, 34 };
        private static readonly int[] DebuffDCOnlyOnFirstHit_1H = { 13, 17, 21, 26, 30, 34 };
        private static readonly int[] DebuffDCOnlyOnFirstHit_2H = { 17, 19, 23, 28, 32, 36 };

        private static int[] GetDebuffDC(ActivationType activation, Handedness hand)
        {
            if (activation == ActivationType.onlyHit)
                return hand == Handedness.OneHanded ? DebuffDCOnlyHit_1H : DebuffDCOnlyHit_2H;

            return hand == Handedness.OneHanded ? DebuffDCOnlyOnFirstHit_1H : DebuffDCOnlyOnFirstHit_2H;
        }

        private static readonly (
            string root,
            string display,
            string desc,
            string buff,
            SavingThrowType save,
            int dCount,
            int dSides
        )[] DebuffOnlyHitDefs = new[]
        {
            ("shaken",     "Fearsome",   "shaken",    "25ec6cb6ab1845c48a95f9c20b034220", SavingThrowType.Will,      1, 3),
            ("blindness",  "Blinding",   "blinded",   "0ec36e7596a4928489d2049e1e1c76a7", SavingThrowType.Fortitude, 1, 3),
            ("dazzled",    "Dazzling",   "dazzled",   "df6d1020da07524423afbae248845ecc", SavingThrowType.Fortitude, 1, 3),
            ("sickened",   "Sickening",  "sickened",  "4e42460798665fd4cb9143ffa7ada323", SavingThrowType.Fortitude, 1, 1),
            ("staggered",  "Staggering", "staggered", "df3950af5a783bd4d91ab73eb8fa0fd3", SavingThrowType.Fortitude, 1, 3),
            ("fatigue",    "Fatiguing",  "fatigued",  "e6f2fc5d73d88064583cb828801172f4", SavingThrowType.Fortitude, 1, 3),
            ("confusion",  "Confusing",  "confused",  "886c7407dc623dc499b9f1465ff382df", SavingThrowType.Will,      1, 1),
            ("entangled",  "Entangling", "entangled", "f7f6260726117cf4b90a6086b05d2e38", SavingThrowType.Reflex,    1, 1),
            ("slowed",     "Slowing",    "slowed",    "488e53ede2802ff4da9372c6a494fb66", SavingThrowType.Reflex,    1, 1),
        };

        // Debuffs (onlyOnFirstHit)
        private static readonly (
            string root,
            string display,
            string desc,
            string buff,
            SavingThrowType save,
            int dCount,
            int dSides
        )[] DebuffFirstHitDefs = new[]
        {
            ("frightened", "Frightening", "frightened", "f08a7239aa961f34c8301518e71d4cdf", SavingThrowType.Will,      1, 3),
            ("stunned",    "Stunning",    "stunned",    "09d39b38bb7c6014394b6daced9bacd3", SavingThrowType.Fortitude, 1, 1),
            ("daze",       "Dazing",      "dazed",      "d2e35b870e4ac574d9873b36402487e5", SavingThrowType.Will,      1, 3),
            ("sleep",      "Slumbering",  "asleep",     "c9937d7846aa9ae46991e9f298be644a", SavingThrowType.Will,      1, 3),
            ("paralyzed",  "Paralyzing",  "paralyzed",  "4d5a2e4c34d24acca575c10003cf8fbc", SavingThrowType.Will,      1, 1),
            ("exhausted",  "Exhausting",  "exhausted",  "46d1b9cc3d0fd36469a471b047d773a2", SavingThrowType.Fortitude, 1, 3),
            ("nauseated",  "Nauseating",  "nauseated",  "956331dba5125ef48afe41875a00ca0e", SavingThrowType.Fortitude, 1, 3),
            ("prone",      "Toppling",    "prone",      "24cf3deb078d3df4d92ba24b176bda97", SavingThrowType.Reflex,    1, 1),
            ("domination", "Dominating",  "dominated",  "c0f4e1c24c9cd334ca988ed1bd9d201f", SavingThrowType.Will,      1, 1),
        };


        // --- Daño elemental/afín (EnergyDamage) ---
        private static readonly (string root, string name, string desc, string prefab)[] DamageDefs = new[]
        {
            ("fire","Flaming","fire",EnchantsPrefabs.Flaming),
            ("cold","Frost","cold",EnchantsPrefabs.Frost),
            ("electricity","Shock","electricity",EnchantsPrefabs.Shock),
            ("sonic","Thundering","sonic",EnchantsPrefabs.Sonic),
            ("acid","Corrosive","acid",EnchantsPrefabs.Corrosive),
            ("unholy","Unholy","negative damage",EnchantsPrefabs.Unholy),
            ("holy","Holy","holy",EnchantsPrefabs.Holy),
        };

        // Progresión de daño por tier (tal cual tu patrón)
        private static readonly (int dice, int sides)[] DamageTierOneHanded =
        {
            (2,3),(2,4),(2,6),(2,8),(2,10),(2,12)
        };
        // Progresión de daño por tier (tal cual tu patrón)
        private static readonly (int dice, int sides)[] DamageTierTwoHanded =
        {
            (3,3),(3,4),(3,6),(3,8),(3,10),(3,12)
        };

        private static readonly (string root, string name, string suffix, string desc, StatType stat)[] StatRoots = new[]
        {
            ("statSTR","Mighty",     "of Strnght",       "strength",     StatType.Strength),
            ("statDEX","Graceful",   "of Agility",       "dexterity",    StatType.Dexterity),
            ("statCON","Resilient",  "of Constitution",  "constitution", StatType.Constitution),
            ("statINT","Cunning",    "of Intelligence",  "intelligence", StatType.Intelligence),
            ("statWIS","Sage",       "of Wisdom",        "wisdom",       StatType.Wisdom),
            ("statCHA","Glamorous",  "of Charisma",      "charisma",     StatType.Charisma),
        };

        private static readonly int[] statBonusOne = { 1, 2, 2, 3, 3, 4 };
        private static readonly int[] statBonusDouble = { 1, 2, 3, 4, 5, 6 };


        // --- Saves (SavesBonus) ---
        private static readonly (string root, string name, string suffix, string desc, StatType stat)[] SaveRoots = new[]
        {
            ("saveFOR","Enduring","of Endurance", "fortitude saving throw",StatType.SaveFortitude),
            ("saveWIL","Ironwill","of Willpower","will saving throw",StatType.SaveWill),
            ("saveREF","Elusive","of Evasion", "reflex saving throw",StatType.SaveReflex),
            ("initiative","Swift","of Alacrity","initiative",StatType.Initiative),
        };
        
        private static readonly int[] saveBonusOne = { 1, 2, 2, 3, 3, 4 };
        private static readonly int[] saveBonusDouble = { 1, 2, 3, 4, 5, 6 };

        // --- Skills (SkillsBonus) ---
        private static readonly (string root, string name, string suffix, string desc, StatType stat)[] SkillRoots = new[]
        {
            ("skillMOB","Mobile",     "of Mobile",         "mobility",              StatType.SkillMobility),
            ("skillATH","Vigorous",   "of Vigor",          "athletics",             StatType.SkillAthletics),
            ("skillARC","Arcane",     "of the Arcanist",   "knowledge arcana",      StatType.SkillKnowledgeArcana),
            ("skillWOR","Scholar",    "of the Scholar",    "knowledge world",       StatType.SkillKnowledgeWorld),
            ("skillNAT","Pathfinder", "of the Pathfinder", "lore nature",           StatType.SkillLoreNature),
            ("skillREL","Saintly",    "of the Zealot",     "lore religion",         StatType.SkillLoreReligion),
            ("skillPERC","Vigilant",  "of Vigilance",      "perception",            StatType.SkillPerception),
            ("skillPERS","Diplomatic","of Diplomacy",      "persuasion",            StatType.SkillPersuasion),
            ("skillSTE","Silent",     "of Silence",        "stealth",               StatType.SkillStealth),
            ("skillTHI","Gambit",     "of Guile",          "Trickery",              StatType.SkillThievery),
            ("skillUMD","Mystic",     "of Attunement",     "use magic device",      StatType.SkillUseMagicDevice),

        };

        private static readonly int[] skillBonusOne = { 2, 3, 4, 5, 6, 8 };
        private static readonly int[] skillBonusDouble = { 2, 4, 6, 8, 10, 12 };

        // --- Combat Maneuver ---
        private static readonly (string root, string name, string suffix, string desc, StatType stat)[] ManeuverRoots = new[]
        {
            ("CMB","Grappling","of Grapple","combat maneuver bonus",StatType.AdditionalCMB),
            ("CMD","Immovable","of Immovable","combat maneuver defense",StatType.AdditionalCMD),
        };

        private static readonly int[] maneuverBonusOne = { 1, 2, 2, 3, 3, 4 };
        private static readonly int[] maneuverBonusDouble = { 1, 2, 3, 4, 5, 6 };

        // --- Caster Level ---
        private static readonly (string root, string name, string desc, StatType stat)[] CasterLevelRoots = new[]
        {
            ("casterLevel","Eldritch","caster level",StatType.BonusCasterLevel),
        };
        private static readonly int[] CLBonusOne = { 1, 1, 1, 1, 2, 2 };
        private static readonly int[] CLBonusDouble = { 1, 1, 2, 2, 3, 3 };

        // --- Caster (genéricos por rasgo) ---
        private static readonly (string root, string name, string desc)[] CasterGeneric = new[]
        {
            ("spellDC","Hexing","spell DC for all saving trhow against spells from the wielder casts"),
            ("spellDieBonus","Overcharged","each die rolled when casting a spell with descriptor fire, cold, elctricity, acid, sonic, force and cure"),
        };

        // --- Schools CL (SchoolCL) ---
        private static readonly (string root, string name, string desc)[] SchoolCLRoots = new[]
        {
            ("divinationCL","Revealing","caster level on divination school spells"),
            ("enchantmentCL","Amplifier","caster level on enchantment school spells"),
            ("evocationCL","Blasting","caster level on evocation school spells"),
            ("conjurationCL","Summoning","caster level on conjuration school spells"),
            ("abjurationCL","Warding","caster level on abjuration school spells"),
            ("illusionCL","Mirage","caster level on illusion school spells"),
            ("transmutationCL","Morphing","caster level on transmutation school spells"),
            ("necromancyCL","Grim","caster level on necromancy school spells"),
        };

        // --- Schools DC (SchoolDC) ---
        private static readonly (string root, string name, string desc)[] SchoolDCRoots = new[]
        {
            ("divinationDC","Insightful","DC on divination school spells"),
            ("enchantmentDC","Mesmeric","DC on enchantment school spells"),
            ("evocationDC","Cataclysmic","DC on evocation school spells"),
            ("conjurationDC","Binding","DC on conjuration school spells"),
            ("abjurationDC","Repelling","DC on abjuration school spells"),
            ("illusionDC","Chimeric","DC on illusion school spells"),
            ("transmutationDC","Mutable","DC on transmutacion school spells"),
            ("necromancyDC","Deathly","DC on necromancy school spells"),
        };

        // -----------------------------
        // Registro
        // -----------------------------
        public static void RegisterAll()
        {

            // Debuffs onlyHit — One-Handed
            {
                var dcs = GetDebuffDC(ActivationType.onlyHit, Handedness.OneHanded);
                foreach (var d in DebuffOnlyHitDefs)
                {
                    var tiers = Enumerable.Range(1, 6)
                        .Select(t => new EnchantTierConfig { AssetId = Id(d.root, t, Handedness.OneHanded), DC = dcs[t - 1] })
                        .ToList();

                    RegisterDebuffTiersFor(
                        tiers,
                        name: d.display,
                        nameRoot: RootWithHand(d.root, Handedness.OneHanded), // evita colisiones de bpName/loc keys
                        description: d.desc,
                        buff: d.buff,
                        durationDiceCount: d.dCount,
                        durationDiceSides: d.dSides,
                        savingThrowType: d.save,
                        activation: ActivationType.onlyHit
                    );

                    LootBuckets.AddRootVariant(EnchantType.OnHit, d.root, Handedness.OneHanded, AffixKind.Prefix, 10);
                }
            }

            // Debuffs onlyHit — Two-Handed
            {
                var dcs = GetDebuffDC(ActivationType.onlyHit, Handedness.TwoHanded);
                foreach (var d in DebuffOnlyHitDefs)
                {
                    var tiers = Enumerable.Range(1, 6)
                        .Select(t => new EnchantTierConfig { AssetId = Id(d.root, t, Handedness.TwoHanded), DC = dcs[t - 1] })
                        .ToList();

                    RegisterDebuffTiersFor(
                        tiers,
                        name: d.display,
                        nameRoot: RootWithHand(d.root, Handedness.TwoHanded),
                        description: d.desc,
                        buff: d.buff,
                        durationDiceCount: d.dCount,
                        durationDiceSides: d.dSides,
                        savingThrowType: d.save,
                        activation: ActivationType.onlyHit
                    );

                    LootBuckets.AddRootVariant(EnchantType.OnHit, d.root, Handedness.TwoHanded, AffixKind.Prefix, 10);
                }
            }
            // Debuffs onlyOnFirstHit — One-Handed
            {
                var dcs = GetDebuffDC(ActivationType.onlyOnFirstHit, Handedness.OneHanded);
                foreach (var d in DebuffFirstHitDefs)
                {
                    var tiers = Enumerable.Range(1, 6)
                        .Select(t => new EnchantTierConfig { AssetId = Id(d.root, t, Handedness.OneHanded), DC = dcs[t - 1] })
                        .ToList();

                    RegisterDebuffTiersFor(
                        tiers: tiers,
                        name: d.display,
                        nameRoot: RootWithHand(d.root, Handedness.OneHanded),
                        description: d.desc,
                        buff: d.buff,
                        durationDiceCount: d.dCount,
                        durationDiceSides: d.dSides,
                        savingThrowType: d.save,
                        activation: ActivationType.onlyOnFirstHit
                    );

                    LootBuckets.AddRootVariant(EnchantType.OnlyOnFirstHit, d.root, Handedness.OneHanded, AffixKind.Prefix, 10);
                }
            }

            // Debuffs onlyOnFirstHit — Two-Handed
            {
                var dcs = GetDebuffDC(ActivationType.onlyOnFirstHit, Handedness.TwoHanded);
                foreach (var d in DebuffFirstHitDefs)
                {
                    var tiers = Enumerable.Range(1, 6)
                        .Select(t => new EnchantTierConfig { AssetId = Id(d.root, t, Handedness.TwoHanded), DC = dcs[t - 1] })
                        .ToList();

                    RegisterDebuffTiersFor(
                        tiers: tiers,
                        name: d.display,
                        nameRoot: RootWithHand(d.root, Handedness.TwoHanded),
                        description: d.desc,
                        buff: d.buff,
                        durationDiceCount: d.dCount,
                        durationDiceSides: d.dSides,
                        savingThrowType: d.save,
                        activation: ActivationType.onlyOnFirstHit
                    );

                    LootBuckets.AddRootVariant(EnchantType.OnlyOnFirstHit, d.root, Handedness.TwoHanded, AffixKind.Prefix, 10);
                }
            }

            // --- Daño elemental/afín (EnergyDamage) ---
            // One-Handed
            foreach (var (root, name, desc, prefab) in DamageDefs)
            {
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig
                    {
                        AssetId = Id(root, t, Handedness.OneHanded),         // GUID: root.one.tN
                        DiceCount = DamageTierOneHanded[t - 1].dice,
                        DiceSide = DamageTierOneHanded[t - 1].sides
                    })
                    .ToList();

                RegisterDamageTiersFor(
                    tiers,
                    name: name,                                               // mismo display para 1H/2H
                    nameRoot: RootWithHand(root, Handedness.OneHanded),       // seed interna: root.one
                    description: desc,
                    prefab: prefab
                );

                LootBuckets.AddRootVariant(EnchantType.EnergyDamage, root, Handedness.OneHanded, AffixKind.Prefix, 10);
            }

            // Two-Handed
            foreach (var (root, name, desc, prefab) in DamageDefs)
            {
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig
                    {
                        AssetId = Id(root, t, Handedness.TwoHanded),         // GUID: root.two.tN
                        DiceCount = DamageTierTwoHanded[t - 1].dice,
                        DiceSide = DamageTierTwoHanded[t - 1].sides
                    })
                    .ToList();

                RegisterDamageTiersFor(
                    tiers,
                    name: name,                                               // mismo display
                    nameRoot: RootWithHand(root, Handedness.TwoHanded),       // seed interna: root.two
                    description: desc,
                    prefab: prefab
                );

                LootBuckets.AddRootVariant(EnchantType.EnergyDamage, root, Handedness.TwoHanded, AffixKind.Prefix, 10);
            }

            // --- Stats (StatsBonus) ---
            RegisterStat(StatRoots, EnchantType.StatsBonus, 10, statBonusOne, statBonusDouble);

            // --- Saves (SavesBonus) ---
            RegisterStat(SaveRoots, EnchantType.SavesBonus, 5, saveBonusOne, saveBonusDouble);

            // Skills (progresión 2/4/6/8/10/12)
            RegisterStat(SkillRoots, EnchantType.SkillsBonus, 5, skillBonusOne, skillBonusDouble);

            // Otros
            RegisterStat(ManeuverRoots, EnchantType.Maneuver, 5, maneuverBonusOne, maneuverBonusDouble);

            // CasterLevel
            RegisterCL(CasterLevelRoots, EnchantType.CasterLevel, 2, CLBonusOne, CLBonusDouble);

            // Caster genéricos (features): spellDC, spellDieBonus
            foreach (var (root, name, desc) in CasterGeneric)
            {
                var tiers = Enumerable.Range(1, 6).Select(t => new EnchantTierConfig
                {
                    AssetId = Id(root, t),
                    Feat = Feature(root, t),
                    BonusDescription = MapBonusDesc(root, t)
                }).ToList();

                RegisterWeaponFeaturesTiersFor(tiers, name, root, description: desc);
                LootBuckets.AddRoot(EnchantType.Caster, root, 10);
            }

            // SchoolCL (features)
            foreach (var (root, name, desc) in SchoolCLRoots)
            {
                var tiers = Enumerable.Range(1, 6).Select(t => new EnchantTierConfig
                {
                    AssetId = Id(root, t),
                    Feat = Feature(root, MapCLFeatTier(t)),
                    BonusDescription = MapCLBonusDesc(t)
                }).ToList();

                RegisterWeaponFeaturesTiersFor(tiers, name, root, description: desc);
                LootBuckets.AddRoot(EnchantType.SchoolCL, root, 10);
            }

            // SchoolDC (features)
            foreach (var (root, name, desc) in SchoolDCRoots)
            {
                var tiers = Enumerable.Range(1, 6).Select(t => new EnchantTierConfig
                {
                    AssetId = Id(root, t),
                    Feat = Feature(root, MapCLFeatTier(t)),
                    BonusDescription = MapCLBonusDesc(t)
                }).ToList();

                RegisterWeaponFeaturesTiersFor(tiers, name, root, description: desc);
                LootBuckets.AddRoot(EnchantType.SchoolDC, root, 10);
            }

            // Precios (tal cual)
            RegisterWeaponPriceForTiers(new List<EnchantTierConfig>
            {
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_20").ToString() },
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_40").ToString() },
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_80").ToString() },
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_160").ToString() },
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_320").ToString() },
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_640").ToString() },
                new EnchantTierConfig { AssetId = GuidUtil.EnchantGuid("price_1280").ToString() },
            });
        }

        // -----------------------------
        // Helpers
        // -----------------------------
        private static string Id(string root, int tier) => GuidUtil.EnchantGuid($"{root}.t{tier}").ToString();

        private static string Feature(string root, int tier) => GuidUtil.FeatureGuid($"{root}.t{tier}").ToString();

        // Mapea tiers 1..6 -> feat tiers (1,1,2,2,3,3)
        private static int MapCLFeatTier(int t) => t <= 2 ? 1 : (t <= 4 ? 2 : 3);
        // Mapea BonusDescription con el mismo patrón
        private static int MapCLBonusDesc(int t) => t <= 2 ? 1 : (t <= 4 ? 2 : 3);

        // spellDC y spellDieBonus usan patrón 1,1,1,2,2,3 en BonusDescription
        private static int MapBonusDesc(string root, int tier)
        {
            if (root == "spellDC" || root == "spellDieBonus")
            {
                if (tier <= 3) return 1;
                if (tier <= 5) return 2;
                return 3;
            }
            return 1;
        }
        private static void RegisterStatLikeVariant(
            (string root, string name, string suffix, string desc, StatType stat)[] defs,
            Handedness hand,
            int[] bonuses,
            EnchantType type,
            int weight
        )
        {
            foreach (var (root, name, suffix, desc, stat) in defs)
            {
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig
                    {
                        AssetId = Id(root, t, hand),
                        Bonus = bonuses[t - 1]
                    })
                    .ToList();

                RegisterWeaponStatsTiersFor(
                    tiers,
                    name: name,
                    nameRoot: RootWithHand(root, hand),  // p.ej. statSTR.one / .dbl
                    description: desc,
                    stat: stat,
                    suffix: suffix                       // p.ej. "of Might"
                );

                LootBuckets.AddRootVariant(type, root, hand, AffixKind.Suffix, weight);
            }
        }
        private static void RegisterStat(
            (string root, string name, string suffix, string desc, StatType stat)[] defs,
            EnchantType type,
            int weight,
            int[] bonusOne,
            int[] bonusDouble
        )
        {
            RegisterStatLikeVariant(defs, Handedness.OneHanded, bonusOne, type, weight);
            RegisterStatLikeVariant(defs, Handedness.Double, bonusDouble, type, weight);
        }
        private static void RegisterCLVariant(
            (string root, string name, string desc, StatType stat)[] defs,
            Handedness hand,
            int[] bonuses,
            EnchantType type,
            int weight
        )
        {

            foreach (var (root, name, desc, stat) in defs)
            {
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig
                    {
                        AssetId = Id(root, t, hand),
                        Bonus = bonuses[t - 1]
                    })
                    .ToList();

                RegisterWeaponStatsTiersFor(
                    tiers,
                    name: name,
                    nameRoot: RootWithHand(root, hand),
                    description: desc,
                    stat: stat,
                    suffix: null,                // usará 'name' (“Eldritch”)
                    affix: AffixKind.Prefix      // ← importante
                );

                LootBuckets.AddRootVariant(type, root, hand, AffixKind.Prefix, weight);
            }
        }
        private static void RegisterCL(
            (string root, string name, string desc, StatType stat)[] defs,
            EnchantType type,
            int weight,
            int[] bonusOne,
            int[] bonusDouble
        )
        {
            RegisterCLVariant(defs, Handedness.OneHanded, bonusOne, type, weight);
            RegisterCLVariant(defs, Handedness.Double, bonusDouble, type, weight);
        }
    }
    /// Acumula IDs por tipo y tier y además por "Value" (peso).
    /// Cada AddRoot puede indicar un value distinto.
    internal static class LootBuckets
    {
        private static string RootWithHand(string root, Handedness hand) => hand switch
        {
            Handedness.OneHanded => $"{root}.one",
            Handedness.TwoHanded => $"{root}.two",
            Handedness.Double => $"{root}.dbl",
            _ => $"{root}.one"
        };
        // Mapa: Tipo -> (Value -> [6 listas de tiers])
        private static readonly Dictionary<EnchantType, Dictionary<int, HashSet<string>[]>>
          _store = new Dictionary<EnchantType, Dictionary<int, HashSet<string>[]>>();

        private static int _defaultValue = 1;

        /// Opcional: cambia el valor por defecto usado por AddRoot(..., value: null)
        public static void SetDefaultValue(int v) { _defaultValue = v; }

        public static void Clear() { _store.Clear(); }

        public static void AddRoot(EnchantType type, string root, int? value = null)
        {
            int val = value ?? _defaultValue;
            var tiers = GetBucket(type, val);
            for (int t = 1; t <= 6; t++)
            {
                tiers[t - 1].Add(GuidUtil.EnchantGuid(string.Format("{0}.t{1}", root, t)).ToString());
            }
        }

        public static void AddRoots(EnchantType type, IEnumerable<string> roots, int? value = null)
        {
            foreach (var r in roots) AddRoot(type, r, value);
        }

        /// Vuelca todo a EnchantList.Item. Cada (Type,Value) genera un EnchantData con ese Value.
        public static void FlushToEnchantList(List<EnchantData> target)
        {
            target.Clear();

            foreach (var byType in _store)
            {
                var type = byType.Key;

                foreach (var byValue in byType.Value)
                {
                    int value = byValue.Key;
                    var tiers = byValue.Value; // HashSet<string>[6]
                    if (tiers.All(l => l.Count == 0)) continue;

                    // buffers por variante y por tier
                    var perHand = new Dictionary<Handedness, List<string>[]> {
                        { Handedness.OneHanded, new[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() } },
                        { Handedness.TwoHanded, new[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() } },
                        { Handedness.Double,    new[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() } },
                    };

                    for (int i = 0; i < 6; i++)
                    {
                        foreach (var id in tiers[i])
                        {
                            // si por lo que sea no estuviera (caso legacy), decide un fallback
                            var hand = _handById.TryGetValue(id, out var h) ? h : Handedness.OneHanded;
                            perHand[hand][i].Add(id);
                        }
                    }

                    // emite hasta 3 EnchantData (uno por mano que tenga contenido)
                    foreach (var kv in perHand)
                    {
                        var hand = kv.Key;
                        var arrs = kv.Value;

                        if (arrs.All(list => list.Count == 0)) continue;

                        target.Add(new EnchantData
                        {
                            AssetIDT1 = arrs[0].ToArray(),
                            AssetIDT2 = arrs[1].ToArray(),
                            AssetIDT3 = arrs[2].ToArray(),
                            AssetIDT4 = arrs[3].ToArray(),
                            AssetIDT5 = arrs[4].ToArray(),
                            AssetIDT6 = arrs[5].ToArray(),
                            Value = value,     // mismo peso del bucket
                            Type = type,      // mismo tipo del bucket
                            Hand = hand       // ← ahora sí, sin ambigüedad
                        });
                    }
                }
            }
        }

        // ---------- helpers internos ----------
        private static HashSet<string>[] GetBucket(EnchantType type, int value)
        {
            if (!_store.TryGetValue(type, out var byValue))
            {
                byValue = new Dictionary<int, HashSet<string>[]>();
                _store[type] = byValue;
            }
            if (!byValue.TryGetValue(value, out var tiers))
            {
                tiers = new HashSet<string>[6] {
                    new HashSet<string>(), new HashSet<string>(), new HashSet<string>(),
                    new HashSet<string>(), new HashSet<string>(), new HashSet<string>()
                };
                byValue[value] = tiers;
            }
            return tiers;
        }
        
        private static readonly Dictionary<string, Handedness> _handById = new Dictionary<string, Handedness>();
        private static readonly Dictionary<string, AffixKind> _affixById = new Dictionary<string, AffixKind>();

        public static void AddRootVariant(EnchantType type, string root, Handedness hand, AffixKind affix, int? value = null)
        {
            int val = value ?? _defaultValue;
            var tiers = GetBucket(type, val);

            for (int t = 1; t <= 6; t++)
            {
                var seed = RootWithHand(root, hand) + ".t" + t;
                var guid = GuidUtil.EnchantGuid(seed).ToString();
                tiers[t - 1].Add(guid);

                _handById[guid] = hand;   // etiqueta variante
                _affixById[guid] = affix; // etiqueta affix
            }
        }
    }

    // -----------------------------
    // Punto de entrada anterior (RegisterAll)
    // -----------------------------
    internal class EnchantRegister
    {
        public static void RegisterAll()
        {
            LootBuckets.Clear();
            LootBuckets.SetDefaultValue(1);
            EnchantCatalog.RegisterAll();     // Crea todos los blueprints desde el catálogo
            LootBuckets.FlushToEnchantList(EnchantList.Item);
        }
    }
}
