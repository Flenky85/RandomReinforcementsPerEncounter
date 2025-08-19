using System.Collections.Generic;

namespace RandomReinforcementsPerEncounter.Config
{
    internal static class LootEconomy
    {
        internal static readonly Dictionary<int, int> GoldByCR = new Dictionary<int, int>
        {
            { 1, 10 }, { 2, 20 }, { 3, 30 }, { 4, 50 }, { 5, 70 },
            { 6, 100 }, { 7, 130 }, { 8, 160 }, { 9, 200 }, { 10, 240 },
            { 11, 280 }, { 12, 330 }, { 13, 380 }, { 14, 440 }, { 15, 500 },
            { 16, 600 }, { 17, 700 }, { 18, 800 }, { 19, 900 }, { 20, 1000 },
            { 21, 1100 }, { 22, 1200 }, { 23, 1300 }, { 24, 1400 }, { 25, 1500 },
            { 26, 1600 }, { 27, 1700 }, { 28, 1800 }, { 29, 1900 }, { 30, 2000 }
        };

        internal static int GetBaseGoldForCR(int cr)
            => GoldByCR.TryGetValue(cr, out var v) ? v : 2000;
    }
}
