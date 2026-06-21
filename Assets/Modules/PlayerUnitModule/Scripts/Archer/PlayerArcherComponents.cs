using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherComponents : MonoBehaviour
    {
        [field: SerializeField] public PlayerArcherLogicComponent LogicComponent { get; private set; }
        [field: SerializeField] public PlayerArcherArrowSpawnerComponent ArrowSpawnerComponent { get; private set; }
    }
}