using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using System.Linq;

namespace ClickerLogic
{
    public class BusinessUpgradeSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EventsBus _eventsBus;
        private BusinessesConfig _businessesConfig;

        public BusinessUpgradeSystem(BusinessesConfig businessesConfig)
        {
            _businessesConfig = businessesConfig;
        }

        public void Init(IEcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            _world = systems.GetWorld();
            _eventsBus = sharedData.EventsBus;
        }

        public void Run(IEcsSystems systems)
        {
            var playerBalanceFilter = _world.Filter<PlayerBalanceComponent>().End();
            var playerBalanceComponents = _world.GetPool<PlayerBalanceComponent>();

            var businessesFilter = _world.Filter<BusinessComponent>().End();
            var businessesComponents = _world.GetPool<BusinessComponent>();

            foreach (var businessEntity in businessesFilter)
            {
                ref var businessComponent = ref businessesComponents.Get(businessEntity);
                var cachedBalanceSpent = 0f;

                var businessId = businessComponent.Id;
                var businessConfig = _businessesConfig.BusinessConfigs.First(x => x.Id == businessId);
                var basePrice = businessConfig.BasePrice;
                var currentPrice = businessComponent.CurrentPrice;

                foreach (var levelUpEventEntity in _eventsBus.GetEventBodies<BusinessLevelUpEvent>(out var eventPool))
                {
                    ref var eventData = ref eventPool.Get(levelUpEventEntity);

                    if (businessComponent.Id != eventData.BusinessId) continue;

                    foreach (var playerBalanceEntity in playerBalanceFilter)
                    {
                        ref var balanceComponent = ref playerBalanceComponents.Get(playerBalanceEntity);

                        if (balanceComponent.Balance.Value >= currentPrice.Value)
                        {
                            _eventsBus.NewEvent<PlayerBalanceChangingEvent>().Amount = -currentPrice.Value;
                            businessComponent.CurrentLevel.Value++;
                            cachedBalanceSpent = currentPrice.Value;
                        }
                    }
                }

                currentPrice.Value = (businessComponent.CurrentLevel.Value + 1) * basePrice;

                foreach (var upgradingEventEntity in _eventsBus.GetEventBodies<BusinessUpgradingEvent>(out var eventPool))
                {
                    ref var eventData = ref eventPool.Get(upgradingEventEntity);

                    if (businessComponent.Id != eventData.BusinessId) continue;

                    foreach (var playerBalanceEntity in playerBalanceFilter)
                    {
                        ref var balanceComponent = ref playerBalanceComponents.Get(playerBalanceEntity);

                        var businessName = businessComponent.Id;
                        var upgradeName = eventData.UpgradeId;
                        var targetUpgrade = businessConfig.Upgrades.First(x => x.Id == upgradeName);

                        if (businessComponent.Upgrades.Any(x => x.Id == targetUpgrade.Id)) return;

                        if (balanceComponent.Balance.Value >= targetUpgrade.BasePrice + cachedBalanceSpent)
                        {
                            businessComponent.AddUpgrade(targetUpgrade);
                            _eventsBus.NewEvent<PlayerBalanceChangingEvent>().Amount = -targetUpgrade.BasePrice;
                        }
                    }
                }
            }
        }
    }
}
