using System;
using Modules.SharedModule.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup
{
    [Serializable]
    public class ShowMoneyPopupConfig
    {
        [field: Header("Components")]
        [field: SerializeField] public Image MoneyImagePrefab { get; private set; }
        [field: SerializeField] public Transform CanvasTransform { get; private set; }
        [field: SerializeField] public RectTransform MoneyAnimationEndPointTransform { get; private set; }
        
        [field: Space]
        [field: Header("Animation settings")]
        [field: SerializeField] public float MoveMoneyDuration { get; private set; } = 2;
        [field: SerializeField] public float PullbackForce { get; private set; } = 100f;
        [field: SerializeField] public Vector2 MaximalMoneySpawnOffset { get; private set; } = new Vector2(5, 5);
        [field: SerializeField] public float ArcOffset { get; private set; } = 200f;
    }
}