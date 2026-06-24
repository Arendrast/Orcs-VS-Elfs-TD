using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup
{
    [Serializable]
    public class BuyUnitPopupConfig
    {
        [field: SerializeField] public Button BuyButton { get; set; }
        [field: SerializeField] public Color InactiveBuyButtonColor { get; private set; }
        [field: SerializeField] public Color ActiveBuyButtonColor { get; private set; }
        [field: SerializeField] public Color InactiveBuyPriceTextColor { get; private set; }
        [field: SerializeField] public Color ActiveBuyPriceTextColor { get; private set; }
        [field: SerializeField] public Vector3 AnimationSmallButtonScale { get; set; }
        [field: SerializeField] public float SetScaleDuration { get; set; } = 1f;
        [field: SerializeField] public TextMeshProUGUI BuyPriceText { get; set; }
    }
}