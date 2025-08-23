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
        public WeaponGrip Hand;
    }

    public static class EnchantList
    {
        public static readonly List<EnchantData> Item = new List<EnchantData>();
    }



    // -----------------------------
    // Catálogo único de raíces (roots) + registro y loot
    // -----------------------------
    internal static class EnchantCatalog1
    {

        public static void RegisterAll()
        {

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
                    var perHand = new Dictionary<WeaponGrip, List<string>[]> {
                        { WeaponGrip.OneHanded, new[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() } },
                        { WeaponGrip.TwoHanded, new[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() } },
                        { WeaponGrip.Double,    new[] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() } },
                    };

                    for (int i = 0; i < 6; i++)
                    {
                        foreach (var id in tiers[i])
                        {
                            // si por lo que sea no estuviera (caso legacy), decide un fallback
                            var hand = _handById.TryGetValue(id, out var h) ? h : WeaponGrip.OneHanded;
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
        
        private static readonly Dictionary<string, WeaponGrip> _handById = new Dictionary<string, WeaponGrip>();
        private static readonly Dictionary<string, AffixKind> _affixById = new Dictionary<string, AffixKind>();

        public static void AddRootVariant(EnchantType type, string root, WeaponGrip hand, AffixKind affix, int? value = null)
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

}
