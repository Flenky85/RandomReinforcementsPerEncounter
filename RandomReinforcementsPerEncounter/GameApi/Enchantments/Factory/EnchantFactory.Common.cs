using RandomReinforcementsPerEncounter.Domain.Models;

namespace RandomReinforcementsPerEncounter
{
    internal static partial class EnchantFactory
    {
        // seed por variante (.one / .two)
        internal static string RootWithHand(string root, WeaponGrip hand) => hand switch
        {
            WeaponGrip.OneHanded => $"{root}.one",
            WeaponGrip.TwoHanded => $"{root}.two",
            _ => $"{root}.one",
        };
        // Nuevo Id con mano: respeta tu patrón original pero añade el sufijo antes del .t{tier}
        internal static string Id(string root, int tier, WeaponGrip hand)
            => GuidUtil.EnchantGuid($"{RootWithHand(root, hand)}.t{tier}").ToString();
    }

}
