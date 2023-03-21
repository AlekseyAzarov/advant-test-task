using SevenBoldPencil.EasyEvents;
using UnityEngine;

namespace ClickerLogic
{
    public abstract class AbstractView : MonoBehaviour, IView
    {
        protected EventsBus EventsBus;

        public abstract void Hide();

        public abstract void Show();

        public void SetEventsBus(EventsBus eventsBus)
        {
            EventsBus = eventsBus;
        }
    }
}
