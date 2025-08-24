using System;
using System.Linq;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    internal static class TierChances
    {
        public static int[] CalcTierChances(int cr)
        {
            int[] initial = { 52, 40, 30, 22, 16, 10 };
            int[] inc = { 6, 5, 4, 3, 2, 1 };
            int[] shift = { 1, 6, 11, 15, 18, 20 };

            int[] tiers = new int[6];
            for (int i = 0; i < 6; i++)
            {
                if (cr < shift[i]) { tiers[i] = 0; continue; }

                int raw = initial[i] + inc[i] * (cr - shift[i]);

                if (i <= 2 && raw > 80)
                {
                    int capCR = shift[i] + ((80 - initial[i] + inc[i] - 1) / inc[i]);
                    int dec = 4 * (cr - capCR);
                    tiers[i] = Math.Max(0, 80 - dec);
                }
                else
                {
                    tiers[i] = raw;
                }
            }
            return tiers;
        }

        public static int GetRandomTier(int[] chances)
        {
            int total = chances.Sum();
            if (total <= 0) return 0;

            int roll = UnityEngine.Random.Range(0, total);
            int acc = 0;

            for (int i = 0; i < chances.Length; i++)
            {
                acc += chances[i];
                if (roll < acc) return i + 1;
            }
            return 0;
        }
    }
}
