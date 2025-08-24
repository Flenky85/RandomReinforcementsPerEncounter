using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using RandomReinforcementsPerEncounter.Config.Ids.Tables;
using RandomReinforcementsPerEncounter.Domain;
using RandomReinforcementsPerEncounter.GameApi.Chest;
using RandomReinforcementsPerEncounter.State;
using System.Linq;
using System.Threading.Tasks;

namespace RandomReinforcementsPerEncounter.Patches
{
    public sealed class MainJoinCombatHandler : IUnitCombatHandler, IGlobalSubscriber
    {
        public static void Init() => EventBus.Subscribe(new MainJoinCombatHandler());

        public async void HandleUnitJoinCombat(UnitEntityData unit)
        {
            if (Game.Instance?.Player?.IsTurnBasedModeOn() == true) return;

            var main = Game.Instance?.Player?.MainCharacter.Value;
            if (unit == null || main == null) return;
            if (unit != main) return;

            // Pequeño delay para que el estado de combate se estabilice
            await Task.Delay(500);

            ReinforcementService.ScheduleFromCurrentCombat();
        }

        public void HandleUnitLeaveCombat(UnitEntityData unit)
        {
            if (Game.Instance?.Player?.IsTurnBasedModeOn() == true) return;

            bool anyInCombat = Game.Instance.State.Units.Any(u => u != null && u.IsInCombat);
            if (anyInCombat) return;

            // Spawn del cofre solo si tenemos posición
            if (LootContext.ChestPosition.HasValue)
            {
                ChestService.SpawnLootChest(BlueprintGuids.DefaultLootChest, LootContext.ChestPosition.Value);
            }

            // Reset de estado para el siguiente combate
            CombatFlags.Reset();
            LootContext.Reset();
            ReinforcementState.Clear();
        }
    }
}
