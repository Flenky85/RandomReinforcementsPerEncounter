using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.View.MapObjects;
using RandomReinforcementsPerEncounter;
using RandomReinforcementsPerEncounter.itemlist.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private static WeaponType RollWeaponType20()
    {
        float r = UnityEngine.Random.value;
        if (r < 0.20f) return WeaponType.OneHandedMelee;
        if (r < 0.40f) return WeaponType.TwoHandedMelee;
        if (r < 0.60f) return WeaponType.OneHandedRanged;
        if (r < 0.80f) return WeaponType.TwoHandedRanged;
        return WeaponType.Double;
    }

    /// <summary>
    /// Devuelve un arma aleatoria con 20% por categoría de WeaponType.
    /// Por ahora 'chance' no se usa (lo reservamos para el siguiente paso).
    /// </summary>
    private static WeaponPick PickRandomWeapon(float oversizedChance = 0.15f)
    {
        bool wantOver = UnityEngine.Random.value < oversizedChance;
        var wanted = RollWeaponType20();

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
            wanted = RollWeaponType20();
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
    
    private static readonly BlueprintGuid Druchite = BlueprintGuid.Parse("e6a7a2b6f26b488783c612add1e9a8bd");
    private static readonly BlueprintGuid ColdIron = BlueprintGuid.Parse("e5990dc76d2a613409916071c898eee8");
    private static readonly BlueprintGuid Mithral = BlueprintGuid.Parse("0ae8fc9f2e255584faf4d14835224875");
    private static readonly BlueprintGuid Adamantine = BlueprintGuid.Parse("ab39e7d59dd12f4429ffef5dca88dc7b");

    //Enhancement +1
    private static readonly BlueprintGuid Shield1 = BlueprintGuid.Parse("e90c252e08035294eba39bafce76c119");
    private static readonly BlueprintGuid Weapon1 = BlueprintGuid.Parse("d42fc23b92c640846ac137dc26e000d4");
    private static readonly BlueprintGuid Armor1 = BlueprintGuid.Parse("a9ea95c5e02f9b7468447bc1010fe152");
    //Enhancement +2
    private static readonly BlueprintGuid Shield2 = BlueprintGuid.Parse("7b9f2f78a83577d49927c78be0f7fbc1");
    private static readonly BlueprintGuid Weapon2 = BlueprintGuid.Parse("eb2faccc4c9487d43b3575d7e77ff3f5");
    private static readonly BlueprintGuid Armor2 = BlueprintGuid.Parse("758b77a97640fd747abf149f5bf538d0");
    //Enhancement +3
    private static readonly BlueprintGuid Shield3 = BlueprintGuid.Parse("ac2e3a582b5faa74aab66e0a31c935a9");
    private static readonly BlueprintGuid Weapon3 = BlueprintGuid.Parse("80bb8a737579e35498177e1e3c75899b");
    private static readonly BlueprintGuid Armor3 = BlueprintGuid.Parse("9448d3026111d6d49b31fc85e7f3745a");
    //Enhancement +4
    private static readonly BlueprintGuid Shield4 = BlueprintGuid.Parse("a5d27d73859bd19469a6dde3b49750ff");
    private static readonly BlueprintGuid Weapon4 = BlueprintGuid.Parse("783d7d496da6ac44f9511011fc5f1979");
    private static readonly BlueprintGuid Armor4 = BlueprintGuid.Parse("eaeb89df5be2b784c96181552414ae5a");
    //Enhancement +5
    private static readonly BlueprintGuid Shield5 = BlueprintGuid.Parse("84d191a748edef84ba30c13b8ab83bd9");
    private static readonly BlueprintGuid Weapon5 = BlueprintGuid.Parse("bdba267e951851449af552aa9f9e3992");
    private static readonly BlueprintGuid Armor5 = BlueprintGuid.Parse("6628f9d77fd07b54c911cd8930c0d531");
    //Enhancement +6
    private static readonly BlueprintGuid Shield6 = BlueprintGuid.Parse("70c26c66adb96d74baec38fc8d20c139");
    private static readonly BlueprintGuid Weapon6 = BlueprintGuid.Parse("0326d02d2e24d254a9ef626cc7a3850f");
    private static readonly BlueprintGuid Armor6 = BlueprintGuid.Parse("de15272d1f4eb7244aa3af47dbb754ef");

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
                ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(matGuid),
                PriceRefs.PriceT0
            );

        }

        if (UnityEngine.Random.value < 0.20f)
        {
            int[] chances = CalcTierChances(cr);
            for (int i = 0; i < chances.Length; i++)
                Debug.Log($"Tier {i + 1}: {chances[i]}");
           
            int tier = GetRandomTier(cr);

            ApplyRandomTierEnchant(entity, tier, chances);

            /*
            switch (tier)
            {
                case 1:
                    AddEnchants(
                        entity,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Weapon1),
                        PriceRefs.PriceT1
                    );
                    ApplyRandomTierEnchant(entity, tier);
                    break;
                case 2:
                    AddEnchants(
                        entity,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Weapon2),
                        PriceRefs.PriceT2
                    );
                    ApplyRandomTierEnchant(entity, 2);
                    break;
                case 3:
                    AddEnchants(
                        entity,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Weapon3),
                        PriceRefs.PriceT3
                    );
                    ApplyRandomTierEnchant(entity, 3);
                    break;
                case 4:
                    AddEnchants(
                        entity,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Weapon4),
                        PriceRefs.PriceT4
                    );
                    ApplyRandomTierEnchant(entity, 4);
                    break;
                case 5:
                    AddEnchants(
                        entity,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Weapon5),
                        PriceRefs.PriceT5
                    );
                    ApplyRandomTierEnchant(entity, 5);
                    break;
                case 6:
                    AddEnchants(
                        entity,
                        ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Weapon6),
                        PriceRefs.PriceT6
                    );
                    ApplyRandomTierEnchant(entity, 6);
                    break;
                default:
                    // sin tier válido, no aplicar nada
                    break;
            }*/
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
    public static int GetRandomTier(int cr)
    {
        int[] chances = CalcTierChances(cr);
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
        switch (tier)
        {
            case 1: return 1; // T1 -> T1
            case 2: return 1; // T2 -> T1
            case 3: return 2; // T3 -> T2
            case 4: return 2; // T4 -> T2
            case 5: return 3; // T5 -> T3
            case 6: return 4; // T6 -> T4
            default:
                // por si algún día pasa de 6: clamp a 1..6 y reusa la lógica
                return tier < 1 ? 1 : (tier > 6 ? 4 : tier);
        }
    }
    // Llamada cómoda: añade N encantamientos por clave
    public static void AddEnchants(ItemEntity entity, params BlueprintItemEnchantment[] enchants)
    {
        if (entity == null || enchants == null || enchants.Length == 0) return;

        var ctx = RRECtx.Permanent();
        foreach (var e in enchants)
        {
            if (e != null)
                entity.AddEnchantment(e, ctx);
        }
    }
    private static BlueprintItemEnchantment PriceForTier(int tier)
    {
        switch (tier)
        {
            case 1: return PriceRefs.PriceT1;
            case 2: return PriceRefs.PriceT2;
            case 3: return PriceRefs.PriceT3;
            case 4: return PriceRefs.PriceT4;
            case 5: return PriceRefs.PriceT5;
            case 6: return PriceRefs.PriceT6;
            default: return PriceRefs.PriceT0;
        }
    }
    private static BlueprintGuid GetWeaponEnchantIdForTier(int tier)
    {
        // Clamp manual para evitar Math.Clamp (compatibilidad C# 7.3)
        if (tier < 1) tier = 1;
        if (tier > 6) tier = 6;

        switch (tier)
        {
            case 1: return Weapon1;
            case 2: return Weapon2;
            case 3: return Weapon3;
            case 4: return Weapon4;
            case 5: return Weapon5;
            case 6: return Weapon6;
        }
        return Weapon1; // fallback
    }
    /// <summary>
    /// Elige un enchant del Tier indicado (ponderado por Value) y lo aplica,
    /// junto con el price correspondiente a ese Tier.
    /// </summary>
    private static bool HasEnchant(ItemEntity item, BlueprintItemEnchantment ench)
    => item?.Enchantments?.Any(e => e?.Blueprint == ench) == true;

    public static void ApplyRandomTierEnchant(ItemEntityWeapon entity, int tier, int[] chances)
    {
        AddEnchants(
            entity,
            ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(GetWeaponEnchantIdForTier(tier)), // auto por tier
            PriceForTier(tier) // auto por tier
        );

        // Tipo de arma
        WeaponTypeEnchant weaponType =
            (entity?.Blueprint?.SecondWeapon != null) ? WeaponTypeEnchant.Double :
            (entity?.Blueprint?.IsTwoHanded == true) ? WeaponTypeEnchant.TwoHanded :
            WeaponTypeEnchant.OneHanded;

        var usedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < tier; i++)
        {
            // Elegir del pool SIN interferir y excluyendo ya usados
            var extra = PickRandomEnchantByTier(tier, weaponType, out EnchantType pickedType, usedIds);

            if (extra == null)
            {
                Debug.Log($"[RRE] Pool vacío en iter {i + 1}/{tier} para T{tier} ({weaponType}).");
                break;
            }

            // Tier efectivo (post-selección)
            int effectiveTier = tier;
            bool applyDiscount =
                (weaponType == WeaponTypeEnchant.OneHanded) ||
                (weaponType == WeaponTypeEnchant.Double &&
                 (pickedType == EnchantType.OnHit ||
                  pickedType == EnchantType.OnlyOnFirstHit ||
                  pickedType == EnchantType.EnergyDamage));

            if (applyDiscount)
                effectiveTier = MapTierWithDiscount(tier);

            var appliedExtra = applyDiscount ? RemapPickedEnchantToTier(extra, effectiveTier) : extra;
            if (appliedExtra == null) appliedExtra = extra; // fallback por seguridad

            string appliedId = appliedExtra.AssetGuid.ToString();

            // Revalidar duplicados tras remapear (por si ya existía la versión de tier bajo)
            if (HasEnchant(entity, appliedExtra) || usedIds.Contains(appliedId))
            {
                usedIds.Add(appliedId);
                // también excluimos el original para no volver a caer en el mismo camino
                usedIds.Add(extra.AssetGuid.ToString());
                i--; // reintenta esta iteración
                continue;
            }

            string extraId = extra.AssetGuid.ToString();
            AddEnchants(entity, appliedExtra, PriceForTier(effectiveTier));
            usedIds.Add(extraId);

            Debug.Log($"[RRE] [{i + 1}/{tier}] Extra enchant T{tier} → T{effectiveTier} aplicado ({weaponType}, {pickedType}): {extra?.name}");

            // Doble: duplica en la segunda cabeza si procede y existe la instancia
            if (weaponType == WeaponTypeEnchant.Double && applyDiscount)
            {
                var second = entity.Second;
                if (second != null && !HasEnchant(second, extra))
                {
                    AddEnchants(second, extra, PriceForTier(effectiveTier));
                    Debug.Log("[RRE] Enchant duplicado en la segunda parte del arma doble (instancia).");
                }
            }
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


/*
 var entity = bpWeapon.CreateEntity() as ItemEntityWeapon;
var ctx = new MechanicsContext(default(Kingmaker.EntitySystem.Persistence.JsonUtility.JsonConstructorMark));

// Ejemplo: aplicar +160 gp
var price160 = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(
    GuidUtil.EnchantGuid("price_160"));
if (price160 != null) entity.AddEnchantment(price160, ctx);

// O apilar el mismo BP varias veces (p.ej. 4× +320 = +1280):
var price320 = ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(
    GuidUtil.EnchantGuid("price_320"));
for (int i = 0; i < 4; i++) entity.AddEnchantment(price320, ctx);

lootPart.Loot.Add(entity);
  */
