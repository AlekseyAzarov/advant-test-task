using TMPro;
using UnityEngine;

namespace ClickerLogic
{
    public class PlayerBalanceView : AbstractView, IViewWithTextTermsConfig
    {
        [SerializeField] private TextMeshProUGUI _balanceText;

        private TextTermsConfig _textTermsConfig;

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetBalance(float balance)
        {
            _balanceText.text = $"{_textTermsConfig.GetText(TextTermsConstants.BALANCE_TERM)}: {string.Format("{0:0}", balance)}$";
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetTextTermsConfig(TextTermsConfig textTermsConfig)
        {
            _textTermsConfig = textTermsConfig;
        }
    }
}
