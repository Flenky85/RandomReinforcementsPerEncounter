using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter;
using RandomReinforcementsPerEncounter.Config.Ids;
using RandomReinforcementsPerEncounter.itemlist.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RandomReinforcementsPerEncounter.Domain.Models;

internal static class RRECtx
{
    // Devuelve un contexto “dummy” válido para marcar el enchant como externo/persistente
    public static MechanicsContext Permanent()
        => new MechanicsContext(default(JsonConstructorMark));
}
internal static class PriceRefs
{
    // añade los que uses
    public static BlueprintItemEnchantment PriceT0 => _p20 ??= Get("price_20");
    public static BlueprintItemEnchantment PriceT1 => _p40 ??= Get("price_40");
    public static BlueprintItemEnchantment PriceT2 => _p80 ??= Get("price_80");
    public static BlueprintItemEnchantment PriceT3 => _p160 ??= Get("price_160");
    public static BlueprintItemEnchantment PriceT4 => _p320 ??= Get("price_320");
    public static BlueprintItemEnchantment PriceT5 => _p640 ??= Get("price_640");
    public static BlueprintItemEnchantment PriceT6 => _p1280 ??= Get("price_1280");

    private static BlueprintItemEnchantment _p20, _p40, _p80, _p160, _p320, _p640, _p1280;

    private static BlueprintItemEnchantment Get(string key)
        => ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(GuidUtil.EnchantGuid(key));
}

public static class LootPicker
{
    private struct WeaponPick
    {
        public string Name;
        public string AssetId;
        public WeaponType Type;
        public WeaponFocusMod Focus;
        public bool IsOversized;
    }

    // 20% por tipo: O1H, O2H, R1H, R2H, Double (tu helper existente)
    private static WeaponType RollWeaponType()
    {
        int wOHM = 30; // OneHandedMelee
        int wTHM = 30; // TwoHandedMelee
        int wOHR = 15; // OneHandedRanged
        int wTHR = 15; // TwoHandedRanged
        int wDBL = 10; // Double

        int total = wOHM + wTHM + wOHR + wTHR + wDBL;
        int roll = UnityEngine.Random.Range(1, total + 1); // [1..total]

        if (roll <= wOHM) return WeaponType.OneHandedMelee;
        roll -= wOHM;
        if (roll <= wTHM) return WeaponType.TwoHandedMelee;
        roll -= wTHM;
        if (roll <= wOHR) return WeaponType.OneHandedRanged;
        roll -= wOHR;
        if (roll <= wTHR) return WeaponType.TwoHandedRanged;
        return WeaponType.Double;
    }

    /// <summary>
    /// Devuelve un arma aleatoria con 20% por categoría de WeaponType.
    /// Por ahora 'chance' no se usa (lo reservamos para el siguiente paso).
    /// </summary>
    private static WeaponPick PickRandomWeapon(float oversizedChance = 0.15f)
    {
        bool wantOver = UnityEngine.Random.value < oversizedChance;
        var wanted = RollWeaponType();

        // Intenta hasta 5 re-rolls de tipo si el bucket sale vacío
        for (int tries = 0; tries < 5; tries++)
        {
            if (wantOver)
            {
                var bucket = weaponListOversized.Item.Where(w => w.Type == wanted).ToList();
                if (bucket.Count > 0)
                {
                    int idx = UnityEngine.Random.Range(0, bucket.Count);
                    var it = bucket[idx];
                    return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = true };
                }
            }
            else
            {
                var bucket = weaponList.Item.Where(w => w.Type == wanted).ToList();
                if (bucket.Count > 0)
                {
                    int idx = UnityEngine.Random.Range(0, bucket.Count);
                    var it = bucket[idx];
                    return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = false };
                }
            }

