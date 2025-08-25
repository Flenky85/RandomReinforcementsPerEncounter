using RandomReinforcementsPerEncounter.Domain.Models;
using System;
using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class LootBuckets
    {
        private const int MaxTier = 6;

        private static readonly Dictionary<EnchantType, Dictionary<int, HashSet<string>[]>>
          _store = new Dictionary<EnchantType, Dictionary<int, HashSet<string>[]>>();

        private static int _defaultValue = 1;

        public static void SetDefaultValue(int v) => _defaultValue = v;

        public static void Clear()
        {
            _store.Clear();
            _handById.Clear();
            _affixById.Clear();
            _applyBothOnDoubleById.Clear();
        }

        private static HashSet<string>[] GetBucket(EnchantType type, int value)
        {
            if (!_store.TryGetValue(type, out var byValue))
            {
                byValue = new Dictionary<int, HashSet<string>[]>();
                _store[type] = byValue;
            }
            if (!byValue.TryGetValue(value, out var tiers))
            {
                tiers = NewTierSets();
                byValue[value] = tiers;
            }
            return tiers;
        }

        private static HashSet<string>[] NewTierSets()
        {
            var a = new HashSet<string>[MaxTier];
            for (int i = 0; i < MaxTier; i++) a[i] = new HashSet<string>();
            return a;
        }

        private static List<string>[] NewTierLists()
        {
            var a = new List<string>[MaxTier];
            for (int i = 0; i < MaxTier; i++) a[i] = new List<string>();
            return a;
        }

        private static bool IsAllEmpty(HashSet<string>[] sets)
        {
            for (int i = 0; i < sets.Length; i++) if (sets[i].Count > 0) return false;
            return true;
        }

        private static bool IsAllEmpty(List<string>[] lists)
        {
            for (int i = 0; i < lists.Length; i++) if (lists[i].Count > 0) return false;
            return true;
        }

        private static readonly Dictionary<string, WeaponGrip> _handById = new Dictionary<string, WeaponGrip>();
        private static readonly Dictionary<string, AffixKind> _affixById = new Dictionary<string, AffixKind>();
        private static readonly Dictionary<string, bool> _applyBothOnDoubleById = new Dictionary<string, bool>();
        private static readonly Dictionary<string, string> _rootById = new Dictionary<string, string>();

        public static IEnumerable<(string id, int weight, WeaponGrip hand, bool applyBothOnDouble)>
        GetCandidatesByTierAndAffix(int tier, AffixKind affix)
        {
            if (tier < 1 || tier > MaxTier) yield break;

            foreach (var byValue in _store.Values)
                foreach (var kv in byValue) 
                {
                    int weight = kv.Key;
                    var set = kv.Value[tier - 1];

                    foreach (var id in set)
                    {
                        if (_affixById.TryGetValue(id, out var a) && a == affix
                            && _handById.TryGetValue(id, out var h))
                        {
                            bool dup = _applyBothOnDoubleById.TryGetValue(id, out var b) && b;
                            yield return (id, weight, h, dup);
                        }
                    }
                }
        }
    }
}
