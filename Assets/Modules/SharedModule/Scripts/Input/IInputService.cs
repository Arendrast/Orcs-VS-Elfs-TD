using UnityEngine.InputSystem;

namespace Modules.SharedModule.Scripts.Input
{
    public interface IInputService
    {
        InputAction MouseClickInputAction { get; }
    }
}