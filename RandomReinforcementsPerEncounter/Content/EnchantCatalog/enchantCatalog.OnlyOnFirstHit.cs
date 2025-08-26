﻿using Kingmaker.EntitySystem.Stats; 
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
       
        private static EnchantDef MakeOnlyFirstHit(
            string seed,
            string affixName,
            string condition,        
            string buffId,
            SavingThrowType save,
            int durDiceCount,
            int durDiceSides)
        {

            int[] OnlyOnFirstHitDCOneHanded = { 13, 17, 21, 26, 30, 34 };
            int[] OnlyOnFirstHitDCTwoHanded = { 17, 19, 23, 28, 32, 36 };
            int chance = 5;

            return new EnchantDef
            {
                Seed = seed,
                Name = affixName,
                AffixDisplay = affixName,
                Desc = condition,
                Affix = AffixKind.Prefix,
                Type = EnchantType.OnlyOnFirstHit,
                Chance = chance,

                ApplyToBothHeadsOnDouble = true,

                TierMapOneHanded = OnlyOnFirstHitDCOneHanded,
                TierMapTwoHanded = OnlyOnFirstHitDCTwoHanded,
                              
                OnHitBuffBlueprintId = buffId,
                OnHitSave = save,
                OnHitDurDiceCount = durDiceCount,
                OnHitDurDiceSides = durDiceSides,
            };
        }

        internal static readonly EnchantDef Frightening = MakeOnlyFirstHit(Seed.frightened, "Frightening", "frightened", BlueprintGuids.Frightened, SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Stunning = MakeOnlyFirstHit(Seed.stunned, "Stunning", "stunned", BlueprintGuids.Stunned, SavingThrowType.Fortitude, 1, 1);
        internal static readonly EnchantDef Dazing = MakeOnlyFirstHit(Seed.daze, "Dazing", "dazed", BlueprintGuids.Dazed, SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Slumbering = MakeOnlyFirstHit(Seed.sleep, "Slumbering", "asleep", BlueprintGuids.Asleep, SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Paralyzing = MakeOnlyFirstHit(Seed.paralyzed, "Paralyzing", "paralyzed", BlueprintGuids.Paralyzed, SavingThrowType.Will, 1, 1);
        internal static readonly EnchantDef Exhausting = MakeOnlyFirstHit(Seed.exhausted, "Exhausting", "exhausted", BlueprintGuids.Exhausted, SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Nauseating = MakeOnlyFirstHit(Seed.nauseated, "Nauseating", "nauseated", BlueprintGuids.Nauseated, SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Toppling = MakeOnlyFirstHit(Seed.prone, "Toppling", "prone", BlueprintGuids.Prone, SavingThrowType.Reflex, 1, 1);
        internal static readonly EnchantDef Dominating = MakeOnlyFirstHit(Seed.domination, "Dominating", "dominated", BlueprintGuids.Dominated, SavingThrowType.Will, 1, 1);
    }
}
