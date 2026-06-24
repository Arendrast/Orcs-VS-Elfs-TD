using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Modules.SharedModule.Scripts
{
    public static class MoveHandTools
    {
        public static void StartHandMoveAndRotateLoopAnimation(Transform hand, Vector3 startPoint, Vector3 endPoint, Vector3 startRotation,
            Vector3 endRotation, float duration, List<Tween> tweens, Ease ease = Ease.InOutSine)
        {
            hand.position = startPoint;

            tweens.Add(hand.DOMove(endPoint,
                duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo).SetUpdate(true));

            StartHandRotateLoopAnimation(hand, startRotation, endRotation, duration, tweens, ease);
        }

        public static void StartHandRotateLoopAnimation(Transform hand, Vector3 startRotation, Vector3 endRotation,
            float duration, List<Tween> tweens, Ease ease = Ease.InOutSine)
        {
            hand.rotation = Quaternion.Euler(startRotation);

            tweens.Add(hand.DORotate(endRotation,
                duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo).SetUpdate(true));
        }
    }
}