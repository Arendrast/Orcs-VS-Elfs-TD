using System;
using TMPro;
using UnityEngine;

namespace Modules.SharedModule.Scripts.Currencies.CurrenciesPopup
{
    [Serializable]
    public class CurrenciesPopupConfig
    {
        [field: SerializeField] public TextMeshProUGUI CurrencyNumberText { get; private set; }
    }
}