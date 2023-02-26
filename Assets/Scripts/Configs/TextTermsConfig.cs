using System.Linq;
using UnityEngine;

namespace ClickerLogic
{
    [CreateAssetMenu(fileName = "Text Terms Config", menuName = "Configs/Text Terms Config")]
    public class TextTermsConfig : ScriptableObject
    {
        [SerializeField] private TextTermData[] _termsData;

        public string GetText(string term)
        {
            var termData = _termsData.First(x => x.Term == term);
            return termData.Text;
        }
    }
}
