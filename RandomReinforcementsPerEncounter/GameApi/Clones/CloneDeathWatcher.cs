using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System.Collections;
using UnityEngine;

namespace RandomReinforcementsPerEncounter.GameApi.Clones
{
    public class CloneDeathWatcher : IUnitLifeStateChanged, IGlobalSubscriber
    {
        public CloneDeathWatcher() => EventBus.Subscribe(this);

        public void HandleUnitLifeStateChanged(UnitEntityData unit, UnitLifeState prevState)
        {
            if (unit == null || unit.Descriptor == null) return;
            if (!unit.Descriptor.State.IsDead) return;

            var view = unit.View;
            if (view == null) return;
            if (view.GetComponent<CloneMarker>() == null) return;

            view.StartCoroutine(DestroyAfterDelay(unit, 4f));
        }

        private static IEnumerator DestroyAfterDelay(UnitEntityData unit, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            unit?.MarkForDestroy();
        }
    }
}
