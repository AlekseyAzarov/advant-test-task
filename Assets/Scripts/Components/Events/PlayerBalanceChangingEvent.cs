using SevenBoldPencil.EasyEvents;

namespace ClickerLogic
{
    public struct PlayerBalanceChangingEvent : IEventReplicant
    {
        public float Amount;
    }
}
