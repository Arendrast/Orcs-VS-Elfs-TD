using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.CoreModule.Scripts.EndPopup
{
    [Serializable]
    public class EndPopupConfig
    {
        [field: Header("General")]
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public float AppearDuration { get; private set; } = 0.5f;
        
        [field: Space]
        [field: Header("Button")]
        [field: SerializeField] public float SetButtonScaleDuration { get; private set; } = 0.75f;
        [field: SerializeField] public float ButtonMinimalScale { get; private set; } = 0.85f;
        [field: SerializeField] public Transform ButtonTransform { get; private set; }
        
        [field: Space]
        [field: Header("Hand")]
        [field: SerializeField] public Vector3 StartHandRotation { get; private set; }
        [field: SerializeField] public Vector3 EndHandRotation { get; private set; }
        [field: SerializeField] public float RotateHandDuration { get; private set; }
        [field: SerializeField] public Transform HandTransform { get; private set; }

        [field: Space]
        [field: Header("Texts")]
        [field: SerializeField] public TextMeshProUGUI[] EndTexts { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ButtonText { get; private set; }
        [field: SerializeField] public string WinText { get; private set; } = "Victory";
        [field: SerializeField] public string DefeatText { get; private set; } = "Defeat";
        [field: SerializeField] public string RepeatText { get; private set; } = "Repeat";
        [field: SerializeField] public string NextLevelText { get; private set; } = "Next Level";
    }
}