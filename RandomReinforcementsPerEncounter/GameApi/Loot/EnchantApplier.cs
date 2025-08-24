using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Items;
using RandomReinforcementsPerEncounter.GameApi.Mechanics;
using RandomReinforcementsPerEncounter.Domain.Models;
using RandomReinforcementsPerEncounter.GameApi.Loot;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class EnchantApplier
    {
        private static bool HasEnchant(ItemEntity item, BlueprintItemEnchantment ench)
            => item?.Enchantments?.Any(e => e?.Blueprint == ench) == true;

        public static void AddEnchants(ItemEntityWeapon entity, bool applyToSecond, params BlueprintItemEnchantment[] enchants)
        {
            if (entity == null || enchants == null || enchants.Length == 0) return;

            var ctx = RRECtx.Permanent();

            foreach (var e in enchants)
                if (e != null)
                    entity.AddEnchantment(e, ctx);

            if (applyToSecond && entity.Second != null)
            {
                var second = entity.Second;
                foreach (var e in enchants)
                {
                    if (e == null) continue;
                    if (HasEnchant(second, e)) continue;
                    second.AddEnchantment(e, ctx);
                }
            }
        }

        private static int MapTierWithDiscount(int tier) => tier switch
        {
            1 => 1,
            2 => 1,
            3 => 2,
            4 => 2,
            5 => 3,
            6 => 4,
            _ => tier < 1 ? 1 : (tier > 6 ? 4 : tier),
        };

        private static BlueprintItemEnchantment PriceForTier(int tier) => tier switch
        {
            1 => PriceRefs.PriceT1,
            2 => PriceRefs.PriceT2,
            3 => PriceRefs.PriceT3,
            4 => PriceRefs.PriceT4,
            5 => PriceRefs.PriceT5,
            6 => PriceRefs.PriceT6,
            _ => PriceRefs.PriceT1,
        };
        /*
        public static void ApplyRandomTierEnchant(ItemEntityWeapon entity, int tier, int[] chances)
        {
            var baseEnchant = LootUtils.TryLoadEnchant(LootRefs.GetWeaponEnchantIdForTier(tier));
            AddEnchants(entity, true, baseEnchant, PriceForTier(tier));

            WeaponGrip weaponType =
                (entity?.Blueprint?.SecondWeapon != null) ? WeaponGrip.Double :
                (entity?.Blueprint?.IsTwoHanded == true) ? WeaponGrip.TwoHanded :
                WeaponGrip.OneHanded;

            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            int[] limitedChances = (chances == null || tier <= 0) ? Array.Empty<int>() : chances.Take(tier).ToArray();
            if (limitedChances.Length == 0) return;

            for (int i = 0; i < tier; i++)
            {
                int enchantTier = TierChances.GetRandomTier(limitedChances);

                var extra = EnchantPicker.PickRandomEnchantByTier(enchantTier, weaponType, out EnchantType pickedType, usedIds);
                if (extra == null)
                {
                    UnityEngine.Debug.Log($"[RRE] Pool vacío en iter {i + 1}/{tier} para T{tier} ({weaponType}).");
                    break;
                }

                int effectiveTier = enchantTier;
                bool applyDiscount =
                    (weaponType == WeaponGrip.OneHanded) ||
                    (weaponType == WeaponGrip.Double &&
                     (pickedType == EnchantType.OnHit || pickedType == EnchantType.OnlyOnFirstHit || pickedType == EnchantType.EnergyDamage));

                if (applyDiscount)
                    effectiveTier = MapTierWithDiscount(enchantTier);

                var mainExtra = applyDiscount ? RemapPickedEnchantToTier(extra, effectiveTier) : extra;
                if (mainExtra == null) mainExtra = extra;

                string appliedId = mainExtra.AssetGuid.ToString();

                if (HasEnchant(entity, mainExtra) || usedIds.Contains(appliedId))
                {
                    i--;
                    usedIds.Add(extra.AssetGuid.ToString());
                    usedIds.Add(appliedId);
                    continue;
                }

                bool duplicateToSecond =
                    (weaponType == WeaponGrip.Double) &&
                    (pickedType == EnchantType.OnHit || pickedType == EnchantType.OnlyOnFirstHit || pickedType == EnchantType.EnergyDamage);

                AddEnchants(entity, duplicateToSecond, mainExtra);

                usedIds.Add(extra.AssetGuid.ToString());
                usedIds.Add(appliedId);

                UnityEngine.Debug.Log($"[RRE] [{i + 1}/{tier}] Extra enchant T{tier} → T{effectiveTier} aplicado " +
                          $"({weaponType}, {pickedType}). DuplicadoSecond={duplicateToSecond}");
            }
        }*/

        public static void ApplyRandomTierEnchant(ItemEntityWeapon entity, int tier, int[] chances)
        {
            // 1) Enchant base por tier (igual que antes)
            var baseEnchant = LootUtils.TryLoadEnchant(LootRefs.GetWeaponEnchantIdForTier(tier));
            AddEnchants(entity, true, baseEnchant, PriceForTier(tier));

            // 2) Tipo de empuñadura y si es a distancia
            WeaponGrip weaponGrip =
                (entity?.Blueprint?.SecondWeapon != null) ? WeaponGrip.Double :
                (entity?.Blueprint?.IsTwoHanded == true) ? WeaponGrip.TwoHanded :
                WeaponGrip.OneHanded;

            bool isRanged = entity?.Blueprint?.IsRanged == true;

            // 3) Control de duplicados en esta ejecución
            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // 4) chances → distribución de tiers válida para este item (limitada al tier del arma)
            int[] limitedChances = (chances == null || tier <= 0) ? Array.Empty<int>() : chances.Take(tier).ToArray();
            if (limitedChances.Length == 0) return;

            // 5) Alternancia: empezamos aleatorio y luego alternamos estricto
            AffixKind desiredAffix = UnityEngine.Random.value < 0.5f ? AffixKind.Prefix : AffixKind.Suffix;

            for (int i = 0; i < tier; i++)
            {
                // 5.a) Elegimos el TIER del extra con la distribución 'chances'
                int enchantTier = TierChances.GetRandomTier(limitedChances);

                // 5.b) Elegimos un enchant ponderado por 'value' dentro del pool filtrado
                //      (tier, 1H/2H/Double, melee/ranged, affix) y evitando IDs ya usados.
                var pick = EnchantPicker.PickWeighted(
                    enchantTier,
                    weaponGrip,
                    isRanged,
                    desiredAffix,
                    usedIds
                );

                if (pick.enchant == null)
                {
                    UnityEngine.Debug.Log($"[RRE] Pool vacío en iter {i + 1}/{tier} (T{enchantTier}, {weaponGrip}, {(isRanged ? "Ranged" : "Melee")}, {desiredAffix}).");
                    break;
                }

                string appliedId = pick.enchant.AssetGuid.ToString();
                if (HasEnchant(entity, pick.enchant) || usedIds.Contains(appliedId))
                {
                    // Reintenta esta iteración
                    i--;
                    usedIds.Add(appliedId);
                    continue;
                }

                // 5.c) Aplicamos (duplicando a la segunda cabeza si procede)
                AddEnchants(entity, pick.duplicateToSecond, pick.enchant);
                usedIds.Add(appliedId);

                UnityEngine.Debug.Log($"[RRE] [{i + 1}/{tier}] Extra {desiredAffix} T{enchantTier} aplicado " +
                                      $"({weaponGrip}, {(isRanged ? "Ranged" : "Melee")}, dupSecond={pick.duplicateToSecond}).");

                // 5.d) Alternamos afijo para la siguiente pasada
                desiredAffix = (desiredAffix == AffixKind.Prefix) ? AffixKind.Suffix : AffixKind.Prefix;
            }
        }

        // remapea el mismo efecto a otro tier del mismo “root”; si falla, devuelve el original
        private static BlueprintItemEnchantment RemapPickedEnchantToTier(BlueprintItemEnchantment picked, int targetTier)
        {
            if (picked == null) return null;
            if (targetTier < 1) targetTier = 1;
            if (targetTier > 6) targetTier = 6;

            string pickedId = picked.AssetGuid.ToString();

            foreach (var d in EnchantList.Item)
            {
                if (d.AssetIDT1 != null && Array.IndexOf(d.AssetIDT1, pickedId) >= 0) return LoadTier(d, targetTier);
                if (d.AssetIDT2 != null && Array.IndexOf(d.AssetIDT2, pickedId) >= 0) return LoadTier(d, targetTier);
                if (d.AssetIDT3 != null && Array.IndexOf(d.AssetIDT3, pickedId) >= 0) return LoadTier(d, targetTier);
                if (d.AssetIDT4 != null && Array.IndexOf(d.AssetIDT4, pickedId) >= 0) return LoadTier(d, targetTier);
                if (d.AssetIDT5 != null && Array.IndexOf(d.AssetIDT5, pickedId) >= 0) return LoadTier(d, targetTier);
                if (d.AssetIDT6 != null && Array.IndexOf(d.AssetIDT6, pickedId) >= 0) return LoadTier(d, targetTier);
            }
            return picked;

            static BlueprintItemEnchantment LoadTier(EnchantData data, int targetTier)
            {
                string[] arr = targetTier switch
                {
                    1 => data.AssetIDT1,
                    2 => data.AssetIDT2,
                    3 => data.AssetIDT3,
                    4 => data.AssetIDT4,
                    5 => data.AssetIDT5,
                    6 => data.AssetIDT6,
                    _ => null
                };
                if (arr == null || arr.Length == 0) return null;

                var id = arr[0];
                return LootUtils.TryLoadEnchant(id);
            }
        }

    }
}
