using System;
using System.Collections.Generic;
using DG.Tweening;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitTutorialPopup
{
    public class BuyUnitTutorialPopupController : IDisposable
    {
        public event Action Closed;

        private BuyUnitModel _buyUnitModel;
        
        private readonly BuyUnitTutorialPopupConfig _config;
        private readonly List<Tween> _tweens = new List<Tween>();
        private readonly GameObject _gameObject;

        public BuyUnitTutorialPopupController(BuyUnitTutorialPopupConfig config, GameObject gameObject)
        {
            _config = config;
            _gameObject = gameObject;
        }

        public void Open(BuyMergeUnitConfig config, BuyUnitModel buyUnitModel)
        {
            _buyUnitModel = buyUnitModel;
            
            _config.BuyButton.transform.localScale = Vector3.one * _config.BuyButtonMaxScale;

            _config.BuyButton.onClick.AddListener(Close);
            
            _config.BuyButtonPriceText.text = config.FirstUnitBuyPrice.ToString();
            
            StartBuyButtonScaleLoopAnimation();
            StartLightMoveLoopAnimation();
            
            TutorialHandTools.StartHandMoveLoopAnimation(_config.HandTransform, _config.FirstPointHandPosition,
                _config.SecondPointHandPosition, _config.FirstPointHandRotation, _config.SecondPointHandRotation, 
                _config.MoveAndRotateHandDuration, _tweens);
        }

        private void StartBuyButtonScaleLoopAnimation()
        {
            _tweens.Add(_config.BuyButton.transform.DOScale(_config.BuyButtonMinScale, _config.BuyButtonSetScaleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true));
        }

        private void StartLightMoveLoopAnimation()
        {
            _tweens.Add(_config.LightImage.transform.DOLocalMoveX(-_config.LightImage.transform.localPosition.x,
                    _config.MoveLightImageDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true));
        }

        private void Close()
        {
            _buyUnitModel.TryBuyUnit(out var mergeCellModel);
            _gameObject.SetActive(false);
            Closed?.Invoke();
        }

        public void Dispose()
        {
            foreach (var tween in _tweens)
            {
                tween?.Kill();
            }
        }
    }
}