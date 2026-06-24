using System;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.TargetPoint
{
    public class TargetPointMovementComponent : MonoBehaviour
    {
        public TargetPointMovementModel Model { get; private set; }
        public TargetPointMovementController Controller { get; private set; }

        public Initializer Initializer => _initializer ??= new Initializer(TryInitialize);
        
        private Initializer _initializer;
        
        public void OnEnable()
        {
            Initializer.TryInitialize();
        }

        private void OnDisable()
        {
            Initializer.Deinitialize();
        }

        private void TryInitialize()
        {
            Model = new TargetPointMovementModel();
            Controller = new TargetPointMovementController(transform, Model, null);
        }
    }
}