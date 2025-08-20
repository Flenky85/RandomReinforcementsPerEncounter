using System.Linq;
using UnityEngine;
using Kingmaker.Enums;
using RandomReinforcementsPerEncounter.Config;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.itemlist.Weapons;

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

        /// <summary> 20% por categoría de WeaponType; soporta oversize. </summary>
        public static WeaponPick PickRandomWeapon(float oversizedChance = 0.15f)
        {
            bool wantOver = UnityEngine.Random.value < oversizedChance;
            var wanted = RollWeaponType();

            for (int tries = 0; tries < 5; tries++)
            {
                if (wantOver)
                {
                    var bucket = weaponListOversized.Item.Where(w => w.Type == wanted).ToList();
                    if (bucket.Count > 0)
                    {
                        var it = bucket[UnityEngine.Random.Range(0, bucket.Count)];
                        return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = true };
                    }
                }
                else
                {
                    var bucket = weaponList.Item.Where(w => w.Type == wanted).ToList();
                    if (bucket.Count > 0)
                    {
                        var it = bucket[UnityEngine.Random.Range(0, bucket.Count)];
                        return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = false };
                    }
                }
                wanted = RollWeaponType();
            }

            if (wantOver && weaponListOversized.Item.Count > 0)
            {
                var it = weaponListOversized.Item[UnityEngine.Random.Range(0, weaponListOversized.Item.Count)];
                return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = true };
            }
            if (!wantOver && weaponList.Item.Count > 0)
            {
                var it = weaponList.Item[UnityEngine.Random.Range(0, weaponList.Item.Count)];
                return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = false };
            }
            return default;
        }
    }
}
