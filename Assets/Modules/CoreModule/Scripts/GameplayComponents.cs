using Modules.EnemyModule.Scripts.Orc;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Merge;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    public class GameplayComponents : MonoBehaviour 
    {
        [field: SerializeField] public OrcEnemyComponents[] OrcEnemyComponents { get; private set; }
        [field: SerializeField] public PlayerArcherComponents[] PlayerArcherComponents { get; private set; }
        [field: SerializeField] public MergeGridComponent MergeGridComponent { get; private set; }
        [field: SerializeField] public DragAndDropMergeGridComponent DragAndDropMergeGridComponent { get; private set; }
    }
}