using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Kingmaker.UnitLogic.Mechanics;

namespace RandomReinforcementsPerEncounter.GameApi.Mechanics
{
    internal static class RRECtx
    {
        public static MechanicsContext Permanent()
            => new MechanicsContext(default(JsonConstructorMark));
    }
}
