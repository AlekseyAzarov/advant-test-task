using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using System.Collections.Generic;
using UnityEngine;

namespace ClickerLogic
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private ViewsPrefabsContainer _viewsPrefabsContainer;
        [SerializeField] private ViewsController _viewsController;
        [SerializeField] private BusinessesConfig _businessesConfig;
        [SerializeField] private ApplicationFocusHandler _applicationFocusHandler;

        private EcsSystems _systems;

        private void Start()
        {
            ConfigureViewsController();

            var world = new EcsWorld();
            var eventBus = new EventsBus();
            var sharedData = new SharedData(eventBus);

            _systems = new EcsSystems(world, sharedData);

            AddSystems(_systems);
            AddSelfDestroyingEvents(_systems, eventBus);

            _systems.Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems?.GetWorld()?.Destroy();
            _systems = null;
        }

        private void ConfigureViewsController()
        {
            var views = _viewsPrefabsContainer.ViewsPrefabs;
            var viewsFactory = new ViewFactory(views);
            _viewsController.SetViewsFactory(viewsFactory);
        }

        private void AddSystems(EcsSystems ecsSystems)
        {
            var saveService = new SaveService();

            var systems = new List<IEcsSystem>
            {
                new PlayerBalanceInitSystem(_viewsController, saveService),
                new BusinessesInitSystem(_viewsController, _businessesConfig, saveService),
                new BusinessUpgradeSystem(_businessesConfig),
                new BusinessBuySystem(),
                new BusinessProfitSystem(),
                new PlayerResourcesSystem(),
                new SaveSystem(saveService, _applicationFocusHandler)
            };

            foreach (var system in systems) ecsSystems.Add(system);
        }

        private void AddSelfDestroyingEvents(EcsSystems ecsSystems, EventsBus eventsBus)
        {
            ecsSystems.Add(eventsBus.GetDestroyEventsSystem()
                .IncReplicant<PlayerBalanceChangingEvent>()
                .IncReplicant<BusinessLevelUpEvent>()
                .IncReplicant<BusinessUpgradingEvent>());
        }
    }
}
