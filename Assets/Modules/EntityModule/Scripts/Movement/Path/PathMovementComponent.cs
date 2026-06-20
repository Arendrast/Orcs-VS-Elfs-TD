using Modules.EntityModule.Scripts.Movement.TargetPoint;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathMovementComponent : MonoBehaviour
    {
        public Initializer Initializer => _initializer ??= new Initializer(TryInitialize);
        
        public PathMovementModel Model { get; private set; }
        public PathMovementController Controller { get; private set; }
        [field: SerializeField] public PathConfig Config { get; private set; }
        [field: SerializeField] public PathMovementConfig MovementConfig { get; private set; }

        private Initializer _initializer;
        
        public void Awake()
        {
            Initializer.TryInitialize();
        }

        private void TryInitialize()
        {
            Model = new PathMovementModel(Config);
            Controller = new PathMovementController(Model, transform, MovementConfig);
        }
    }
}