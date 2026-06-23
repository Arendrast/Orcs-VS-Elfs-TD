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
        [field: SerializeField] public TextMeshProUGUI BuyPriceText { get; set; }
    }
}