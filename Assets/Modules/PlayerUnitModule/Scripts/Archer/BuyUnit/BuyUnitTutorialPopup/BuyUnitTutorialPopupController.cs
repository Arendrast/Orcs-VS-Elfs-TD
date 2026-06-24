using System;
using System.Collections.Generic;
using DG.Tweening;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts;
using Modules.SharedModule.Scripts.Audio;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitTutorialPopup
{
    public class BuyUnitTutorialPopupController : IDisposable
    {
        public event Action Closed;

        private BuyUnitModel _buyUnitModel;
        private AudioService _audioService;

        private readonly BuyUnitTutorialPopupConfig _config;
        private readonly List<Tween> _tweens = new List<Tween>();
        private readonly GameObject _gameObject;
        private MergeUnitFactory _mergeUnitFactory;
        private BuyMergeUnitConfig _buyMergeUnitConfig;

        public BuyUnitTutorialPopupController(BuyUnitTutorialPopupConfig config, GameObject gameObject)
        {
            _config = config;
            _gameObject = gameObject;
        }

        public void Open(BuyMergeUnitConfig config, BuyUnitModel buyUnitModel, AudioService audioService,
            MergeUnitFactory mergeUnitFactory, BuyMergeUnitConfig buyMergeUnitConfig)
        {
            _audioService = audioService;
            _buyUnitModel = buyUnitModel;
            _mergeUnitFactory = mergeUnitFactory;
            _buyMergeUnitConfig = buyMergeUnitConfig;

            _config.BuyButton.transform.localScale = Vector3.one * _config.BuyButtonMaxScale;

            _config.BuyButton.onClick.AddListener(Close);

            _config.BuyButtonPriceText.text = config.FirstUnitBuyPrice.ToString();

            StartBuyButtonScaleLoopAnimation();
            StartLightMoveLoopAnimation();

            MoveHandTools.StartHandMoveAndRotateLoopAnimation(_config.HandTransform, _config.FirstPointHandPosition,
                _config.SecondPointHandPosition, _config.FirstPointHandRotation, _config.SecondPointHandRotation,
                _config.MoveAndRotateHandDuration, _tweens);

            _config.CanvasGroup.alpha = 0;
            _config.CanvasGroup.DOFade(1, _config.CanvasGroupAppearTime).SetUpdate(true);

            _gameObject.SetActive(true);
        }

        private void StartBuyButtonScaleLoopAnimation()
        {
            _tweens.Add(_config.BuyButton.transform
                .DOScale(_config.BuyButtonMinScale, _config.BuyButtonSetScaleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true));
        }

        private void StartLightMoveLoopAnimation()
        {
            _tweens.Add(_config.LightImage.transform.DOLocalMoveX(
                    -_config.LightImage.transform.localPosition.x * 3f,
                    _config.MoveLightImageDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true));
        }

        private void Close()
        {
            _buyUnitModel.TryBuyUnit(out var mergeCellModel);
            _audioService.TryPlayOneShotForMainAudioSource(AudioId.UnitBuy);
            
            mergeCellModel.TargetUnit = _mergeUnitFactory.GetMergeUnitModel(_buyMergeUnitConfig.SpawnUnitPrefab,
                mergeCellModel.MergeCellComponent.PositionTransform.position);

            _config.CanvasGroup.DOFade(0, _config.CanvasGroupDisappearTime).SetUpdate(true).OnComplete(OnClose);
        }

        private void OnClose()
        {
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