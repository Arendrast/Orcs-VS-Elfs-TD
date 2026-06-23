using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup
{
    public class BuyUnitPopupController
    {
        private int _boughtUnitsCount;

        private readonly BuyUnitPopupConfig _config;
        private readonly BuyUnitModel _model;
        private readonly MergeUnitFactory _mergeUnitFactory;
        private readonly BuyMergeUnitConfig _buyMergeUnitConfig;

        public BuyUnitPopupController(BuyUnitPopupConfig config, BuyUnitModel model, MergeUnitFactory mergeUnitFactory,
            BuyMergeUnitConfig buyMergeUnitConfig)
        {
            _config = config;
            _model = model;
            _mergeUnitFactory = mergeUnitFactory;
            _buyMergeUnitConfig = buyMergeUnitConfig;
            
            _config.BuyButton.gameObject.SetActive(false);
        }

        public void EnableBuyButton()
        {
            _config.BuyButton.gameObject.SetActive(true);
            _config.BuyButton.onClick.AddListener(TryBuyUnit);

            UpdateBuyButtonPriceText();
        }

        private void UpdateBuyButtonPriceText()
        {
            _config.BuyPriceText.text = _model.CurrentBuyPrice.ToString();
        }

        private void TryBuyUnit()
        {
            if (!_model.TryBuyUnit(out var mergeCellModel))
            {
                return;
            }

            mergeCellModel.TargetUnit = _mergeUnitFactory.GetMergeUnitModel(_buyMergeUnitConfig.SpawnUnitPrefab,
                mergeCellModel.MergeCellComponent.PositionTransform.position);

            UpdateBuyButtonPriceText();
        }
    }
}