using RandomReinforcementsPerEncounter.Domain.Models; // GuidUtil & WeaponGrip

namespace RandomReinforcementsPerEncounter.Config.Ids.Generators
{
    internal static class EnchantIds
    {
        public static string RootWithHand(string root, WeaponGrip hand) => hand switch
        {
            WeaponGrip.OneHanded => $"{root}.one",
            WeaponGrip.TwoHanded => $"{root}.two",
            _ => $"{root}.one",
        };

        public static string Id(string root, int tier, WeaponGrip hand)
            => IdGenerators.EnchantId($"{RootWithHand(root, hand)}.t{tier}").ToString();
    }
}
