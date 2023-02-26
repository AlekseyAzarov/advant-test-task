using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClickerLogic
{
    public class BusinessUiWidget : MonoBehaviour
    {
        public event Action<string> LevelUpClicked;
        public event Action<string, string> UpgradeClicked;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Slider _profitProgress;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _profitText;
        [SerializeField] private TextMeshProUGUI _levelUpPriceText;
        [SerializeField] private Transform _upgradeWidgetsParent;
        [SerializeField] private UpgradeWidget _upgradeWidgetPrefab;
        [SerializeField] private Button _levelUpButton;

        private List<UpgradeWidget> _upgradeWidgets = new List<UpgradeWidget>();
        private BusinessesNamesConfig _businessesNamesConfig;
        private TextTermsConfig _textTermsConfig;
        private string _businessId;

        private void Start()
        {
            _levelUpButton.onClick.AddListener(OnLevelUpButtonClicked);
        }

        private void OnDestroy()
        {
            _levelUpButton.onClick.RemoveListener(OnLevelUpButtonClicked);

            foreach (var upgradeWidget in _upgradeWidgets)
            {
                upgradeWidget.UpgradeClicked -= OnUpgradeClicked;
            }
        }

        public BusinessUiWidget SetBusinessesNamesConfig(BusinessesNamesConfig businessesNamesConfig)
        {
            _businessesNamesConfig = businessesNamesConfig;
            return this;
        }

        public BusinessUiWidget SetTextTermsConfig(TextTermsConfig textTermsConfig)
        {
            _textTermsConfig = textTermsConfig;
            return this;
        }

        public BusinessUiWidget SetId(string id)
        {
            _businessId = id;
            return this;
        }

        public BusinessUiWidget SetName(string name = "")
        {
            var nameFromConfig = _businessesNamesConfig
                .BusinessesNamesConfigs
                .First(x => x.Id == _businessId)
                .Name;
            _nameText.text = name == string.Empty ? nameFromConfig : name;
            return this;
        }

        public BusinessUiWidget SetProfitProgress(float progress, float maxProgress)
        {
            var normalizedProgress = progress / maxProgress;
            _profitProgress.value = normalizedProgress;
            return this;
        }

        public BusinessUiWidget SetLevel(int level)
        {
            _levelText.text = $"{_textTermsConfig.GetText(TextTermsConstants.LEVEL_TERM)}\n{level}";
            return this;
        }

        public BusinessUiWidget SetProfit(float value)
        {
            _profitText.text = $"{_textTermsConfig.GetText(TextTermsConstants.PROFIT_TERM)}\n{string.Format("{0:0}", value)}$";
            return this;
        }

        public BusinessUiWidget SetLevelUpPrice(int price)
        {
            _levelUpPriceText.text = $"{_textTermsConfig.GetText(TextTermsConstants.PRICE_TERM)}: {price}";
            return this;
        }

        public BusinessUiWidget SetUpgradesData(BusinessUpgradeData[] upgrades)
        {
            foreach (var upgrade in upgrades)
            {
                var upgradeWidget = Instantiate(_upgradeWidgetPrefab, _upgradeWidgetsParent);

                var upgradeName = _businessesNamesConfig
                .BusinessesNamesConfigs
                .First(x => x.Id == _businessId)
                .Upgrades
                .First(x => x.Id == upgrade.Id)
                .Name;

                upgradeWidget
                    .SetId(upgrade.Id)
                    .SetName(upgradeName)
                    .SetTextTermsConfig(_textTermsConfig)
                    .SetBonusValue(upgrade.BaseProfitMultiplier)
                    .SetPrice(upgrade.BasePrice);

                upgradeWidget.UpgradeClicked += OnUpgradeClicked;

                _upgradeWidgets.Add(upgradeWidget);
            }

            return this;
        }

        public BusinessUiWidget SetUpgradeAsBought(string upgradeId)
        {
            var upgradeUi = _upgradeWidgets.First(x => x.UpgradeId == upgradeId);
            upgradeUi.SetBoughtText();

            return this;
        }

        private void OnLevelUpButtonClicked()
        {
            LevelUpClicked?.Invoke(_businessId);
        }

        private void OnUpgradeClicked(string upgradeId)
        {
            UpgradeClicked?.Invoke(_businessId, upgradeId);
        }
    }
}
