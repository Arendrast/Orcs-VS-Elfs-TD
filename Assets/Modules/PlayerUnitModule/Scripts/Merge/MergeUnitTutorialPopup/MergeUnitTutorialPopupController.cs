using System;
using System.Collections.Generic;
using DG.Tweening;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge.MergeUnitTutorialPopup
{
    public class MergeUnitTutorialPopupController : IDisposable
    {
        private readonly MergeUnitTutorialPopupConfig _config;
        private readonly GameObject _popup;

        private readonly List<Tween> _tweens = new List<Tween>();

        public MergeUnitTutorialPopupController(MergeUnitTutorialPopupConfig config, GameObject popup)
        {
            _config = config;
            _popup = popup;
        }

        public void Open(MergeUnitComponent firstUnit, MergeUnitComponent secondUnit, Camera camera)
        {
            MoveHandTools.StartHandMoveAndRotateLoopAnimation(_config.HandTransform, camera.WorldToScreenPoint(firstUnit.transform.position),
                camera.WorldToScreenPoint(secondUnit.transform.position), _config.OnFirstUnitHandRotation, _config.OnSecondUnitHandRotation, 
                _config.MoveAndRotateHandDuration, _tweens);
            
            _popup.SetActive(true);

            _config.CanvasGroup.alpha = 0;
            _config.CanvasGroup.DOFade(1, _config.AppearDisappearDuration).SetUpdate(true);
        }

        public void Close()
        {
            _config.CanvasGroup.DOFade(0, _config.AppearDisappearDuration).SetUpdate(true).OnComplete(Deactivate);
        }

        public void Dispose()
        {
            foreach (var tween in _tweens)
            {
                tween?.Kill();
            }
        }

        private void Deactivate()
        {
            _popup.SetActive(false);
        }
    }
}