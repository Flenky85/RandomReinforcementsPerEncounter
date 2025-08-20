using Kingmaker.EntitySystem.Persistence.JsonUtility;
using Kingmaker.UnitLogic.Mechanics;

namespace RandomReinforcementsPerEncounter.GameApi.Mechanics
{
    internal static class RRECtx
    {
        // Contexto “dummy” para marcar enchant como externo/persistente
        public static MechanicsContext Permanent()
            => new MechanicsContext(default(JsonConstructorMark));
    }
}
