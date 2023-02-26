using UnityEngine;

namespace ClickerLogic
{
    public abstract class AbstractView : MonoBehaviour, IView
    {
        public abstract void Hide();

        public abstract void Show();
    }
}
