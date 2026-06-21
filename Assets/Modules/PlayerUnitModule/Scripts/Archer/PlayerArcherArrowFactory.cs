using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherArrowFactory
    {
        public ArcherArrowMovementController GetCreatedArcherArrowMovementController(ArcherArrowMovementComponent prefab,
            Transform target,
            float flyTime, Vector3 startPosition, Action endedMovement)
        {
            var instance = Object.Instantiate(prefab);
            var controller = new ArcherArrowMovementController(target, flyTime, startPosition, instance.transform, endedMovement);
            
            instance.Construct(controller);

            return controller;
        }
    }
}