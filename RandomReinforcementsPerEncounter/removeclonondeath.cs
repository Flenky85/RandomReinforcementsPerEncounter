using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RandomReinforcementsPerEncounter
{
    public static class CloneUtility
    {
        public static void DestroyAllClones()
        {
            foreach (var unit in Game.Instance?.State?.Units ?? Enumerable.Empty<UnitEntityData>())
            {
                if (unit?.View?.GetComponent<CloneMarker>() != null)
                    unit.MarkForDestroy();
            }
        }
    }

    public class CloneDeathWatcher : IUnitLifeStateChanged, IGlobalSubscriber
    {
        public CloneDeathWatcher()
        {
            EventBus.Subscribe(this);
        }

        public void HandleUnitLifeStateChanged(UnitEntityData unit, UnitLifeState prevState)
        {
            if (unit == null || !unit.Descriptor.State.IsDead)
                return;

            if (unit.View?.GetComponent<CloneMarker>() == null)
                return;

            var go = unit.View.gameObject;
            if (go == null)
                return;

            go.AddComponent<CloneDestructionDelayed>().Unit = unit;
        }

        private class CloneDestructionDelayed : MonoBehaviour
        {
            public UnitEntityData Unit;

            private void Awake()
            {
                StartCoroutine(DestroyAfterDelay());
            }

            private IEnumerator DestroyAfterDelay()
            {
                yield return new WaitForSeconds(4f);
                Unit?.MarkForDestroy();
            }
        }
    }

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.SaveRoutine))]
    public static class Patch_SaveManager_SaveRoutine
    {
        static void Prefix(SaveInfo saveInfo)
        {
            CloneUtility.DestroyAllClones();
        }
    }

    public class AreaUnloadWatcher : IAreaHandler, IGlobalSubscriber
    {
        public AreaUnloadWatcher()
        {
            EventBus.Subscribe(this);
        }

        public void OnAreaBeginUnloading()
        {
            CloneUtility.DestroyAllClones();
        }

        public void OnAreaDidLoad() { }
    }

    public class CloneMarker : MonoBehaviour
    {
        // Empty: serves only as an identifier
    }
}

