using System.Runtime.Serialization;

namespace ClickerLogic
{
    [DataContract]
    public struct PlayerBalanceDataSave
    {
        [DataMember] public float Balance;
    }
}
