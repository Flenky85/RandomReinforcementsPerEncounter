using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;    // ApplyBuff, ContextDuration
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Utils;                       // LocalizationTool, BlueprintTool
using BlueprintCore.Utils.Types;                 // ContextValues
using Kingmaker.Blueprints;                      // BlueprintGuid
using Kingmaker.EntitySystem.Stats;              // SavingThrowType
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
                var nameKey = $"RRE.{nameRoot}.T{i + 1}.Name";
                var descKey = $"RRE.{nameRoot}.T{i + 1}.Desc";
                var bpName = $"RRE_{nameRoot}_T{i + 1}_Enchant";

                var locName = LocalizationTool.CreateString(nameKey, $"{nameRoot} (T{i + 1})");
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

        public class DebuffTierConfig
        {
            public string Seed { get; set; }
            public int DC { get; set; }
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

        private const string LINK_SAVE = "Encyclopedia:Saving_Throw";
        private const string LINK_DC = "Encyclopedia:DC";
        private const string LINK_DICE = "Encyclopedia:Dice";
        private const string LINK_ROUND = "Encyclopedia:Combat_Round"; // o "Encyclopedia:Round"

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

    }
}
