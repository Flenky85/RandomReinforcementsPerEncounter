using Kingmaker.EntitySystem.Stats; // SavingThrowType
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

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
