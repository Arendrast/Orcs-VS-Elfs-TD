using Modules.SharedModule;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.TargetPoint
{
    public class TargetPointMovementComponent : MonoBehaviour
    {
        public TargetPointMovementModel Model { get; private set; }
        public TargetPointMovementController Controller { get; private set; }

        public Initializer Initializer => _initializer ??= new Initializer(TryInitialize);
        
        private Initializer _initializer;
        
        public void Awake()
        {
            Initializer.TryInitialize();
        }

        private void TryInitialize()
        {
            Model = new TargetPointMovementModel();
            Controller = new TargetPointMovementController(transform, Model, null);
        }
    }
}