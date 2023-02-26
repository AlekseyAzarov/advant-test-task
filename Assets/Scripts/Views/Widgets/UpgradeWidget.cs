using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickerLogic
{
    public class UpgradeWidget : MonoBehaviour
    {
        public event Action<string> UpgradeClicked;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _bonusDescriptionText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _upgrgadeButton;

        private TextTermsConfig _textTermsConfig;
        private string _upgradeId;

        public string UpgradeId => _upgradeId;

        private void Start()
        {
            _upgrgadeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void OnDestroy()
        {
            _upgrgadeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        }

        public UpgradeWidget SetId(string id)
        {
            _upgradeId = id;
            return this;
        }

        public UpgradeWidget SetName(string name)
        {
            _nameText.text = name;
            return this;
        }

        public UpgradeWidget SetBonusValue(float value)
        {
            _bonusDescriptionText.text = $"{_textTermsConfig.GetText(TextTermsConstants.PROFIT_TERM)}: {value * 100}%";
            return this;
        }

        public UpgradeWidget SetPrice(int price)
        {
            _priceText.text = $"{_textTermsConfig.GetText(TextTermsConstants.PRICE_TERM)}: {price}$";
            return this;
        }

        public UpgradeWidget SetBoughtText()
        {
            _priceText.text = $"{_textTermsConfig.GetText(TextTermsConstants.BOUGHT_TERM)}";
            return this;
        }

        public UpgradeWidget SetTextTermsConfig(TextTermsConfig textTermsConfig)
        {
            _textTermsConfig = textTermsConfig;
            return this;
        }

        private void OnUpgradeButtonClicked()
        {
            UpgradeClicked?.Invoke(_upgradeId);
        }
    }
}
