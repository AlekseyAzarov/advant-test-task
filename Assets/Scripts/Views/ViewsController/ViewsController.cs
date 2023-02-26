using System.Collections.Generic;
using UnityEngine;

namespace ClickerLogic
{
    public class ViewsController : MonoBehaviour, IViewsController
    {
        [SerializeField] private BusinessesNamesConfig _businessNameConfigData;
        [SerializeField] private TextTermsConfig _textTermsConfig;

        private IViewFactory _viewFactory;
        private List<IView> _spawnedViews = new List<IView>();

        public T ShowView<T>() where T : class, IView
        {
            var spawnedView = _spawnedViews.Find(x => x.GetType() == typeof(T));

            if (spawnedView == null)
            {
                spawnedView = _viewFactory.CreateView<T>(transform);

                var viewWithNamesConfig = spawnedView as IViewWithNamesConfig;
                var viewWithTextTermsConfig = spawnedView as IViewWithTextTermsConfig;

                viewWithNamesConfig?.SetNamesConfig(_businessNameConfigData);
                viewWithTextTermsConfig?.SetTextTermsConfig(_textTermsConfig);

                _spawnedViews.Add(spawnedView);
            }

            spawnedView.Show();
            return (T)spawnedView;
        }

        public void HideView<T>() where T : class, IView
        {
            var spawnedView = _spawnedViews.Find(x => x.GetType() == typeof(T));

            if (spawnedView == null)
            {
                Debug.LogError($"Trying to hide view that wasn't shown. Type: {typeof(T)}");
            }

            spawnedView.Hide();
        }

        public void SetViewsFactory(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }
    }
}
