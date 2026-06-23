using System;
using System.Linq;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup
{
    public class BuyUnitModel
    {
        public int CurrentBuyPrice { get; private set; }

        public event Action BoughtUnit;
        
        private int _boughtUnitsCount;

        private readonly BuyMergeUnitConfig _priceConfig;
        private readonly CurrencyRepositoryService _currencyRepositoryService;
        private readonly MergeGridModel _mergeGridModel;

        public BuyUnitModel(BuyMergeUnitConfig priceConfig, CurrencyRepositoryService currencyRepositoryService,
            MergeGridModel mergeGridModel)
        {
            _priceConfig = priceConfig;
            _currencyRepositoryService = currencyRepositoryService;
            _mergeGridModel = mergeGridModel;

            CurrentBuyPrice = _priceConfig.FirstUnitBuyPrice;
        }

        public bool TryBuyUnit(out MergeCellModel mergeCellModel)
        {
            mergeCellModel = null;
            
            if (!_currencyRepositoryService.MakeOperationOnCurrencyNumber(CurrentBuyPrice,
                    CurrencyRepositoryService.SetCurrencyOperation.Decrease))
            {
                return false;
            }

            mergeCellModel = _mergeGridModel.Cells.FirstOrDefault(cell => !cell.HasTargetUnit());

            if (mergeCellModel == null)
            {
                return false;
            }

            if (_boughtUnitsCount == 1)
            {
                CurrentBuyPrice = _priceConfig.SecondUnitBuyPrice;
            }
            else
            {
                CurrentBuyPrice = _priceConfig.SecondUnitBuyPrice +
                               (_boughtUnitsCount - 1) * _priceConfig.PriceIncrementValueAfterEveryBuy;
            }
            
            BoughtUnit?.Invoke();

            return true;
        }
    }
}