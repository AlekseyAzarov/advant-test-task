using System;

namespace ClickerLogic
{
    [Serializable]
    public struct BusinessNameConfigData
    {
        public string Id;
        public string Name;
        public BusinessUpgradeNameConfigData[] Upgrades;
    }
}
