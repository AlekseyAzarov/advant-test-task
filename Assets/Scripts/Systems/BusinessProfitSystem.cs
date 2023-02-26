using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace ClickerLogic
{
    public class BusinessProfitSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EventsBus _eventsBus;

        public void Init(IEcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            _world = systems.GetWorld();
            _eventsBus = sharedData.EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            var ownedBusinessesFilter = _world.Filter<BoughtBusinessComponent>().End();
            var businessesPool = _world.GetPool<BusinessComponent>();

            foreach (var ownedBusinessEntity in ownedBusinessesFilter)
            {
                ref var businessComponent = ref businessesPool.Get(ownedBusinessEntity);

                var currentLevel = businessComponent.CurrentLevel;
                var currentProfit = businessComponent.CurrentProfit;
                var maxProfitDelay = businessComponent.ProfitDelay;
                var currentProfitDelay = businessComponent.CurrentProfitDelay;
                currentProfitDelay.Value += Time.deltaTime;

                var upgradesMultiplier = 0f;
                foreach (var upgrade in businessComponent.Upgrades)
                {
                    upgradesMultiplier += upgrade.BaseProfitMultiplier;
                }

                currentProfit.Value = currentLevel.Value * businessComponent.BaseProfit * (1 + upgradesMultiplier);

                if (businessComponent.CurrentProfitDelay.Value >= maxProfitDelay)
                {
                    currentProfitDelay.Value = 0f;

                    _eventsBus.NewEvent<PlayerBalanceChangingEvent>().Amount = currentProfit.Value;
                }
            }
        }
    }
}
