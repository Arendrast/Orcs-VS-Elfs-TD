using System;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge.MergeUnitTutorialPopup
{
    [Serializable]
    public class MergeUnitTutorialPopupConfig
    {
        [field: SerializeField] public Transform HandTransform { get; private set; }
        [field: SerializeField] public float MoveAndRotateHandDuration { get; private set; }
        [field: SerializeField] public Vector3 OnFirstUnitHandRotation { get; private set; }
        [field: SerializeField] public Vector3 OnSecondUnitHandRotation { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public float AppearDisappearDuration { get; private set; } = 0.5f;
    }
}