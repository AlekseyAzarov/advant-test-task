using Leopotam.EcsLite;
using System.Collections.Generic;

namespace ClickerLogic
{
    public class BusinessesInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private BusinessesConfig _businessesConfig;
        private ISaveService _saveService;
        private IViewsController _viewsController;

        public BusinessesInitSystem(IViewsController viewsController, BusinessesConfig businessesConfig, ISaveService saveService)
        {
            _viewsController = viewsController;
            _businessesConfig = businessesConfig;
            _saveService = saveService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var businessesPool = world.GetPool<BusinessComponent>();

            var businessView = _viewsController.ShowView<BusinessesView>();

            var businessesAtStart = _businessesConfig.OwnedBusinessesAtStart;
            var savedData = new List<BusinessDataSave>();
            var isSaveExists = _saveService.IsFileExists(SaveFileNames.BUSINESSES_FILE);

            if (isSaveExists)
            {
                savedData = _saveService.Load<List<BusinessDataSave>>(SaveFileNames.BUSINESSES_FILE);
            }

            for (int configIndex = 0; configIndex < _businessesConfig.BusinessConfigs.Length; configIndex++)
            {
                var config = _businessesConfig.BusinessConfigs[configIndex];
                var save = isSaveExists ? savedData[configIndex] : default;
                var businessLevel = configIndex + 1 <= businessesAtStart ? 1 : 0;

                var businessWidget = businessView.AddBusinessWidget();

                businessWidget
                    .SetId(config.Id)
                    .SetName()
                    .SetUpgradesData(config.Upgrades);

                var entity = world.NewEntity();
                ref var businessComponent = ref businessesPool.Add(entity);

                var profitDelay = isSaveExists ? save.ProfitDelay : config.ProfitDelay;
                var baseProfit = isSaveExists ? save.BaseProfit : config.BaseProfit;
                var currentProfitDelay = isSaveExists ? save.CurrentProfitDelay : 0f;
                var currentPrice = isSaveExists ? save.Price : config.BasePrice;
                var currentProfit = isSaveExists ? save.Profit : config.BaseProfit;
                businessLevel = isSaveExists ? save.Level : businessLevel;

                businessComponent.Id = config.Id;
                businessComponent.ProfitDelay = profitDelay;
                businessComponent.BaseProfit = baseProfit;
                businessComponent.View = businessWidget;
                businessComponent.Upgrades = new List<BusinessUpgradeData>();

                businessComponent.CurrentProfitDelay = new ReactiveProperty<float>();
                businessComponent.CurrentLevel = new ReactiveProperty<int>(-1);
                businessComponent.CurrentPrice = new ReactiveProperty<int>();
                businessComponent.CurrentProfit = new ReactiveProperty<float>();

                businessComponent.CurrentProfitDelay.Changed += businessComponent.OnCurrentProfitDelayChanged;
                businessComponent.CurrentLevel.Changed += businessComponent.OnLevelChanged;
                businessComponent.CurrentPrice.Changed += businessComponent.OnCurrentPriceChanged;
                businessComponent.CurrentProfit.Changed += businessComponent.OnCurrentProfitChanged;

                businessComponent.CurrentProfitDelay.Value = currentProfitDelay;
                businessComponent.CurrentLevel.Value = businessLevel;
                businessComponent.CurrentPrice.Value = currentPrice;
                businessComponent.CurrentProfit.Value = currentProfit;

                if (!isSaveExists) continue;

                foreach (var upgrade in save.Upgrades)
                {
                    businessComponent.AddUpgrade(upgrade);
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var businessesFilter = world.Filter<BusinessComponent>().End();
            var businessesComponents = world.GetPool<BusinessComponent>();
            var saveData = new List<BusinessDataSave>();

            foreach (var businessEntity in businessesFilter)
            {
                ref var businessComponent = ref businessesComponents.Get(businessEntity);

                businessComponent.CurrentProfitDelay.Changed -= businessComponent.OnCurrentProfitDelayChanged;
                businessComponent.CurrentLevel.Changed -= businessComponent.OnLevelChanged;
                businessComponent.CurrentPrice.Changed -= businessComponent.OnCurrentPriceChanged;
                businessComponent.CurrentProfit.Changed -= businessComponent.OnCurrentProfitChanged;
            }
        }
    }
}
