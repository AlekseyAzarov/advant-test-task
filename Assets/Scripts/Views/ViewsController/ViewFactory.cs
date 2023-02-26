using System.Linq;
using UnityEngine;

namespace ClickerLogic
{
    public class ViewFactory : IViewFactory
    {
        private AbstractView[] _viewsPrefabs;

        public ViewFactory(AbstractView[] viewsPrefabs)
        {
            _viewsPrefabs = viewsPrefabs;
        }

        public T CreateView<T>(Transform parent) where T : class, IView
        {
            var prefab = _viewsPrefabs.First(x => x.GetType() == typeof(T));
            var spawnedPrefab = Object.Instantiate(prefab, parent);
            spawnedPrefab.gameObject.SetActive(false);
            return spawnedPrefab.GetComponent<T>();
        }
    }
}
