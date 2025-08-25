using UnityEngine;

namespace RandomReinforcementsPerEncounter.GameApi.Chest
{
    internal static class ChestScale
    {
        public static float ForPlus(int plus)
        {
            return Mathf.Clamp(plus, 0, 6) switch
            {
                0 => 0.50f,
                1 => 0.65f,
                2 => 0.80f,
                3 => 0.95f,
                4 => 1.10f,
                5 => 1.25f,
                _ => 1.40f,
            };
        }
    }
}
