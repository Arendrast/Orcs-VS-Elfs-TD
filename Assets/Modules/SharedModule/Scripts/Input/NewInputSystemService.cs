using UnityEngine.InputSystem;

namespace Modules.SharedModule.Scripts.Input
{
    public class NewInputSystemService : IInputService
    {
        public InputAction MouseClickInputAction => _inputActions.Global.Touch;

        private readonly InputActions _inputActions;
        
        public NewInputSystemService(InputActions inputActions)
        {
            _inputActions = inputActions;
            _inputActions.Enable();
        }
    }
}