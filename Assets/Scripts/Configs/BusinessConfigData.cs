using System;

namespace ClickerLogic
{
    [Serializable]
    public struct BusinessConfigData
    {
        public string Id;
        public int ProfitDelay;
        public int BasePrice;
        public int BaseProfit;
        public BusinessUpgradeData[] Upgrades;
    }
}
