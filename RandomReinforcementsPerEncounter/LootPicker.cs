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
    public static BlueprintItemEnchantment Price20 => _p20 ??= Get("price_20");
    public static BlueprintItemEnchantment Price40 => _p40 ??= Get("price_40");
    public static BlueprintItemEnchantment Price80 => _p80 ??= Get("price_80");
    public static BlueprintItemEnchantment Price160 => _p160 ??= Get("price_160");
    public static BlueprintItemEnchantment Price320 => _p320 ??= Get("price_320");
    public static BlueprintItemEnchantment Price640 => _p640 ??= Get("price_640");
    public static BlueprintItemEnchantment Price1280 => _p1280 ??= Get("price_1280");

    private static BlueprintItemEnchantment _p20, _p40, _p80, _p160, _p320, _p640, _p1280;

    private static BlueprintItemEnchantment Get(string key)
        => ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(GuidUtil.EnchantGuid(key));
}

public static class LootPicker
{
    // 20% por tipo: O1H, O2H, R1H, R2H, Double
    private static WeaponType RollWeaponType20()
    {
        float r = Random.value; // [0,1)
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
    public static WeaponLootData RandomWeapon()
    {

        var wanted = RollWeaponType20();

        var bucket = weaponList.Item.Where(w => w.Type == wanted).ToList();

        for (int i = 0; i < 5 && bucket.Count == 0; i++)
        {
            wanted = RollWeaponType20();
            bucket = weaponList.Item.Where(w => w.Type == wanted).ToList();
        }

        if (bucket.Count == 0)
        {
            if (weaponList.Item.Count == 0) return null;
            int anyIdx = Random.Range(0, weaponList.Item.Count);
            return weaponList.Item[anyIdx];
        }

        int idx = Random.Range(0, bucket.Count);
        return bucket[idx];
    }

    private static readonly BlueprintGuid SawtoothSabreGuid = BlueprintGuid.Parse("45e76b1480d14d959d780f1182a6cabf");
    private static readonly BlueprintGuid SabreEnchantToStrip = BlueprintGuid.Parse("e5990dc76d2a613409916071c898eee8");
    private static readonly BlueprintGuid Oversized = BlueprintGuid.Parse("d8e1ebc1062d8cc42abff78783856b0d");
    private static readonly BlueprintGuid Druchite = BlueprintGuid.Parse("e6a7a2b6f26b488783c612add1e9a8bd");
    private static readonly BlueprintGuid ColdIron = BlueprintGuid.Parse("e5990dc76d2a613409916071c898eee8");
    private static readonly BlueprintGuid Mithral = BlueprintGuid.Parse("0ae8fc9f2e255584faf4d14835224875");
    private static readonly BlueprintGuid Adamantine = BlueprintGuid.Parse("ab39e7d59dd12f4429ffef5dca88dc7b");
    public static void AddPickedWeaponToLoot(InteractionLootPart lootPart, WeaponLootData picked, int cr )
    {
        if (picked == null) return;

        var bp = ResourcesLibrary.TryGetBlueprint<BlueprintItem>(picked.AssetId);
        var bpWeapon = bp as BlueprintItemWeapon;
        if (bpWeapon == null) return;

        var entity = bpWeapon.CreateEntity() as ItemEntityWeapon;
        if (entity == null) return;

        // EXCEPCIÓN: si es el Sawtooth Sabre “especial”, quitar su enchant por defecto
        if (bpWeapon.AssetGuid == SawtoothSabreGuid)
        {
            var toRemove = entity.Enchantments?
                .FirstOrDefault(e => (e?.Blueprint as BlueprintWeaponEnchantment)?.AssetGuid == SabreEnchantToStrip);
            if (toRemove != null)
                entity.RemoveEnchantment(toRemove);
                entity.AddEnchantment( PriceRefs.Price20, RRECtx.Permanent());
        }

        if (UnityEngine.Random.value < 0.10f)
        {
            entity.AddEnchantment(ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(Oversized), RRECtx.Permanent());
        }
        if (UnityEngine.Random.value < 0.20f)
        {
            float r = UnityEngine.Random.value;
            var matGuid =
                (r < 0.50f) ? ColdIron :
                (r < 0.80f) ? Mithral :
                (r < 0.90f) ? Adamantine :
                              Druchite;

            entity.AddEnchantment(
                ResourcesLibrary.TryGetBlueprint<BlueprintItemEnchantment>(matGuid),
                RRECtx.Permanent());
        }
        if (UnityEngine.Random.value < 0.20f)
        {
            int[] chances = CalcTierChances(cr);
            for (int i = 0; i < chances.Length; i++)
                Debug.Log($"Tier {i + 1}: {chances[i]}");

        }





            lootPart.Loot.Add(entity); // mete la instancia en el cofre
    }
    public static int[] CalcTierChances(int cr)
    {
        int[] inc = { 6, 5, 4, 3, 2, 1 };
        int[] shift = { 0, 5, 9, 13, 16, 19 }; // CR mínimo a partir del cual empieza cada tier

        int Clamp50(int v) => v < 0 ? 0 : (v > 50 ? 50 : v);

        int[] tiers = new int[6];
        for (int i = 0; i < 6; i++)
        {
            int val = inc[i] * (cr - shift[i]);
            tiers[i] = Clamp50(val);
        }

        return tiers;
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
