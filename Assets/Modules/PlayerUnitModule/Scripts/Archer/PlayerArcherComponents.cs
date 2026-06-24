using Modules.EntityModule.Scripts;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherComponents : MonoBehaviour
    {
        [field: SerializeField] public PlayerArcherLogicComponent LogicComponent { get; private set; }
        [field: SerializeField] public DisableObserverComponent DisableObserverComponent { get; private set; }
        [field: SerializeField] public EntitySoundsComponent EntitySoundsComponent { get; private set; }
        [field: SerializeField] public PlayerArcherArrowSpawnerComponent ArrowSpawnerComponent { get; private set; }
        [field: SerializeField] public MergeUnitComponent MergeUnitComponent { get; private set; }
    }
}