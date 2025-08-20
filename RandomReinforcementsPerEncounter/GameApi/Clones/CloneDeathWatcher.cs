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
            if (unit.View == null) return;
            if (unit.View.GetComponent<CloneMarker>() == null) return;

            var go = unit.View.gameObject;
            if (go == null) return;

            go.AddComponent<CloneDestructionDelayed>().Unit = unit;
        }

        private class CloneDestructionDelayed : MonoBehaviour
        {
            public UnitEntityData Unit;

            private void Awake() => StartCoroutine(DestroyAfterDelay());

            private IEnumerator DestroyAfterDelay()
            {
                yield return new WaitForSeconds(4f);
                Unit?.MarkForDestroy();
            }
        }
    }
}
