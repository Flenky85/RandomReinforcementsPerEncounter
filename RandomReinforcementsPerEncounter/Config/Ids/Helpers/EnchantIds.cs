using RandomReinforcementsPerEncounter.Domain.Models; // GuidUtil & WeaponGrip

namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class EnchantIds
    {
        /// <summary>Devuelve root con sufijo por mano: ".one" o ".two". Double -> ".one" (fallback).</summary>
        public static string RootWithHand(string root, WeaponGrip hand) => hand switch
        {
            WeaponGrip.OneHanded => $"{root}.one",
            WeaponGrip.TwoHanded => $"{root}.two",
            _ => $"{root}.one",
        };

        /// <summary>ID (string) para un tier concreto: "{root}.t{tier}"</summary>
        public static string Id(string root, int tier, WeaponGrip hand)
            => IdGenerators.EnchantId($"{RootWithHand(root, hand)}.t{tier}").ToString();
    }
}
