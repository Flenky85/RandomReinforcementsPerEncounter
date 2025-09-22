using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.Config.Settings;
using RandomReinforcementsPerEncounter.Content.WeaponCatalog;
using RandomReinforcementsPerEncounter.Domain.Models;
using System.Linq;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class WeaponRoller
    {
        private static WeaponType RollWeaponType()
        {
            int wOHM = WeaponLootBalance.OneHandedMelee;
            int wTHM = WeaponLootBalance.TwoHandedMelee;
            int wOHR = WeaponLootBalance.OneHandedRanged;
            int wTHR = WeaponLootBalance.TwoHandedRanged;
            int wDBL = WeaponLootBalance.Double;

            int total = wOHM + wTHM + wOHR + wTHR + wDBL;
            int roll = UnityEngine.Random.Range(1, total + 1);

            if (roll <= wOHM) return WeaponType.OneHandedMelee; roll -= wOHM;
            if (roll <= wTHM) return WeaponType.TwoHandedMelee; roll -= wTHM;
            if (roll <= wOHR) return WeaponType.OneHandedRanged; roll -= wOHR;
            if (roll <= wTHR) return WeaponType.TwoHandedRanged;
            return WeaponType.Double;
        }

        public static WeaponPick PickRandomWeapon()
        {
            float oversizedChance = Mathf.Clamp01(ModSettings.Instance.OversizedPct / 100f);

            bool wantOver = UnityEngine.Random.value < oversizedChance;
            var wanted = RollWeaponType();

            for (int tries = 0; tries < 5; tries++)
            {
                if (wantOver)
                {
                    var bucket = WeaponCatalogOversized.Item.Where(w => w.Type == wanted).ToList();
                    if (bucket.Count > 0)
                    {
                        var it = bucket[UnityEngine.Random.Range(0, bucket.Count)];
                        return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = true };
                    }
                }
                else
                {
                    var bucket = WeaponCatalog.Item.Where(w => w.Type == wanted).ToList();
                    if (bucket.Count > 0)
                    {
                        var it = bucket[UnityEngine.Random.Range(0, bucket.Count)];
                        return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = false };
                    }
                }
                wanted = RollWeaponType();
            }

            if (wantOver && WeaponCatalogOversized.Item.Count > 0)
            {
                var it = WeaponCatalogOversized.Item[UnityEngine.Random.Range(0, WeaponCatalogOversized.Item.Count)];
                return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = true };
            }
            if (!wantOver && WeaponCatalog.Item.Count > 0)
            {
                var it = WeaponCatalog.Item[UnityEngine.Random.Range(0, WeaponCatalog.Item.Count)];
                return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = false };
            }
            return default;
        }
    }
}
