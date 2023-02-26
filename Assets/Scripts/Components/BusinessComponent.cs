using System.Collections.Generic;

namespace ClickerLogic
{
    public struct BusinessComponent
    {
        public BusinessUiWidget View;

        public string Id;
        public int ProfitDelay;
        public int BaseProfit;
        public List<BusinessUpgradeData> Upgrades;

        public ReactiveProperty<float> CurrentProfitDelay;
        public ReactiveProperty<float> CurrentProfit;
        public ReactiveProperty<int> CurrentLevel;
        public ReactiveProperty<int> CurrentPrice;

        public void AddUpgrade(BusinessUpgradeData businessUpgradeData)
        {
            Upgrades.Add(businessUpgradeData);
            View.SetUpgradeAsBought(businessUpgradeData.Id);
        }

        public void OnLevelChanged(int level)
        {
            View.SetLevel(level);
        }

        public void OnCurrentProfitDelayChanged(float currentProfitDelay)
        {
            View.SetProfitProgress(currentProfitDelay, ProfitDelay);
        }

        public void OnCurrentPriceChanged(int currentPrice)
        {
            View.SetLevelUpPrice(currentPrice);
        }

        public void OnCurrentProfitChanged(float currentProfit)
        {
            View.SetProfit(currentProfit);
        }
    }
}
