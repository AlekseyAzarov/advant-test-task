using UnityEngine;

namespace ClickerLogic
{
    public interface IViewFactory
    {
        T CreateView<T>(Transform parent) where T : class, IView;
    }
}
