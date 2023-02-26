using System;

namespace ClickerLogic
{
    [Serializable]
    public struct PlayerBalanceComponent
    {
        public PlayerBalanceView View;

        public ReactiveProperty<float> Balance;

        public void OnBalanceChanged(float value)
        {
            Balance.Value = Math.Clamp(value, 0, int.MaxValue);
            View.SetBalance(value);
        }
    }
}
