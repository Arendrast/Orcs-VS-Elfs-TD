using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modules.SharedModule.Scripts;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherArrowFactory
    {
        private readonly Pool<ArcherArrowMovementComponent> _pool;

        public PlayerArcherArrowFactory()
        {
            _pool = new Pool<ArcherArrowMovementComponent>(null, null, null, defaultCapacity: 10);
        }

        public ArcherArrowMovementController GetArcherArrowMovementController(
            ArcherArrowMovementComponent prefab,
            Transform target,
            float flyTime, Vector3 startPosition, Action endedMovement)
        {
            var instance = _pool.TryGet(prefab);

            endedMovement += Release;
            
           var controller =
                new ArcherArrowMovementController(target, flyTime, startPosition, instance.transform,
                    endedMovement);

            instance.Construct(controller);

            return controller;

            void Release()
            {
                _pool.TryRelease(instance);
            }
        }
    }
}