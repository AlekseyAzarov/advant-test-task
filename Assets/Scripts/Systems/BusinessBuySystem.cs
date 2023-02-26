using Leopotam.EcsLite;

namespace ClickerLogic
{
    public class BusinessBuySystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            var businessFilter = _world.Filter<BusinessComponent>().End();
            var businessesPool = _world.GetPool<BusinessComponent>();
            var playerOwnedBusinessPool = _world.GetPool<BoughtBusinessComponent>();

            foreach (var businessEntity in businessFilter)
            {
                ref var businessComponent = ref businessesPool.Get(businessEntity);

                if (businessComponent.CurrentLevel.Value > 0 && !playerOwnedBusinessPool.Has(businessEntity))
                {
                    playerOwnedBusinessPool.Add(businessEntity);
                }
            }
        }
    }
}
