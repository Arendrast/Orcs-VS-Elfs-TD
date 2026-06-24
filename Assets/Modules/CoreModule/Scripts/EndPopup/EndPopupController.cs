using System;
using System.Collections.Generic;
using DG.Tweening;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.CoreModule.Scripts.EndPopup
{
    public class EndPopupController : IDisposable
    {
        private readonly List<Tween> _tweens = new List<Tween>();
        private readonly EndPopupConfig _config;
        private readonly GameObject _gameObject;

        public EndPopupController(EndPopupConfig config, GameObject gameObject)
        {
            _config = config;
            _gameObject = gameObject;
        }

        public void Open(TimeScaleRepositoryService timeScaleRepositoryService, bool isWin)
        {
            _gameObject.SetActive(true);
            
            timeScaleRepositoryService.SetTimeScale(0);

            _config.CanvasGroup.alpha = 0;
            _config.CanvasGroup.DOFade(1, _config.AppearDuration).SetUpdate(true);

            MoveHandTools.StartHandRotateLoopAnimation(_config.HandTransform,
                _config.StartHandRotation, _config.EndHandRotation,
                _config.RotateHandDuration, _tweens);

            _tweens.Add(_config.ButtonTransform.DOScale(_config.ButtonMinimalScale, _config.SetButtonScaleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true));

            foreach (var endText in _config.EndTexts)
            {
               endText.text = isWin ? _config.WinText : _config.DefeatText;
            }
            
            _config.ButtonText.text = isWin ? _config.NextLevelText : _config.RepeatText;
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