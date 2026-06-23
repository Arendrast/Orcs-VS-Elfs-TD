using System;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathMovementComponent : MonoBehaviour
    {
        public Initializer Initializer => _initializer ??= new Initializer(TryInitialize);
        
        public PathMovementModel Model { get; private set; }
        public PathMovementController Controller { get; private set; }
        [field: SerializeField] public PathComponent Component { get; private set; }
        [field: SerializeField] public PathMovementConfig MovementConfig { get; private set; }

        private Initializer _initializer;
        
        public void Awake()
        {
            Initializer.TryInitialize();
        }

        private void OnDisable()
        {
            Initializer.Deinitialize();
        }

        private void TryInitialize()
        {
            Model = new PathMovementModel(Component);
            Controller = new PathMovementController(Model, transform, MovementConfig);
        }
    }
}