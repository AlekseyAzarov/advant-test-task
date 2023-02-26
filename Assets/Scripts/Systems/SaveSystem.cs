using Leopotam.EcsLite;
using System.Collections.Generic;

namespace ClickerLogic
{
    public class SaveSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private ISaveService _saveService;
        private ApplicationFocusHandler _focusHandler;
        private EcsWorld _world;

        public SaveSystem(ISaveService saveService, ApplicationFocusHandler focusHandler)
        {
            _saveService = saveService;
            _focusHandler = focusHandler;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _focusHandler.FocusChanged += OnFocusChanged;
        }

        public void Destroy(IEcsSystems systems)
        {
            _focusHandler.FocusChanged -= OnFocusChanged;
            Save(_world);
        }

        private void Save(EcsWorld world)
        {
            var businessesFilter = world.Filter<BusinessComponent>().End();
            var businessesComponents = world.GetPool<BusinessComponent>();
            var businessesToSave = new List<BusinessComponent>();

            foreach (var businessEntity in businessesFilter)
            {
                ref var businessComponent = ref businessesComponents.Get(businessEntity);
                businessesToSave.Add(businessComponent);
            }

            SaveBusinesses(businessesToSave);

            var playerBalanceFilter = world.Filter<PlayerBalanceComponent>().End();
            var balancePool = world.GetPool<PlayerBalanceComponent>();

            foreach (var playerBalanceEntity in playerBalanceFilter)
            {
                ref var balanceComponent = ref balancePool.Get(playerBalanceEntity);

                var savedData = new PlayerBalanceDataSave
                {
                    Balance = balanceComponent.Balance.Value
                };

                SavePlayerBalance(savedData);
            }
        }

        private void SavePlayerBalance(PlayerBalanceDataSave data)
        {
            _saveService.Save(data, SaveFileNames.PLAYER_BALANCE);
        }

        private void SaveBusinesses(List<BusinessComponent> businessComponents)
        {
            var saveData = new List<BusinessDataSave>();

            foreach (var businessComponent in businessComponents)
            {
                saveData.Add(new BusinessDataSave
                {
                    Id = businessComponent.Id,
                    ProfitDelay = businessComponent.ProfitDelay,
                    CurrentProfitDelay = businessComponent.CurrentProfitDelay.Value,
                    Level = businessComponent.CurrentLevel.Value,
                    Price = businessComponent.CurrentPrice.Value,
                    Profit = businessComponent.CurrentProfit.Value,
                    BaseProfit = businessComponent.BaseProfit,
                    Upgrades = businessComponent.Upgrades
                });
            }

            _saveService.Save(saveData, SaveFileNames.BUSINESSES_FILE);
        }

        private void OnFocusChanged(bool hasFocus)
        {
            if (hasFocus) return;

            Save(_world);
        }
    }
}
