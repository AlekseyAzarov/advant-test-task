using SevenBoldPencil.EasyEvents;
using System.Linq;
using UnityEngine;

namespace ClickerLogic
{
    public class ViewFactory : IViewFactory
    {
        private AbstractView[] _viewsPrefabs;
        private EventsBus _eventsBus;

        public ViewFactory(AbstractView[] viewsPrefabs, EventsBus eventsBus)
        {
            _viewsPrefabs = viewsPrefabs;
            _eventsBus = eventsBus;
        }

        public T CreateView<T>(Transform parent) where T : class, IView
        {
            var prefab = _viewsPrefabs.First(x => x.GetType() == typeof(T));
            var spawnedPrefab = Object.Instantiate(prefab, parent);
            spawnedPrefab.SetEventsBus(_eventsBus);
            spawnedPrefab.gameObject.SetActive(false);
            return spawnedPrefab.GetComponent<T>();
        }
    }
}
