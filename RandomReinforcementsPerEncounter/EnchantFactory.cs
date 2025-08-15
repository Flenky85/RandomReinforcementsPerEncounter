using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;    // ApplyBuff, ContextDuration
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;                       // LocalizationTool, BlueprintTool
using BlueprintCore.Utils.Types;                 // ContextValues
using Kingmaker.Blueprints;                      // BlueprintGuid
using Kingmaker.EntitySystem.Stats;              // SavingThrowType
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;                      // DiceType
using Kingmaker.UnitLogic.Buffs.Blueprints;      // BlueprintBuffReference
using Kingmaker.UnitLogic.Mechanics.Actions;     // ContextActionSavingThrow, ContextActionConditionalSaved
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Collections.Generic;  // ContextSetAbilityParams

namespace RandomReinforcementsPerEncounter
{
    public static class EnchantFactory
    {
        public static void RegisterDebuffTiersFor(
            List<DebuffTierConfig> tiers,
            string nameRoot,
            string buff, 
            int durationDiceCount, 
            int durationDiceSides, 
            int savingThrowType,
            int activationType,
            string description,   
            string condition     
            )
        {
  
            var buffRef = BlueprintTool.GetRef<BlueprintBuffReference>(buff);
            var diceType = MapDiceType(durationDiceSides);
            var saveType = MapSavingThrowType(savingThrowType);

            bool onlyHit = activationType == 1;
            bool onlyOnFirstHit = activationType == 2;

            for (int i = 0; i < tiers.Count; i++)
            {
                var tierConfig = tiers[i];
                var guid = GuidUtil.FromString(tierConfig.Seed);
                var keys = BuildKeys(nameRoot, i + 1);
                var nameKey = keys.nameKey;
                var descKey = keys.descKey;
                var bpName = keys.bpName;
                
                var locName = keys.locName;
                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    BuildDescription(
                        saveType,
                        tierConfig.DC,
                        description,
                        condition,
                        durationDiceCount,
                        durationDiceSides,
                        onlyOnFirstHit
                    )
                );

                WeaponEnchantmentConfigurator
                    .New(bpName, guid.ToString())
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .Configure();

                var onSuccess = ActionsBuilder.New();
                var onFail = ActionsBuilder.New()
                    .ApplyBuff(
                        buffRef,
                        ContextDuration.VariableDice(diceType, durationDiceCount, isExtendable: true),
                        toCaster: false,
                        asChild: true
                    );

                var savedBranch = ActionsBuilder.New()
                    .Add<ContextActionConditionalSaved>(c =>
                    {
                        c.Succeed = onSuccess.Build();
                        c.Failed = onFail.Build();
                    });

                var onHitActions = ActionsBuilder.New()
                    .Add<ContextActionSavingThrow>(a =>
                    {
                        a.Type = saveType;
                        a.FromBuff = false;
                        a.HasCustomDC = false;
                        a.Actions = savedBranch.Build();
                    });

                WeaponEnchantmentConfigurator
                    .For(guid.ToString())
                    .AddInitiatorAttackWithWeaponTrigger(
                        onHitActions,
                        onlyHit: onlyHit,
                        onlyOnFirstHit: onlyOnFirstHit
                    )
                    .AddComponent<ContextSetAbilityParams>(c =>
                    {
                        c.Add10ToDC = false;
                        c.DC = ContextValues.Constant(tierConfig.DC);
                        c.CasterLevel = ContextValues.Constant(0);
                        c.Concentration = ContextValues.Constant(0);
                        c.SpellLevel = ContextValues.Constant(0);
                    })
                    .Configure();
            }
        }

        public static void RegisterDamageTiersFor(
                    List<DebuffTierConfig> tiers,
                    string nameRoot,          // ej: "Flaming"
                    string description,       // ej: "fire"
                    string prefab             // ej: "91e5a56dd421a2941984a36a2af164b6"
                )
        {
            var energy = MapEnergyType(description);
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];

                // Validación rápida
                var rolls = t.DiceCount <= 0 ? 1 : t.DiceCount;
                var diceT = MapDiceType(t.DiceSide <= 0 ? 6 : t.DiceSide);

