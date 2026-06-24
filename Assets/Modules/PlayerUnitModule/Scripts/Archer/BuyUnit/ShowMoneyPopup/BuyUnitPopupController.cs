using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Currencies;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup
{
    public class BuyUnitPopupController : IDisposable
    {
        private bool _isButtonInactive = true;

        private Tween _tween;
        private readonly BuyUnitPopupConfig _config;
        private readonly BuyUnitModel _model;
        private readonly MergeUnitFactory _mergeUnitFactory;
        private readonly BuyMergeUnitConfig _buyMergeUnitConfig;
        private readonly AudioService _audioService;
        private readonly CurrencyRepositoryService _currencyRepositoryService;

        private bool _canBuyUnit = true;
        
        public BuyUnitPopupController(BuyUnitPopupConfig config, BuyUnitModel model, MergeUnitFactory mergeUnitFactory,
            BuyMergeUnitConfig buyMergeUnitConfig, AudioService audioService,
            CurrencyRepositoryService currencyRepositoryService)
        {
            _config = config;
            _model = model;
            _mergeUnitFactory = mergeUnitFactory;
            _buyMergeUnitConfig = buyMergeUnitConfig;
            _audioService = audioService;
            _currencyRepositoryService = currencyRepositoryService;

            currencyRepositoryService.UpdatedCurrencyNumber += UpdateActiveStateVisual;
            
            EnableBuyButton();
        }
        
        public void Dispose()
        {
            _tween?.Kill();
        }

        public void SetCanBuyUnit(bool canBuyUnit)
        {
            _canBuyUnit = canBuyUnit;
            UpdateActiveStateVisual(_currencyRepositoryService.CurrencyNumber);
        }

        private void EnableBuyButton()
        {
            _config.BuyButton.gameObject.SetActive(true);
            _config.BuyButton.onClick.AddListener(TryBuyUnit);

            UpdateBuyButtonPriceText();
        }
        
        private void UpdateBuyButtonPriceText()
        {
            _config.BuyPriceText.text = _model.CurrentBuyPrice.ToString();
            UpdateActiveStateVisual(_currencyRepositoryService.CurrencyNumber);
        }

        private void UpdateActiveStateVisual(int currencyNumber)
        {
            if (_canBuyUnit && currencyNumber >= _model.CurrentBuyPrice)
            {
                if (_isButtonInactive)
                {
                    _tween = _config.BuyButton.transform
                        .DOScale(_config.AnimationSmallButtonScale, _config.SetScaleDuration)
                        .SetEase(Ease.InOutSine)
                        .SetLoops(-1, LoopType.Yoyo);

                    SetActiveStateVisual(true);
                }
                
                return;
            }

            TryDeactivate();
        }

        private void TryDeactivate()
        {
            if (!_isButtonInactive)
            {
                _tween?.Kill();
                SetActiveStateVisual(false);
            }
        }

        private void SetActiveStateVisual(bool active)
        {
            _config.BuyButton.transform.localScale = Vector3.one;
            
            _config.BuyButton.image.color = active ? _config.ActiveBuyButtonColor : _config.InactiveBuyButtonColor;
            _config.BuyPriceText.color = active ? _config.ActiveBuyPriceTextColor : _config.InactiveBuyPriceTextColor;
            _isButtonInactive = !active;
        }

        private void TryBuyUnit()
        {
            _audioService.TryPlayOneShotForMainAudioSource(AudioId.UnitBuy);

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