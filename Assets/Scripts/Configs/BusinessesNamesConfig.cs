using UnityEngine;

namespace ClickerLogic
{
    [CreateAssetMenu(fileName = "Businesses Names Config", menuName = "Configs/Businesses Names Config")]
    public class BusinessesNamesConfig : ScriptableObject
    {
        [SerializeField] private BusinessNameConfigData[] _businessesNamesConfigs;

        public BusinessNameConfigData[] BusinessesNamesConfigs => _businessesNamesConfigs;
    }
}
