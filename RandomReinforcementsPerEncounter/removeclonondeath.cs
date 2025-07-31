/*using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public class CloneDeathWatcher : IUnitLifeStateChanged, IGlobalSubscriber
    {
        public CloneDeathWatcher()
        {
            EventBus.Subscribe(this);
            Debug.Log("[Cloner] 🧠 Suscrito a muertes globales.");
        }

        public void HandleUnitLifeStateChanged(UnitEntityData unit, UnitLifeState prevState)
        {
            if (unit == null) return;
            if (!unit.Descriptor.State.IsDead) return;

            var blueprintName = unit.Blueprint?.name;
            if (string.IsNullOrEmpty(blueprintName)) return;
            if (!blueprintName.EndsWith("_Clone")) return;

            Debug.Log($"[Cloner] 💀 Eliminando clon muerto: {unit.CharacterName}");
            // Destruye el GameObject visual
            if (unit.View != null)
            {
                UnityEngine.Object.Destroy(unit.View.gameObject);
                Debug.Log("[Cloner] 🧽 Token visual destruido.");
            }

            // Destruye la entidad lógica
            unit.Destroy();
        }
    }
}*/

using System.Collections;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public class CloneDeathWatcher : IUnitLifeStateChanged, IGlobalSubscriber
    {
        public CloneDeathWatcher()
        {
            EventBus.Subscribe(this);
            Debug.Log("[Cloner] 🧠 Suscrito a muertes globales.");
        }

        public void HandleUnitLifeStateChanged(UnitEntityData unit, UnitLifeState prevState)
        {
            if (unit == null || !unit.Descriptor.State.IsDead) return;

            var blueprintName = unit.Blueprint?.name;
            if (string.IsNullOrEmpty(blueprintName)) return;
            if (!blueprintName.EndsWith("_Clone")) return;

            Debug.Log($"[Cloner] 💀 Clon muerto detectado: {unit.CharacterName}, eliminando en 4s...");

            var go = unit.View?.gameObject;
            if (go == null) return;

            var cleaner = go.AddComponent<CloneDestructionDelayed>();
            cleaner.Unit = unit;
        }

        private class CloneDestructionDelayed : MonoBehaviour
        {
            public UnitEntityData Unit;

            private void Start()
            {
                StartCoroutine(DestroyAfterDelay());
            }

            private IEnumerator DestroyAfterDelay()
            {
                yield return new WaitForSeconds(4f);

                if (Unit?.View != null)
                {
                    Destroy(Unit.View.gameObject);
                    Debug.Log("[Cloner] 🧽 Token visual destruido tras 4s.");
                }

                Unit?.Destroy();
                Debug.Log("[Cloner] 🧨 Unidad lógica eliminada tras 4s.");
            }
        }
    }
}


