using Leopotam.EcsLite;

namespace ClickerLogic
{
    public class PlayerBalanceInitSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private IViewsController _viewsController;
        private ISaveService _saveService;

        public PlayerBalanceInitSystem(IViewsController viewsController, ISaveService saveService)
        {
            _viewsController = viewsController;
            _saveService = saveService;
        }

        public void Init(IEcsSystems systems)
        {
            var playerBalance = 0f;

            if (_saveService.IsFileExists(SaveFileNames.PLAYER_BALANCE))
            {
                var savedData = _saveService.Load<PlayerBalanceDataSave>(SaveFileNames.PLAYER_BALANCE);
                playerBalance = savedData.Balance;
            }

            var world = systems.GetWorld();
            var balancePool = world.GetPool<PlayerBalanceComponent>();

            var balanceEntity = world.NewEntity();
            ref var balanceComponent = ref balancePool.Add(balanceEntity);

            var balanceView = _viewsController.ShowView<PlayerBalanceView>();
            balanceComponent.View = balanceView;
            balanceComponent.Balance = new ReactiveProperty<float>(-1f);
            balanceComponent.Balance.Changed += balanceComponent.OnBalanceChanged;
            balanceComponent.Balance.Value = playerBalance;
        }

        public void Destroy(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerBalanceFilter = world.Filter<PlayerBalanceComponent>().End();
            var balancePool = world.GetPool<PlayerBalanceComponent>();

            foreach (var playerBalanceEntity in playerBalanceFilter)
            {
                ref var balanceComponent = ref balancePool.Get(playerBalanceEntity);

                balanceComponent.Balance.Changed -= balanceComponent.OnBalanceChanged;

                var savedData = new PlayerBalanceDataSave
                {
                    Balance = balanceComponent.Balance.Value
                };

                _saveService.Save(savedData, SaveFileNames.PLAYER_BALANCE);
            }
        }
    }
}