                var guid = GuidUtil.FromString(t.Seed);
                var keys = BuildKeys(nameRoot, i + 1);
                var nameKey = keys.nameKey;
                var descKey = keys.descKey;
                var bpName = keys.bpName;

                var locName = keys.locName;
                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    BuildEnergyDescription(rolls, t.DiceSide <= 0 ? 6 : t.DiceSide, description)
                );

                // Crear el enchant
                var cfg = WeaponEnchantmentConfigurator
                    .New(bpName, guid.ToString())
                    .SetEnchantName(locName)
                    .SetDescription(locDesc);

                // (Opcional) FX de arma si nos pasas el guid
                if (!string.IsNullOrEmpty(prefab))
                {
                    // Nota: Si tu versión de BlueprintCore usa otra firma para el prefab,
                    // cambia esta línea por la equivalente que uses en tu proyecto.
                    cfg = cfg.SetWeaponFxPrefab(prefab);
                }

                // Añadir el componente de daño de energía
                cfg.AddWeaponEnergyDamageDice(
                        element: energy,
                        energyDamageDice: new DiceFormula(rolls, diceT)
                    )
                   .Configure();
            }
        }

        public static void RegisterWeaponStatsTiersFor(
            List<DebuffTierConfig> tiers,
            string nameRoot,                 // p.ej. "Strength"
            StatType stat,                   // p.ej. StatType.Strength
            string description,                 // p.ej. "Strength" (texto visible)
            string encyclopedia
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int plus = t.Bonus <= 0 ? 1 : t.Bonus;

                var enchGuid = GuidUtil.FromString(t.Seed);
                var keys = BuildKeys(nameRoot, i + 1);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var descKey = keys.descKey;

                // Usa tu descripción existente (bonus + enlace a stat)
                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    BuildEnhancementDescription(plus, description, encyclopedia)
                );

                WeaponEnchantmentConfigurator
                    .New(bpName, enchGuid.ToString())
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .AddStatBonusEquipment(
                        descriptor: ModifierDescriptor.UntypedStackable,
                        stat: stat,
                        value: plus
                    )
                    .Configure();
            }
        }

        public static void RegisterWeaponSpellsTiersFor(
            List<DebuffTierConfig> tiers,
            string nameRoot,     
            string description,  
            string encyclopedia,
            string type
        )
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                var t = tiers[i];
                int plus = t.Bonus <= 0 ? 1 : t.Bonus;

                var enchGuid = GuidUtil.FromString(t.Seed);
                var keys = BuildKeys(nameRoot, i + 1);
                var bpName = keys.bpName;
                var locName = keys.locName;
                var descKey = keys.descKey;

                // Usa tu descripción existente (bonus + enlace a stat)
                var locDesc = LocalizationTool.CreateString(
                    descKey,
                    BuildEnhancementDescription(plus, description, encyclopedia)
                );

                WeaponEnchantmentConfigurator
                    .New(bpName, enchGuid.ToString())
                    .SetEnchantName(locName)
                    .SetDescription(locDesc)
                    .AddIncreaseSpellDC(
                        descriptor: ModifierDescriptor.UntypedStackable,
                        B: stat,
                        value: plus
                    )
                    .Configure();
            }
        }

        public class DebuffTierConfig
        {
            public string Seed { get; set; }
            public int DC { get; set; }              // se ignora en daño de energía
            public int DiceCount { get; set; }       // NUEVO: usado por daño de energía
            public int DiceSide { get; set; }        // NUEVO: usado por daño de energía
            public int Bonus { get; set; }
        }

        private static DiceType MapDiceType(int sides)
        {
            return sides switch
            {
                1 => DiceType.One,
                3 => DiceType.D3,
                4 => DiceType.D4,
                6 => DiceType.D6,
                8 => DiceType.D8,
                10 => DiceType.D10,
                12 => DiceType.D12,
                20 => DiceType.D20,
                100 => DiceType.D100,
                _ => DiceType.D3,// fallback
            };
        }
        private static SavingThrowType MapSavingThrowType(int id)
        {
            return id switch
            {
                1 => SavingThrowType.Fortitude,
                2 => SavingThrowType.Will,
                3 => SavingThrowType.Reflex,
                _ => SavingThrowType.Fortitude,// fallback
            };
        }
        private static (string nameKey, string descKey, string bpName, LocalizedString locName) BuildKeys(string nameRoot, int tierIndex)
        {
            string nameKey = $"RRE.{nameRoot}.T{tierIndex}.Name";
            string descKey = $"RRE.{nameRoot}.T{tierIndex}.Desc";
            string bpName = $"RRE_{nameRoot}_T{tierIndex}_Enchant";
            var locName = LocalizationTool.CreateString(nameKey, $"{nameRoot} (T{tierIndex})");

            return (nameKey, descKey, bpName, locName);
        }

        private const string LINK_SAVE = "Encyclopedia:Saving_Throw";
        private const string LINK_DC = "Encyclopedia:DC";
        private const string LINK_DICE = "Encyclopedia:Dice";
        private const string LINK_ROUND = "Encyclopedia:Combat_Round";
        private const string LINK_ENERGY = "Encyclopedia:Energy_Damage";
        private const string LINK_BONUS = "Encyclopedia:Bonus";

        private static string BuildDescription(
            SavingThrowType saveType, int dc,
            string conditionText,       // "blinded"  (Visible word)
            string conditionLinkKey,    // "Blind"    (Encyclopedia)
            int durationCount, int durationSides,
            bool onlyOnFirstHit)
        {
            string intro = onlyOnFirstHit
                ? "The first time this weapon hits a given enemy, that enemy must pass a "
                : "Whenever this weapon lands a hit, the enemy must pass a ";

            string saveName = saveType.ToString().ToLowerInvariant();
            string diceText = durationSides == 1 ? durationCount.ToString() : $"{durationCount}d{durationSides}";
            bool isExactlyOneRound = durationSides == 1 && durationCount == 1;
            string roundText = isExactlyOneRound ? "round" : "rounds";

            // Enlaces
            string saveChunk = $"{{g|{LINK_SAVE}}}{saveName} saving throw{{/g}}";
            string dcChunk = $"({{g|{LINK_DC}}}DC{{/g}} {dc})";
            string condChunk = $"{{g|Encyclopedia:Condition{conditionLinkKey}}}{conditionText}{{/g}}";
            string diceChunk = $"{{g|{LINK_DICE}}}{diceText}{{/g}}";
            string roundChunk = $"{{g|{LINK_ROUND}}}{roundText}{{/g}}";

            return $"{intro}{saveChunk} {dcChunk} or become {condChunk} for {diceChunk} {roundChunk}.";
        }

        private static string BuildEnergyDescription(int diceCount, int diceSides, string energyWord)
        {
            string diceText = diceSides == 1 ? diceCount.ToString() : $"{diceCount}d{diceSides}";
            string diceChunk = $"{{g|{LINK_DICE}}}{diceText}{{/g}}";
            string energyChunk = $"{{g|{LINK_ENERGY}}}{energyWord} damage{{/g}}";

            // Mantén la redacción sencilla y consistente con el resto:
            return $"This weapon deals an extra {diceChunk} points of {energyChunk} on a successful hit.";
        }

        private static string BuildEnhancementDescription(int bonus, string description, string encyclopedia)
        {
            // “This item grants a +X enhancement bonus to Strength.”
            string bonusChunk = $"{{g|{LINK_BONUS}}}bonus{{/g}}";
            string statChunk = $"{{g|Encyclopedia:{encyclopedia}}}{description}{{/g}}";
            return $"This item grants a +{bonus} stackable {bonusChunk} to {statChunk}.";
        }

        private static DamageEnergyType MapEnergyType(string s)
        {
            if (string.IsNullOrEmpty(s)) return DamageEnergyType.Fire;
            switch (s.ToLowerInvariant())
            {
                case "acid": return DamageEnergyType.Acid;
                case "fire": return DamageEnergyType.Fire;
                case "cold": return DamageEnergyType.Cold;
                case "electricity": return DamageEnergyType.Electricity;
                case "sonic": return DamageEnergyType.Sonic;
                case "negative damage": return DamageEnergyType.NegativeEnergy;
                case "holy": return DamageEnergyType.Holy;
                default: return DamageEnergyType.Fire;
            }
        }

    }
}
