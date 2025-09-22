using RandomReinforcementsPerEncounter.Config.Settings;
using System;
using System.Linq;

namespace RandomReinforcementsPerEncounter.GameApi.Loot
{
    // Maps party/enemy CR to per-tier weights (T1..T6), then samples a tier by weighted roll.
    internal static class TierChances
    {
        public static int[] CalcTierChances(int cr)
        {
            // Base config per tier (index 0..5 = T1..T6):
            // - initial: base chance when the tier first unlocks
            // - inc:     chance added per CR above the unlock
            // - shift:   CR at which the tier begins to appear
            int[] initial = { 52, 40, 30, 22, 16, 10 };
            int[] inc = { 6, 5, 4, 3, 2, 1 };
            int[] shift = { 1, 6, 11, 15, 18, 20 };

            int[] tiers = new int[6];
            for (int i = 0; i < 6; i++)
            {
                // Below unlock CR → this tier is not available.
                if (cr < shift[i]) { tiers[i] = 0; continue; }

                int raw = initial[i] + inc[i] * (cr - shift[i]);

                // For early tiers (T1..T3), apply a soft cap at 80, then decay by 4 per CR past the cap.
                // This reduces low-tier dominance at very high CR.
                if (i <= 2 && raw > 80)
                {
                    int capCR = shift[i] + ((80 - initial[i] + inc[i] - 1) / inc[i]); // first CR that would hit 80
                    int dec = 4 * (cr - capCR);
                    tiers[i] = Math.Max(0, 80 - dec);
                }
                else
                {
                    tiers[i] = raw;
                }
            }
            var s = ModSettings.Instance;

            int[] mult = {
                s.TierVisual1,
                s.TierVisual2,
                s.TierVisual3,
                s.TierVisual4,
                s.TierVisual5,
                s.TierVisual6
            };

            for (int i = 0; i < 6; i++)
            {
                tiers[i] = Math.Max(0, (int)Math.Round(tiers[i] * (double)mult[i]));
            }

            return tiers;
        }

        public static int GetRandomTier(int[] chances)
        {
            // Weighted pick: returns 1..6 for T1..T6; 0 if nothing is eligible.
            int total = chances.Sum();
            if (total <= 0) return 0;

            int roll = UnityEngine.Random.Range(0, total); // [0, total)
            int acc = 0;

            for (int i = 0; i < chances.Length; i++)
            {
                acc += chances[i];
                if (roll < acc) return i + 1;
            }
            return 0; // Fallback (should not happen if totals are consistent)
        }
    }
}
