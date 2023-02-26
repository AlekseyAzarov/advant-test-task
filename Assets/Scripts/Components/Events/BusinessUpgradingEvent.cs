using SevenBoldPencil.EasyEvents;

namespace ClickerLogic
{
    public struct BusinessUpgradingEvent : IEventReplicant
    {
        public string BusinessId;
        public string UpgradeId;
    }
}
