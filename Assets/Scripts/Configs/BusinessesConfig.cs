using UnityEngine;

namespace ClickerLogic
{
    [CreateAssetMenu(fileName = "Businesses Config", menuName = "Configs/Businesses Config")]
    public class BusinessesConfig : ScriptableObject
    {
        [SerializeField] private BusinessConfigData[] _businessConfigs;
        [SerializeField] private int _ownedBusinessesAtStart;

        public BusinessConfigData[] BusinessConfigs => _businessConfigs;
        public int OwnedBusinessesAtStart => _ownedBusinessesAtStart;
    }
}
