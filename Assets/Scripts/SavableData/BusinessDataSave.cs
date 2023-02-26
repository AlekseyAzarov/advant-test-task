using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ClickerLogic
{
    [DataContract]
    public struct BusinessDataSave
    {
        [DataMember] public string Id;
        [DataMember] public float CurrentProfitDelay;
        [DataMember] public float Profit;
        [DataMember] public int Price;
        [DataMember] public int Level;
        [DataMember] public int ProfitDelay;
        [DataMember] public int BaseProfit;
        [DataMember] public List<BusinessUpgradeData> Upgrades;
    }
}
