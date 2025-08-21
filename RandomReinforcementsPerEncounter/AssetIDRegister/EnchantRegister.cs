using System.Collections.Generic;
using System.Linq;
using Kingmaker.EntitySystem.Stats;
using static RandomReinforcementsPerEncounter.EnchantFactory;
using RandomReinforcementsPerEncounter.Domain.Models;

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
        Others,
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
        // Prefabs (tal cual los tenías)
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

        // --- Debuffs (onlyHit) ---
        private static readonly string[] DebuffOnlyHitRoots = new[]
        {
            "shaken","blindness","dazzled","sickened","staggered","fatigue","confusion","entangled","slowed"
        };
        private static readonly int[] DebuffOnlyHitDC = { 11, 14, 17, 20, 23, 26 };
        private static readonly Dictionary<string, (string Display, string Desc, string Buff, SavingThrowType Save, int dCount, int dSides)>
        DebuffOnlyHitMeta = new Dictionary<string, (string, string, string, SavingThrowType, int, int)>()
        {
            ["shaken"] = ("Fearsome", "shaken", "25ec6cb6ab1845c48a95f9c20b034220", SavingThrowType.Will, 1, 3),
            ["blindness"] = ("Blinding", "blinded", "0ec36e7596a4928489d2049e1e1c76a7", SavingThrowType.Fortitude, 1, 3),
            ["dazzled"] = ("Dazzling", "dazzled", "df6d1020da07524423afbae248845ecc", SavingThrowType.Fortitude, 1, 3),
            ["sickened"] = ("Sickening", "sickened", "4e42460798665fd4cb9143ffa7ada323", SavingThrowType.Fortitude, 1, 1),
            ["staggered"] = ("Staggering", "staggered", "df3950af5a783bd4d91ab73eb8fa0fd3", SavingThrowType.Fortitude, 1, 3),
            ["fatigue"] = ("Fatiguing", "fatigued", "e6f2fc5d73d88064583cb828801172f4", SavingThrowType.Fortitude, 1, 3),
            ["confusion"] = ("Confusing", "confused", "886c7407dc623dc499b9f1465ff382df", SavingThrowType.Will, 1, 1),
            ["entangled"] = ("Entangling", "entangled", "f7f6260726117cf4b90a6086b05d2e38", SavingThrowType.Reflex, 1, 1),
            ["slowed"] = ("Slowing", "slowed", "488e53ede2802ff4da9372c6a494fb66", SavingThrowType.Reflex, 1, 1),
        };

        // --- Debuffs (onlyOnFirstHit) ---
        private static readonly string[] DebuffFirstHitRoots = new[]
        {
            "frightened","stunned","daze","sleep","paralyzed","exhausted","nauseated","prone","domination"
        };
        private static readonly int[] DebuffFirstHitDC = { 13, 17, 21, 25, 29, 33 };
        private static readonly Dictionary<string, (string Display, string Desc, string Buff, SavingThrowType Save, int dCount, int dSides)>
        DebuffFirstHitMeta = new Dictionary<string, (string, string, string, SavingThrowType, int, int)>()
        {
            ["frightened"] = ("Frightening", "frightened", "f08a7239aa961f34c8301518e71d4cdf", SavingThrowType.Will, 1, 3),
            ["stunned"] = ("Stunning", "stunned", "09d39b38bb7c6014394b6daced9bacd3", SavingThrowType.Fortitude, 1, 1),
            ["daze"] = ("Dazing", "dazed", "d2e35b870e4ac574d9873b36402487e5", SavingThrowType.Will, 1, 3),
            ["sleep"] = ("Slumbering", "asleep", "c9937d7846aa9ae46991e9f298be644a", SavingThrowType.Will, 1, 3),
            ["paralyzed"] = ("Paralyzing", "paralyzed", "4d5a2e4c34d24acca575c10003cf8fbc", SavingThrowType.Will, 1, 1),
            ["exhausted"] = ("Exhausting", "exhausted", "46d1b9cc3d0fd36469a471b047d773a2", SavingThrowType.Fortitude, 1, 3),
            ["nauseated"] = ("Nauseating", "nauseated", "956331dba5125ef48afe41875a00ca0e", SavingThrowType.Fortitude, 1, 3),
            ["prone"] = ("Toppling", "prone", "24cf3deb078d3df4d92ba24b176bda97", SavingThrowType.Reflex, 1, 1),
            ["domination"] = ("Dominating", "dominated", "c0f4e1c24c9cd334ca988ed1bd9d201f", SavingThrowType.Will, 1, 1),
        };

        // --- Daño elemental/afín (EnergyDamage) ---
        private static readonly (string root, string name, string desc, string prefab)[] DamageDefs = new[]
        {
            ("fire","Flaming","fire",PREFAB_FLAMING),
            ("cold","Frost","cold",PREFAB_FROST),
            ("electricity","Shock","electricity",PREFAB_SHOCK),
            ("sonic","Thundering","sonic",PREFAB_SONIC),
            ("acid","Corrosive","acid",PREFAB_CORROSIVE),
            ("unholy","Unholy","negative damage",PREFAB_UNHOLY),
            ("holy","Holy","holy",PREFAB_HOLY),
        };

        // Progresión de daño por tier (tal cual tu patrón)
        private static readonly (int dice, int sides)[] DamageTier =
        {
            (1,3),(1,6),(1,10),(2,6),(2,8),(2,10)
        };

        // --- Stats (StatsBonus) ---
        private static readonly (string root, string name, string desc, StatType stat)[] StatRoots = new[]
        {
            ("statSTR","Mighty","strength",StatType.Strength),
            ("statDEX","Graceful","dexterity",StatType.Dexterity),
            ("statCON","Resilient","constitution",StatType.Constitution),
            ("statINT","Cunning","intelligence",StatType.Intelligence),
            ("statWIS","Sage","wisdom",StatType.Wisdom),
            ("statCHA","Glamorous","charisma",StatType.Charisma),
        };

        // --- Saves (SavesBonus) ---
        private static readonly (string root, string name, string desc, StatType stat)[] SaveRoots = new[]
        {
            ("saveFOR","Enduring","fortitude saving throw",StatType.SaveFortitude),
            ("saveWIL","Ironwill","will saving throw",StatType.SaveWill),
            ("saveREF","Elusive","reflex saving throw",StatType.SaveReflex),
        };

        // --- Skills (SkillsBonus) ---
        private static readonly (string root, string name, string desc, StatType stat)[] SkillRoots = new[]
        {
            ("skillMOB","Mobile","mobility",StatType.SkillMobility),
            ("skillATH","Vigorous","athletics",StatType.SkillAthletics),
            ("skillARC","Arcane","knowledge arcana",StatType.SkillKnowledgeArcana),
            ("skillWOR","Scholar","knowledge world",StatType.SkillKnowledgeWorld),
            ("skillNAT","Pathfinder","lore nature",StatType.SkillLoreNature),
            ("skillREL","Saintly","lore religion",StatType.SkillLoreReligion),
            ("skillPERC","Vigilant","perception",StatType.SkillPerception),
            ("skillPERS","Diplomatic","persuasion",StatType.SkillPersuasion),
            ("skillSTE","Silent","stealth",StatType.SkillStealth),
            ("skillTHI","Gambit","Trickery",StatType.SkillThievery),
            ("skillUMD","Mystic","use magic device",StatType.SkillUseMagicDevice),
        };

        // --- Otros (Others) ---
        private static readonly (string root, string name, string desc, StatType stat)[] OtherRoots = new[]
        {
            ("initiative","Swift","initiative",StatType.Initiative),
            ("CMB","Grappling","combat maneuver bonus",StatType.AdditionalCMB),
            ("CMD","Immovable","combat maneuver defense",StatType.AdditionalCMD),
            ("casterLevel","Eldritch","caster level",StatType.BonusCasterLevel),
        };

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
            ("transmutacionCL","Morphing","caster level on transmutacion school spells"),
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
            ("transmutacionDC","Mutable","DC on transmutacion school spells"),
            ("necromancyDC","Deathly","DC on necromancy school spells"),
        };

        // -----------------------------
        // Registro
        // -----------------------------
        public static void RegisterAll()
        {
            // Debuffs onlyHit
            foreach (var root in DebuffOnlyHitRoots)
            {
                var meta = DebuffOnlyHitMeta[root];
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig { AssetId = Id(root, t), DC = DebuffOnlyHitDC[t - 1] })
                    .ToList();

                RegisterDebuffTiersFor(
                    tiers,
                    name: meta.Display,
                    nameRoot: root,
                    description: meta.Desc,
                    buff: meta.Buff,
                    durationDiceCount: meta.dCount,
                    durationDiceSides: meta.dSides,
                    savingThrowType: meta.Save,
                    activation: ActivationType.onlyHit
                );
                LootBuckets.AddRoot(EnchantType.OnHit, root, 10);
            }

            // Debuffs onlyOnFirstHit
            foreach (var root in DebuffFirstHitRoots)
            {
                var meta = DebuffFirstHitMeta[root];
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig { AssetId = Id(root, t), DC = DebuffFirstHitDC[t - 1] })
                    .ToList();

                RegisterDebuffTiersFor(
                    tiers,
                    name: meta.Display,
                    nameRoot: root,
                    description: meta.Desc,
                    buff: meta.Buff,
                    durationDiceCount: meta.dCount,
                    durationDiceSides: meta.dSides,
                    savingThrowType: meta.Save,
                    activation: ActivationType.onlyOnFirstHit
                );
                LootBuckets.AddRoot(EnchantType.OnlyOnFirstHit, root, 10);
            }

            // Daño elemental/afín
            foreach (var (root, name, desc, prefab) in DamageDefs)
            {
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig
                    {
                        AssetId = Id(root, t),
                        DiceCount = DamageTier[t - 1].dice,
                        DiceSide = DamageTier[t - 1].sides
                    })
                    .ToList();

                var dmgRoots = DamageDefs.Select(d => d.root);
                LootBuckets.AddRoots(EnchantType.EnergyDamage, dmgRoots, 10);

                RegisterDamageTiersFor(
                    tiers,
                    name: name,
                    nameRoot: root,
                    description: desc,
                    prefab: prefab
                );
            }

            // Stats
            foreach (var (root, name, desc, stat) in StatRoots)
            {
                RegisterWeaponStatsTiersFor(TierLinear(root, +1), name, root, desc, stat);
                LootBuckets.AddRoot(EnchantType.StatsBonus, root, 10);
            }

            // Saves
            foreach (var (root, name, desc, stat) in SaveRoots)
            {
                RegisterWeaponStatsTiersFor(TierLinear(root, +1), name, root, desc, stat);
                LootBuckets.AddRoot(EnchantType.SavesBonus, root, 5);
            }

            // Skills (progresión 2/4/6/8/10/12)
            foreach (var (root, name, desc, stat) in SkillRoots)
            {
                RegisterWeaponStatsTiersFor(TierLinear(root, bonusPerTier: 2), name, root, desc, stat);
                LootBuckets.AddRoot(EnchantType.SkillsBonus, root, 5);
            }

            // Otros
            foreach (var (root, name, desc, stat) in OtherRoots)
            {
                var inc = (root == "casterLevel") ? new[] { 1, 1, 1, 2, 2, 3 } : new[] { 2, 4, 6, 8, 10, 12 };
                var tiers = Enumerable.Range(1, 6)
                    .Select(t => new EnchantTierConfig { AssetId = Id(root, t), Bonus = inc[t - 1] })
                    .ToList();

                RegisterWeaponStatsTiersFor(tiers, name, root, desc, stat);
                int weight = root == "casterLevel" ? 2 : 5;
                LootBuckets.AddRoot(EnchantType.Others, root, weight);
            }

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

        private static List<EnchantTierConfig> TierLinear(string root, int bonusPerTier)
        {
            var bonuses = Enumerable.Range(1, 6).Select(t => t * bonusPerTier).ToArray();
            return Enumerable.Range(1, 6)
                .Select(t => new EnchantTierConfig { AssetId = Id(root, t), Bonus = bonuses[t - 1] })
                .ToList();
        }

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
    }
    /// Acumula IDs por tipo y tier y además por "Value" (peso).
    /// Cada AddRoot puede indicar un value distinto.
    internal static class LootBuckets
    {
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
                    var tiers = byValue.Value; // 6 listas
                    if (tiers.All(l => l.Count == 0)) continue;

                    target.Add(new EnchantData
                    {
                        AssetIDT1 = tiers[0].ToArray(),
                        AssetIDT2 = tiers[1].ToArray(),
                        AssetIDT3 = tiers[2].ToArray(),
                        AssetIDT4 = tiers[3].ToArray(),
                        AssetIDT5 = tiers[4].ToArray(),
                        AssetIDT6 = tiers[5].ToArray(),
                        Value = value,
                        Type = type
                    });
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
