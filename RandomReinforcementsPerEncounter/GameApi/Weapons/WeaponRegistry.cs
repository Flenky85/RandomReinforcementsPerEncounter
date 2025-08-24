using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.RuleSystem;
using RandomReinforcementsPerEncounter.Config.Ids;
using RandomReinforcementsPerEncounter.Config.Ids.Generators;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Content.WeaponCatalog;
using RandomReinforcementsPerEncounter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomReinforcementsPerEncounter.GameApi.Weapons
{
    internal static class WeaponRegistry
    {
        private static BlueprintItemWeapon _sawtoothStandard;
        private static readonly string ColdIronSawtoothSabreId = BlueprintGuids.ColdIronSawtoothSabreId;
        private static readonly BlueprintGuid SawtoothSabreGuid = IdGenerators.WeaponId(Seed.sawtoothSabre);
        private static readonly BlueprintGuid OversizedEnchantGuid = BlueprintGuid.Parse(BlueprintGuids.OverSized);

        public static BlueprintItemWeapon Create_SawtoothSabre_Standard()
        {
            if (_sawtoothStandard != null) return _sawtoothStandard;

            var origRef = BlueprintTool.GetRef<BlueprintItemWeaponReference>(ColdIronSawtoothSabreId);

            _sawtoothStandard = ItemWeaponConfigurator
              .New("RRE_StandardSawtoothSabre", SawtoothSabreGuid.ToString())
              .CopyFrom(origRef)     
              .ClearEnchantments()
              .SetDisplayNameText(LocalizationTool.CreateString("RRE_StandardSawtoothSabre_Name", "Sawtooth Sabre"))
              .Configure();

            _sawtoothStandard.m_Cost = 20;

            return _sawtoothStandard;
        }
        public static void BuildAllOversizedFromList()
        {
            foreach (var entry in WeaponCatalogOversized.Item)
            {
                try
                {
                    BuildOversized(entry);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[RRE] Oversized FAIL '{entry?.Name}': {ex}");
                }
            }
        }
        // --- Clona UNA arma oversized desde su descriptor de lista ---
        private static void BuildOversized(WeaponOverLootData data)
        {
            if (data == null) return;

            var newId = BlueprintGuid.Parse(data.AssetId);
            var exists = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(newId);
            if (exists != null)
            {
                UnityEngine.Debug.Log($"[RRE] Oversized already exists: {data.Name} ({newId})");
                return;
            }

            // 0) Resolver original y el enchant Oversized
            var origId = BlueprintGuid.Parse(data.Original);
            var origRef = BlueprintTool.GetRef<BlueprintItemWeaponReference>(origId.ToString());
            if (origRef == null || origRef.Get() == null)
                throw new InvalidOperationException($"Original not found: {data.Original} ({data.Name})");

            var overs = ResourcesLibrary.TryGetBlueprint<Kingmaker.Blueprints.Items.Ecnchantments.BlueprintWeaponEnchantment>(OversizedEnchantGuid);
            var overRef = overs?.ToReference<BlueprintWeaponEnchantmentReference>();

            // 1) Crear el clon limpio
            ItemWeaponConfigurator
                .New(data.Name, newId.ToString())
                .CopyFrom(origRef)
                .Configure();

            // 2) Subir el dado de daño del principal
            var clone = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(newId);
            if (clone == null)
                throw new InvalidOperationException($"Clone not found right after Configure(): {data.Name} ({newId})");

            var baseDamage = clone.m_OverrideDamageDice
                ? clone.m_DamageDice
                : clone.m_Type?.Get()?.m_BaseDamage ?? clone.m_DamageDice;

            var stepped = StepUp(baseDamage);
            clone.m_OverrideDamageDice = true;
            clone.m_DamageDice = stepped;
            

            // 3) Si es arma doble, clonar y subir el segundo extremo
            if (clone.Double && clone.m_SecondWeapon != null)
            {
                var secondOrig = clone.m_SecondWeapon.Get();
                if (secondOrig != null)
                {
                    var secondNewGuid = DoubleSecondIds.ForVariant(data.AssetId);
                    var secondName = data.Name + "_Second";

                    ItemWeaponConfigurator
                        .New(secondName, secondNewGuid.ToString())
                        .CopyFrom(secondOrig.ToReference<BlueprintItemWeaponReference>())
                        .Configure();

                    var secondClone = ResourcesLibrary.TryGetBlueprint<BlueprintItemWeapon>(secondNewGuid);
                    if (secondClone != null)
                    {
                        var secBase = secondClone.m_OverrideDamageDice
                            ? secondClone.m_DamageDice
                            : secondClone.m_Type?.Get()?.m_BaseDamage ?? secondClone.m_DamageDice;

                        secondClone.m_OverrideDamageDice = true;
                        secondClone.m_DamageDice = StepUp(secBase);

                        // Añadir Oversized al segundo extremo también (si existe)
                        if (overRef != null)
                        {
                            var list2 = secondClone.m_Enchantments?.ToList() ?? new List<BlueprintWeaponEnchantmentReference>();
                            if (!list2.Contains(overRef)) list2.Add(overRef);
                            secondClone.m_Enchantments = list2.ToArray();
                        }

                        // Reapuntar el segundo del clon principal al nuevo clon
                        clone.m_SecondWeapon = secondClone.ToReference<BlueprintItemWeaponReference>();
                    }
                }
            }

            // 4) Añadir Oversized al principal (si existe)
            if (overRef != null)
            {
                var list = clone.m_Enchantments?.ToList() ?? new List<BlueprintWeaponEnchantmentReference>();
                if (!list.Contains(overRef)) list.Add(overRef);
                clone.m_Enchantments = list.ToArray();
            }
            else
            {
                UnityEngine.Debug.LogWarning($"[RRE] Oversized enchant not found in library: {OversizedEnchantGuid}");
            }

            UnityEngine.Debug.Log($"[RRE] Oversized OK: {data.Name} ({newId}) | {baseDamage.Rolls}d{(int)baseDamage.Dice} -> {stepped.Rolls}d{(int)stepped.Dice}");
        }


        // --- Escalado de daño por “size up” típico WotR/Pathfinder ---
        private static DiceFormula StepUp(DiceFormula f)
        {
            // Casos comunes 1 dado
            if (f.Rolls == 1)
            {
                switch (f.Dice)
                {
                    case DiceType.D3: return new DiceFormula(1, DiceType.D4);
                    case DiceType.D4: return new DiceFormula(1, DiceType.D6);
                    case DiceType.D6: return new DiceFormula(1, DiceType.D8);
                    case DiceType.D8: return new DiceFormula(2, DiceType.D6);
                    case DiceType.D10: return new DiceFormula(2, DiceType.D8);
                    case DiceType.D12: return new DiceFormula(3, DiceType.D6);
                }
            }
            // Algunos 2 dados típicos
            else if (f.Rolls == 2)
            {
                switch (f.Dice)
                {
                    case DiceType.D4: return new DiceFormula(2, DiceType.D6);
                    case DiceType.D6: return new DiceFormula(3, DiceType.D6);
                    case DiceType.D8: return new DiceFormula(3, DiceType.D8);
                    case DiceType.D10: return new DiceFormula(4, DiceType.D8);
                }
            }
            else if (f.Rolls == 3)
            {
                switch (f.Dice)
                {
                    case DiceType.D6: return new DiceFormula(4, DiceType.D6);
                    case DiceType.D8: return new DiceFormula(4, DiceType.D8);
                }
            }
            // Por defecto, sin cambio
            return f;
        }
    }
}
