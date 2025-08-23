using Kingmaker.EntitySystem.Stats; // SavingThrowType
using RandomReinforcementsPerEncounter.Domain.Models;
using System;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        // Helper para crear EnchantDef de tipo OnlyOnFirstHit
        private static EnchantDef MakeOnlyFirstHit(
            string seed,
            string affixName,
            string condition,        // "frightened", "stunned", ...
            string buffId,
            SavingThrowType save,
            int durDiceCount,
            int durDiceSides)
        {

            int[] OnlyOnFirstHitDCOneHanded = { 13, 17, 21, 26, 30, 34 };
            int[] OnlyOnFirstHitDCTwoHanded = { 17, 19, 23, 28, 32, 36 };
            int chance = 10;

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

                // DC por tier lógico
                TierMapOneHanded = OnlyOnFirstHitDCOneHanded,
                TierMapTwoHanded = OnlyOnFirstHitDCTwoHanded,

                // Específico de debuff on-hit
                OnHitBuffBlueprintId = buffId,
                OnHitSave = save,
                OnHitDurDiceCount = durDiceCount,
                OnHitDurDiceSides = durDiceSides,
            };
        }

        // ---- Definiciones (OnlyOnFirstHit) ----
        internal static readonly EnchantDef Frightening = MakeOnlyFirstHit("frightened", "Frightening", "frightened", "f08a7239aa961f34c8301518e71d4cdf", SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Stunning = MakeOnlyFirstHit("stunned", "Stunning", "stunned", "09d39b38bb7c6014394b6daced9bacd3", SavingThrowType.Fortitude, 1, 1);
        internal static readonly EnchantDef Dazing = MakeOnlyFirstHit("daze", "Dazing", "dazed", "d2e35b870e4ac574d9873b36402487e5", SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Slumbering = MakeOnlyFirstHit("sleep", "Slumbering", "asleep", "c9937d7846aa9ae46991e9f298be644a", SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Paralyzing = MakeOnlyFirstHit("paralyzed", "Paralyzing", "paralyzed", "4d5a2e4c34d24acca575c10003cf8fbc", SavingThrowType.Will, 1, 1);
        internal static readonly EnchantDef Exhausting = MakeOnlyFirstHit("exhausted", "Exhausting", "exhausted", "46d1b9cc3d0fd36469a471b047d773a2", SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Nauseating = MakeOnlyFirstHit("nauseated", "Nauseating", "nauseated", "956331dba5125ef48afe41875a00ca0e", SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Toppling = MakeOnlyFirstHit("prone", "Toppling", "prone", "24cf3deb078d3df4d92ba24b176bda97", SavingThrowType.Reflex, 1, 1);
        internal static readonly EnchantDef Dominating = MakeOnlyFirstHit("domination", "Dominating", "dominated", "c0f4e1c24c9cd334ca988ed1bd9d201f", SavingThrowType.Will, 1, 1);
    }
}
