using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClickerLogic
{
    public class PlayerResourcesSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EventsBus _eventsBus;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _eventsBus = systems.GetShared<SharedData>().EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            if (!_eventsBus.HasEvents<PlayerBalanceChangingEvent>()) return;

            var playerBalanceFilter = _world.Filter<PlayerBalanceComponent>().End();
            var playerBalanceComponents = _world.GetPool<PlayerBalanceComponent>();

            foreach (var eventEntity in _eventsBus.GetEventBodies<PlayerBalanceChangingEvent>(out var eventsPool))
            {
                ref var eventData = ref eventsPool.Get(eventEntity);

                foreach (var playerBalanceEntity in playerBalanceFilter)
                {
                    ref var playerBalance = ref playerBalanceComponents.Get(playerBalanceEntity);
                    playerBalance.Balance.Value += eventData.Amount;
                }
            }
        }
    }
}
