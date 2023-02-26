using UnityEngine;

namespace ClickerLogic
{
    [CreateAssetMenu(fileName = "Views Prefabs Container", menuName = "Views/Views Prefabs Container")]
    public class ViewsPrefabsContainer : ScriptableObject
    {
        [SerializeField] private AbstractView[] _viewsPrefabs;

        public AbstractView[] ViewsPrefabs => _viewsPrefabs;
    }
}
