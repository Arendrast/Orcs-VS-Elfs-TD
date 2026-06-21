using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Orc
{
    public class OrcEnemyComponents : MonoBehaviour
    {
        [field: SerializeField] public OrcEnemyLogicComponent OrcEnemyLogicComponent { get; private set; }
        [field: SerializeField] public ActivatorAfterDelayComponent ActivatorAfterDelayComponent { get; private set; }
        [field: SerializeField] public ToCameraLookerComponent ToCameraLookerComponent { get; private set; }
    }
}