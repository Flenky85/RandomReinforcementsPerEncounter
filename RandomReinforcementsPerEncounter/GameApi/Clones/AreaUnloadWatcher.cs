using System;
using System.Collections.Generic;
using Kingmaker.PubSubSystem;
using Kingmaker.Blueprints.Area;
using RandomReinforcementsPerEncounter.GameApi.Clones;

namespace RandomReinforcementsPerEncounter.GameApi.Clones
{
    public class AreaUnloadWatcher : IAreaHandler, IGlobalSubscriber
    {
        public AreaUnloadWatcher() => EventBus.Subscribe(this);

        public void OnAreaBeginUnloading() => CloneUtility.DestroyAllClones();

        public void OnAreaDidLoad() { }
    }
}
