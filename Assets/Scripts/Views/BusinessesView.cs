using UnityEngine;

namespace ClickerLogic
{
    public class BusinessesView : AbstractView, IViewWithNamesConfig, IViewWithTextTermsConfig
    {
        [SerializeField] private BusinessUiWidget _businessWidgetPrefab;
        [SerializeField] private Transform _widgetsParent;

        private BusinessesNamesConfig _businessesNamesConfig;
        private TextTermsConfig _textTermsConfig;

        public BusinessUiWidget AddBusinessWidget()
        {
            var widget = Instantiate(_businessWidgetPrefab, _widgetsParent);
            widget.SetBusinessesNamesConfig(_businessesNamesConfig);
            widget.SetTextTermsConfig(_textTermsConfig);
            return widget;
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetNamesConfig(BusinessesNamesConfig businessesNamesConfig)
        {
            _businessesNamesConfig = businessesNamesConfig;
        }

        public void SetTextTermsConfig(TextTermsConfig textTermsConfig)
        {
            _textTermsConfig = textTermsConfig;
        }
    }
}
