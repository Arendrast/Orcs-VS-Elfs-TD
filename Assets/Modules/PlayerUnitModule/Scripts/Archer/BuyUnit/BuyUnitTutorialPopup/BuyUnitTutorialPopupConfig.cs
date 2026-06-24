using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitTutorialPopup
{
    [Serializable]
    public class BuyUnitTutorialPopupConfig
    {
        [field: Space]
        [field: Header("General")]   
        [field: SerializeField]  public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public float CanvasGroupAppearTime { get; private set; } = 1f;
        [field: SerializeField] public float CanvasGroupDisappearTime { get; private set; } = 0.5f;
        
        [field: Header("Buy Button")]
        [field: SerializeField]  public Button BuyButton { get; private set; }
        [field: SerializeField] public float BuyButtonSetScaleDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float BuyButtonMaxScale { get; private set; } = 1f;
        [field: SerializeField] public float BuyButtonMinScale { get; private set; } = 0.5f;
        [field: SerializeField] public TextMeshProUGUI BuyButtonPriceText { get; private set; }

        [field: Space]
        [field: Header("Light Image")]
        [field: SerializeField]
        public float MoveLightImageDuration { get; private set; } = 0.5f;

        [field: SerializeField] public Image LightImage { get; private set; }
        
        [field: Space]
        [field: Header("Hand")]         
        [field: SerializeField] public Transform HandTransform { get; private set; }
        [field: SerializeField] public float MoveAndRotateHandDuration { get; private set; }
        [field: SerializeField] public Transform FirstPointHandPositionTransform { get; private set; }
        [field: SerializeField] public Transform SecondPointHandPositionTransform { get; private set; }
        [field: SerializeField] public Vector3 FirstPointHandRotation { get; private set; }
        [field: SerializeField] public Vector3 SecondPointHandRotation { get; private set; }
    }
}