using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class UpgradeUnitVfxComponent : MonoBehaviour
    {
        [field: SerializeField] public DisablerAfterTimeComponent DisablerAfterTimeComponent { get; private set; }
    }
}