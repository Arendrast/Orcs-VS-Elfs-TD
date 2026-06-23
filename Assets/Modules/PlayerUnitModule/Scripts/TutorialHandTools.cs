using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts
{
    public static class TutorialHandTools
    {
        public static void StartHandMoveLoopAnimation(Transform hand, Vector3 startPoint, Vector3 endPoint, Vector3 startRotation,
            Vector3 endRotation, float duration, List<Tween> tweens)
        {
            hand.position = startPoint;

            tweens.Add(hand.DOMove(endPoint,
                duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true));

            hand.rotation = Quaternion.Euler(startRotation);

            tweens.Add(hand.DORotate(endRotation,
                duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true));
        }
    }
}