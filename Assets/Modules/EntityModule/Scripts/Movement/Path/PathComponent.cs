using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform[] MovementPositionsTransforms { get; private set; }
    }
}