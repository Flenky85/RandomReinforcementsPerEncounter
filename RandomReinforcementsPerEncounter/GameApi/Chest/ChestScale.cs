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
                1 => 0.75f,
                2 => 1.00f,
                3 => 1.25f,
                4 => 1.50f,
                5 => 1.75f,
                _ => 2.00f,// +6
            };
        }
    }
}
