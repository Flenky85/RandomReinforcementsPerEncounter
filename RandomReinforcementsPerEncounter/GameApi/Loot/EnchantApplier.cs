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
    }
}
