namespace ClickerLogic
{
    public interface IViewsController
    {
        T ShowView<T>() where T : class, IView;
        void HideView<T>() where T : class, IView;
    }
}
