using Kingmaker.EntitySystem.Stats; // SavingThrowType
using RandomReinforcementsPerEncounter.Domain.Models;


namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantCatalog
    {
        // Helper mínimo para OnHit (Prefix por defecto)
        private static EnchantDef MakeOnHit(
            string seed,
            string affixName,
            string condition,           // "shaken", "blinded", ...
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

                // DC por tier lógico
                TierMapOneHanded = OnHitDCOneHanded,
                TierMapTwoHanded = OnHitDCTwoHanded,

                // Específico OnHit
                OnHitBuffBlueprintId = buffId,
                OnHitSave = save,
                OnHitDurDiceCount = durDiceCount,
                OnHitDurDiceSides = durDiceSides,
            };
        }

        // ---- Definiciones ----
        internal static readonly EnchantDef Fearsome = MakeOnHit("shaken", "Fearsome", "shaken", "25ec6cb6ab1845c48a95f9c20b034220", SavingThrowType.Will, 1, 3);
        internal static readonly EnchantDef Blinding = MakeOnHit("blindness", "Blinding", "blinded", "0ec36e7596a4928489d2049e1e1c76a7", SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Dazzling = MakeOnHit("dazzled", "Dazzling", "dazzled", "df6d1020da07524423afbae248845ecc", SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Sickening = MakeOnHit("sickened", "Sickening", "sickened", "4e42460798665fd4cb9143ffa7ada323", SavingThrowType.Fortitude, 1, 1);
        internal static readonly EnchantDef Staggering = MakeOnHit("staggered", "Staggering", "staggered", "df3950af5a783bd4d91ab73eb8fa0fd3", SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Fatiguing = MakeOnHit("fatigue", "Fatiguing", "fatigued", "e6f2fc5d73d88064583cb828801172f4", SavingThrowType.Fortitude, 1, 3);
        internal static readonly EnchantDef Confusing = MakeOnHit("confusion", "Confusing", "confused", "886c7407dc623dc499b9f1465ff382df", SavingThrowType.Will, 1, 1);
        internal static readonly EnchantDef Entangling = MakeOnHit("entangled", "Entangling", "entangled", "f7f6260726117cf4b90a6086b05d2e38", SavingThrowType.Reflex, 1, 1);
        internal static readonly EnchantDef Slowing = MakeOnHit("slowed", "Slowing", "slowed", "488e53ede2802ff4da9372c6a494fb66", SavingThrowType.Reflex, 1, 1);
    }
}
