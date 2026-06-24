using Modules.CoreModule.Scripts.EndPopup;
using Modules.EnemyModule.Scripts.Orc;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Currencies.CurrenciesPopup;
using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class GameplayStateComponents : MonoBehaviour 
    {
        [field: SerializeField] public OrcEnemyComponents[] OrcEnemyComponents { get; private set; }
        [field: SerializeField] public BuyMergeUnitConfig BuyMergeUnitConfig { get; private set; }
        [field: SerializeField] public MergeGridComponent MergeGridComponent { get; private set; }
        [field: SerializeField] public DragAndDropMergeGridComponent DragAndDropMergeGridComponent { get; private set; }
        [field: SerializeField] public TutorialGameSubStateComponents TutorialGameSubStateComponents { get; private set; }
        [field: SerializeField] public BuyUnitPopupComponent BuyUnitPopupComponent { get; private set; }
        [field: SerializeField] public ShowMoneyPopupComponent ShowMoneyPopupComponent { get; private set; }
        [field: SerializeField] public CurrenciesPopupComponent CurrenciesPopupComponent { get; private set; }
        [field: SerializeField] public AudioConfig AudioConfig { get; private set; }
        [field: SerializeField] public EndPopupComponent EndPopupComponent { get; private set; }
        [field: SerializeField] public float TimeAfterLooseBeforeShowPopup { get; private set; } = 1.5f;
        [field: SerializeField] public float TimeAfterReachEnemyEndPointBeforeLoose { get; private set; } = 1.5f;
        [field: SerializeField] public float TimeAfterWinBeforeShowPopup { get; private set; } = 1.5f;
    }
}