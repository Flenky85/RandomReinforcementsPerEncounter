using Kingmaker.Blueprints;

namespace RandomReinforcementsPerEncounter.GameApi.Enchantments.Components
{
    /// <summary>
    /// Adds a flat gp delta to the item price, used by hidden price-tier enchants.
    /// </summary>
    public class RRE_PriceDeltaComponent : BlueprintComponent
    {
        public int Delta; // gp to add
    }
}
