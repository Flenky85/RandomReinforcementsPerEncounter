using Kingmaker.EntitySystem.Stats; 
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
     
        private static EnchantDef MakeOnHit(
            string seed,
            string affixName,
            string condition,           
            string buffId,
            SavingThrowType save,
            int durDiceCount,
            int durDiceSides)   
        {
            int[] OnHitDCOneHanded = { 11, 15, 19, 24, 28, 32 };
            int[] OnHitDCTwoHanded = { 13, 17, 21, 26, 30, 34 };
            int chance = 10;

            return new EnchantDef
            {
                Seed = seed,
                Name = affixName,
                AffixDisplay = affixName,
                Desc = condition,
                Affix = AffixKind.Prefix,
                Type = EnchantType.OnHit,
                Chance = chance,

                ApplyToBothHeadsOnDouble = true,

                TierMapOneHanded = OnHitDCOneHanded,
                TierMapTwoHanded = OnHitDCTwoHanded,

                OnHitBuffBlueprintId = buffId,
                OnHitSave = save,
                OnHitDurDiceCount = durDiceCount,
                OnHitDurDiceSides = durDiceSides,
            };
        }

        internal static readonly EnchantDef Fearsome = MakeOnHit(Seed.shaken, "Fearsome", "shaken", BlueprintGuids.Shaken, SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Blinding = MakeOnHit(Seed.blindness, "Blinding", "blinded", BlueprintGuids.Blinded, SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Dazzling = MakeOnHit(Seed.dazzled, "Dazzling", "dazzled", BlueprintGuids.Dazzled, SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Sickening = MakeOnHit(Seed.sickened, "Sickening", "sickened", BlueprintGuids.Sickened, SavingThrowType.Fortitude, 1, 1);
        internal static readonly EnchantDef Staggering = MakeOnHit(Seed.staggered, "Staggering", "staggered", BlueprintGuids.Staggered, SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Fatiguing = MakeOnHit(Seed.fatigue, "Fatiguing", "fatigued", BlueprintGuids.Fatigued, SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Confusing = MakeOnHit(Seed.confusion, "Confusing", "confused", BlueprintGuids.Confused, SavingThrowType.Will, 1, 1);
        internal static readonly EnchantDef Entangling = MakeOnHit(Seed.entangled, "Entangling", "entangled", BlueprintGuids.Entangled, SavingThrowType.Reflex, 1, 1);
        internal static readonly EnchantDef Slowing = MakeOnHit(Seed.slowed, "Slowing", "slowed", BlueprintGuids.Slowed, SavingThrowType.Reflex, 1, 1);

    }
}
