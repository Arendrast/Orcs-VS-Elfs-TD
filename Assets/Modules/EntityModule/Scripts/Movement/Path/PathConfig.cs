using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathConfig : MonoBehaviour
    {
        [field: SerializeField] public Transform[] MovementPositionsTransforms { get; private set; }
    }
}