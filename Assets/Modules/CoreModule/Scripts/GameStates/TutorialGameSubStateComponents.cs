using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitTutorialPopup;
using Modules.PlayerUnitModule.Scripts.Merge.MergeUnitTutorialPopup;
using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class TutorialGameSubStateComponents : MonoBehaviour
    {
        [field: SerializeField] public float DelayAfterKillFirstEnemyBeforeShowUI { get; private set; } = 0.5f;
        [field: SerializeField] public BuyUnitTutorialPopupComponent BuyUnitTutorialPopupComponent { get; private set; }
        [field: SerializeField] public MergeUnitTutorialPopupComponent MergeUnitTutorialPopupComponent { get; private set; }
    }
}