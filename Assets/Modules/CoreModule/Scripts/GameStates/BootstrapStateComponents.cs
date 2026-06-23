using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class BootstrapStateComponents : MonoBehaviour
    {
        [field: SerializeField] public int TargetFrameRate { get; private set; } = 90;
    }
}