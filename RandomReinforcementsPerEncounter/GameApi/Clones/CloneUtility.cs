using Kingmaker;
using Kingmaker.UnitLogic;
using System.Linq;

namespace RandomReinforcementsPerEncounter.GameApi.Clones
{
    public static class CloneUtility
    {
        public static void DestroyAllClones()
        {
            var units = Game.Instance?.State?.Units;
            if (units == null) return;

            foreach (var unit in units.ToList()) // snapshot
            {
                if (unit?.View?.GetComponent<CloneMarker>() != null)
                    unit.MarkForDestroy();
            }
        }
    }
}
