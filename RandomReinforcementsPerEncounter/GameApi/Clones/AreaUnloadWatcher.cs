using Kingmaker.PubSubSystem;

namespace RandomReinforcementsPerEncounter.GameApi.Clones
{
    public class AreaUnloadWatcher : IAreaHandler, IGlobalSubscriber
    {
        public AreaUnloadWatcher() => EventBus.Subscribe(this);

        public void OnAreaBeginUnloading() => CloneUtility.DestroyAllClones();

        public void OnAreaDidLoad() { }
    }
}