            // Reintenta con otro tipo si el bucket estaba vacío
            wanted = RollWeaponType();
        }

        // Fallback: cualquier arma de la lista elegida
        if (wantOver && weaponListOversized.Item.Count > 0)
        {
            int anyIdx = UnityEngine.Random.Range(0, weaponListOversized.Item.Count);
            var it = weaponListOversized.Item[anyIdx];
            return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = true };
        }
        if (!wantOver && weaponList.Item.Count > 0)
        {
            int anyIdx = UnityEngine.Random.Range(0, weaponList.Item.Count);
            var it = weaponList.Item[anyIdx];
            return new WeaponPick { Name = it.Name, AssetId = it.AssetId, Type = it.Type, Focus = it.Focus, IsOversized = false };
        }

        // Si no hay nada, devolvemos default (AssetId nulo) y que el caller corte
        return default;
    }
    
    private static readonly BlueprintGuid Druchite = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Druchite);
    private static readonly BlueprintGuid ColdIron = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.ColdIron);
    private static readonly BlueprintGuid Mithral = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Mithral);
    private static readonly BlueprintGuid Adamantine = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Adamantine);

    private static readonly BlueprintGuid MasterWork = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.MasterWork);

    private static readonly BlueprintGuid Composite = BlueprintGuid.Parse(BlueprintGuids.ItemQuality.Composite);

    //Enhancement +1
    private static readonly BlueprintGuid Shield1 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ShieldPlus1);
    private static readonly BlueprintGuid Weapon1 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus1);
    private static readonly BlueprintGuid Armor1 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ArmorPlus1);
    //Enhancement +2
    private static readonly BlueprintGuid Shield2 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ShieldPlus2);
    private static readonly BlueprintGuid Weapon2 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus2);
    private static readonly BlueprintGuid Armor2 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ArmorPlus2);
    //Enhancement +3
    private static readonly BlueprintGuid Shield3 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ShieldPlus3);
    private static readonly BlueprintGuid Weapon3 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus3);
    private static readonly BlueprintGuid Armor3 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ArmorPlus3);
    //Enhancement +4
    private static readonly BlueprintGuid Shield4 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ShieldPlus4);
    private static readonly BlueprintGuid Weapon4 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus4);
    private static readonly BlueprintGuid Armor4 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ArmorPlus4);
    //Enhancement +5
    private static readonly BlueprintGuid Shield5 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ShieldPlus5);
    private static readonly BlueprintGuid Weapon5 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus5);
    private static readonly BlueprintGuid Armor5 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ArmorPlus5);
    //Enhancement +6
    private static readonly BlueprintGuid Shield6 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ShieldPlus6);
    private static readonly BlueprintGuid Weapon6 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.WeaponPlus6);
    private static readonly BlueprintGuid Armor6 = BlueprintGuid.Parse(BlueprintGuids.EnhancementPlus.ArmorPlus6);

    public static void AddPickedWeaponToLoot(InteractionLootPart lootPart, int cr)
    {

        var pick = PickRandomWeapon(0.15f); // 15% oversized
        if (string.IsNullOrEmpty(pick.AssetId)) return;

        var bp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(pick.AssetId);
        var bpWeapon = bp as BlueprintItemWeapon;
        if (bpWeapon == null) return;

        var entity = bpWeapon.CreateEntity() as ItemEntityWeapon;
        if (entity == null) return;
        entity.Identify();

        if (UnityEngine.Random.value < 0.20f)
        {
            float r = UnityEngine.Random.value;
            var matGuid =
                (r < 0.50f) ? ColdIron :
                (r < 0.80f) ? Mithral :
                (r < 0.90f) ? Adamantine :
                              Druchite;

            AddEnchants(
                entity,
                true,
                ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(matGuid),
                PriceRefs.PriceT0
            );

        }

        if (bpWeapon.Category == WeaponCategory.Longbow || bpWeapon.Category == WeaponCategory.Shortbow)
        {
            if (UnityEngine.Random.value < 0.50f) 
            {
                AddEnchants(
                    entity,
                    true,
                    ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Composite),
                    PriceRefs.PriceT0
                );
            }
        }

        if (UnityEngine.Random.value < 0.20f)
        {
            int[] chances = CalcTierChances(cr);
            for (int i = 0; i < chances.Length; i++)
                Debug.Log($"Tier {i + 1}: {chances[i]}");
           
            int tier = GetRandomTier(chances);

            ApplyRandomTierEnchant(entity, tier, chances);
        }

        if (bpWeapon.IsMagic == false)
        {
            if (UnityEngine.Random.value < 0.30f) 
            {
                var masterworkEnchant = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(MasterWork);
                if (masterworkEnchant != null)
                {
                    AddEnchants(
                        entity, 
                        true, 
                        masterworkEnchant,
                        PriceRefs.PriceT0
                    );
                    Debug.Log("[RRE] Masterwork enchant aplicado (arma no mágica).");
                }
            }
        }

        lootPart.Loot.Add(entity); // mete la instancia en el cofre
    }
    public static int[] CalcTierChances(int cr)
    {
        int[] initial = { 52, 40, 30, 22, 16, 10 };
        int[] inc = { 6, 5, 4, 3, 2, 1 };
        int[] shift = { 1, 6, 11, 15, 18, 21 }; // CR donde el tier pasa a ser initial

        int[] tiers = new int[6];
        for (int i = 0; i < 6; i++)
        {
            if (cr < shift[i]) { tiers[i] = 0; continue; }

            int raw = initial[i] + inc[i] * (cr - shift[i]);

            // Tiers 1..3: al alcanzar 80, decaen 4 por CR a partir del CR de tope
            if (i <= 2 && raw > 80)
            {
                int capCR = shift[i] + ((80 - initial[i] + inc[i] - 1) / inc[i]); // ceil entero
                int dec = 4 * (cr - capCR);
                tiers[i] = Math.Max(0, 80 - dec);
            }
            else
            {
                tiers[i] = raw;
            }
        }
        return tiers;
    }
    public static int GetRandomTier(int[] chances)
    {
        int total = chances.Sum();
        if (total <= 0) return 0; // 0 = sin tier válido

        int roll = UnityEngine.Random.Range(0, total);
        int acc = 0;

        for (int i = 0; i < chances.Length; i++)
        {
            acc += chances[i];
            if (roll < acc)
            {
                return i + 1; // Tiers empiezan en 1
            }
        }

        return 0; // por seguridad, no debería llegar aquí
    }
    private static string[] GetTierArray(EnchantData d, int tier)
    {
        switch (tier)
        {
            case 1: return d.AssetIDT1;
            case 2: return d.AssetIDT2;
            case 3: return d.AssetIDT3;
            case 4: return d.AssetIDT4;
            case 5: return d.AssetIDT5;
            case 6: return d.AssetIDT6;
            default: return null;
        }
    }

    /// <summary>
    /// Devuelve un BlueprintItemEnchantment aleatorio del Tier indicado,
    /// ponderado por EnchantData.Value. Si no hay nada, devuelve null.
    /// </summary>
    public static BlueprintItemEnchantment PickRandomEnchantByTier(
        int tier,
        WeaponTypeEnchant weaponType,
        out EnchantType pickedType)
    {
        pickedType = EnchantType.Others;

        var pool = new List<(string id, int weight)>();
        foreach (var d in EnchantList.Item)
        {
            if (d == null || d.Value <= 0) continue;
            var arr = GetTierArray(d, tier);
            if (arr == null || arr.Length == 0) continue;

            foreach (var id in arr)
            {
                if (!string.IsNullOrEmpty(id))
                    pool.Add((id, d.Value));
            }
        }

        int total = pool.Sum(p => p.weight);
        if (total <= 0) return null;

        int roll = UnityEngine.Random.Range(0, total);
        int acc = 0;
        string pickedId = null;

        foreach (var p in pool)
        {
            acc += p.weight;
            if (roll < acc) { pickedId = p.id; break; }
        }

        if (string.IsNullOrEmpty(pickedId)) return null;

        pickedType = GetEnchantTypeByAssetId(pickedId);

        try
        {
            var guid = BlueprintGuid.Parse(pickedId);
            return ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(guid);
        }
        catch
        {
            return null;
        }
    }

    private static int MapTierWithDiscount(int tier)
    {
        return tier switch
        {
            1 => 1,// T1 -> T1
            2 => 1,// T2 -> T1
            3 => 2,// T3 -> T2
            4 => 2,// T4 -> T2
            5 => 3,// T5 -> T3
            6 => 4,// T6 -> T4
            _ => tier < 1 ? 1 : (tier > 6 ? 4 : tier),// por si algún día pasa de 6: clamp a 1..6 y reusa la lógica
        };
    }
    /// <summary>
    /// Aplica los mismos encantamientos al entity principal y, si se pasa, al secondEntity.
    /// Evita duplicados en el second con HasEnchant (si lo tienes disponible).
    /// </summary>
    public static void AddEnchants(ItemEntityWeapon entity, bool applyToSecond, params BlueprintItemEnchantment[] enchants)
    {
        if (entity == null || enchants == null || enchants.Length == 0) return;

        var ctx = RRECtx.Permanent();

        // Principal
        foreach (var e in enchants)
        {
            if (e != null)
                entity.AddEnchantment(e, ctx);
        }

        // Secundario (si existe y está permitido)
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
    private static BlueprintItemEnchantment PriceForTier(int tier)
    {
        return tier switch
        {
            1 => PriceRefs.PriceT1,
            2 => PriceRefs.PriceT2,
            3 => PriceRefs.PriceT3,
            4 => PriceRefs.PriceT4,
            5 => PriceRefs.PriceT5,
            6 => PriceRefs.PriceT6,
            _ => PriceRefs.PriceT0,
        };
    }
    private static BlueprintGuid GetWeaponEnchantIdForTier(int tier)
    {

        return tier switch
        {
            1 => Weapon1,
            2 => Weapon2,
            3 => Weapon3,
            4 => Weapon4,
            5 => Weapon5,
            6 => Weapon6,
            _ => Weapon1
        };

    }
    /// <summary>
    /// Elige un enchant del Tier indicado (ponderado por Value) y lo aplica,
    /// junto con el price correspondiente a ese Tier.
    /// </summary>
    private static bool HasEnchant(ItemEntity item, BlueprintItemEnchantment ench)
    => item?.Enchantments?.Any(e => e?.Blueprint == ench) == true;

    public static void ApplyRandomTierEnchant(ItemEntityWeapon entity, int tier, int[] chances)
    {
        var baseEnchant = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(GetWeaponEnchantIdForTier(tier));
        AddEnchants(entity, true, baseEnchant, PriceForTier(tier)); 

        // 2) Tipo de arma
        WeaponTypeEnchant weaponType =
            (entity?.Blueprint?.SecondWeapon != null) ? WeaponTypeEnchant.Double :
            (entity?.Blueprint?.IsTwoHanded == true) ? WeaponTypeEnchant.TwoHanded :
            WeaponTypeEnchant.OneHanded;

        for (int i = 0; i < tier; i++)
        {
            var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int[] limitedChances = chances.Take(tier).ToArray();
            int enchantTier = GetRandomTier(limitedChances);

            var extra = PickRandomEnchantByTier(enchantTier, weaponType, out EnchantType pickedType, usedIds);
            if (extra == null)
            {
                Debug.Log($"[RRE] Pool vacío en iter {i + 1}/{tier} para T{tier} ({weaponType}).");
                break;
            }

            // 3) Descuento de tier (igual que antes)
            int effectiveTier = enchantTier;
            bool applyDiscount =
                (weaponType == WeaponTypeEnchant.OneHanded) ||
                (weaponType == WeaponTypeEnchant.Double &&
                 (pickedType == EnchantType.OnHit || pickedType == EnchantType.OnlyOnFirstHit || pickedType == EnchantType.EnergyDamage));

            if (applyDiscount)
                effectiveTier = MapTierWithDiscount(enchantTier);

            var mainExtra = applyDiscount ? RemapPickedEnchantToTier(extra, effectiveTier) : extra;
            if (mainExtra == null) mainExtra = extra;

            string appliedId = mainExtra.AssetGuid.ToString();

            if (HasEnchant(entity, mainExtra) || usedIds.Contains(appliedId))
            {
                usedIds.Add(appliedId);
                usedIds.Add(extra.AssetGuid.ToString());
                i--;
                continue;
            }

            // 4) ¿Duplicar a la segunda cabeza para este EXTRA?
            bool duplicateToSecond =
                (weaponType == WeaponTypeEnchant.Double) &&
                (pickedType == EnchantType.OnHit || pickedType == EnchantType.OnlyOnFirstHit || pickedType == EnchantType.EnergyDamage);

            // 5) Aplicar el extra con o sin duplicación (SOLO aquí controlamos el false)
            AddEnchants(entity, duplicateToSecond, mainExtra);

            usedIds.Add(extra.AssetGuid.ToString());
            usedIds.Add(appliedId);

            Debug.Log($"[RRE] [{i + 1}/{tier}] Extra enchant T{tier} → T{effectiveTier} aplicado " +
                      $"({weaponType}, {pickedType}). DuplicadoSecond={duplicateToSecond}");
        }
    }
    // Devuelve el mismo efecto pero en el tier solicitado (si existe). Si no lo encuentra, devuelve el original.
    private static BlueprintItemEnchantment RemapPickedEnchantToTier(BlueprintItemEnchantment picked, int targetTier)
    {
        if (picked == null) return null;
        if (targetTier < 1) targetTier = 1;
        if (targetTier > 6) targetTier = 6;

        string pickedId = picked.AssetGuid.ToString();

        foreach (var d in EnchantList.Item)
        {
            // Buscar en qué root/tier está el id original
            if (d.AssetIDT1 != null && Array.IndexOf(d.AssetIDT1, pickedId) >= 0) return LoadTier(d, 1, targetTier);
            if (d.AssetIDT2 != null && Array.IndexOf(d.AssetIDT2, pickedId) >= 0) return LoadTier(d, 2, targetTier);
            if (d.AssetIDT3 != null && Array.IndexOf(d.AssetIDT3, pickedId) >= 0) return LoadTier(d, 3, targetTier);
            if (d.AssetIDT4 != null && Array.IndexOf(d.AssetIDT4, pickedId) >= 0) return LoadTier(d, 4, targetTier);
            if (d.AssetIDT5 != null && Array.IndexOf(d.AssetIDT5, pickedId) >= 0) return LoadTier(d, 5, targetTier);
            if (d.AssetIDT6 != null && Array.IndexOf(d.AssetIDT6, pickedId) >= 0) return LoadTier(d, 6, targetTier);
        }

        // No se encontró el root: devolver original
        return picked;

        // local: carga cualquier id válido del tier objetivo
        static BlueprintItemEnchantment LoadTier(EnchantData data, int originalTier, int targetTier)
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

            // Opción determinista: coge el primero. (Si prefieres aleatorio, elige uno al azar.)
            var id = arr[0];
            try
            {
                var guid = BlueprintGuid.Parse(id);
                return ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(guid);
            }
            catch { return null; }
        }
    }

    public static BlueprintItemEnchantment PickRandomEnchantByTier(
        int tier,
        WeaponTypeEnchant weaponType,
        out EnchantType pickedType,
        HashSet<string> excludeIds // NUEVO
    )
    {
        pickedType = EnchantType.Others;
        var pool = new List<(string id, int weight)>();

        foreach (var d in EnchantList.Item)
        {
            if (d == null || d.Value <= 0) continue;
            var arr = GetTierArray(d, tier);
            if (arr == null || arr.Length == 0) continue;

            foreach (var id in arr)
            {
                if (!string.IsNullOrEmpty(id) && (excludeIds == null || !excludeIds.Contains(id)))
                    pool.Add((id, d.Value));
            }
        }

        int total = pool.Sum(p => p.weight);
        if (total <= 0) return null;

        int roll = UnityEngine.Random.Range(0, total);
        int acc = 0;
        string pickedId = null;

        foreach (var p in pool)
        {
            acc += p.weight;
            if (roll < acc) { pickedId = p.id; break; }
        }
        if (string.IsNullOrEmpty(pickedId)) return null;

        pickedType = GetEnchantTypeByAssetId(pickedId);

        try
        {
            var guid = BlueprintGuid.Parse(pickedId);
            return ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(guid);
        }
        catch { return null; }
    }


    public enum WeaponTypeEnchant
    {
        OneHanded,
        TwoHanded,
        Double
    }

    private static EnchantType GetEnchantTypeByAssetId(string assetId)
    {
        foreach (var d in EnchantList.Item)
        {
            if (d.AssetIDT1 != null && Array.IndexOf(d.AssetIDT1, assetId) >= 0) return d.Type;
            if (d.AssetIDT2 != null && Array.IndexOf(d.AssetIDT2, assetId) >= 0) return d.Type;
            if (d.AssetIDT3 != null && Array.IndexOf(d.AssetIDT3, assetId) >= 0) return d.Type;
            if (d.AssetIDT4 != null && Array.IndexOf(d.AssetIDT4, assetId) >= 0) return d.Type;
            if (d.AssetIDT5 != null && Array.IndexOf(d.AssetIDT5, assetId) >= 0) return d.Type;
            if (d.AssetIDT6 != null && Array.IndexOf(d.AssetIDT6, assetId) >= 0) return d.Type;
        }
        return EnchantType.Others;
    }

}