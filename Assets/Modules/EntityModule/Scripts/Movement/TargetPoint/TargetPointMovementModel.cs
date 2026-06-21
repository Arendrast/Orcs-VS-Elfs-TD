using System;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.TargetPoint
{
    public class TargetPointMovementModel
    {

        public Vector3 TargetPoint { get; set; }
        public float TargetSpeed { get; set; }
        public bool DoesMove { get; private set; }

        public event Action StartedMovement, StoppedMovement;
        
        public void SetDoesMove(bool isMove)
        {
            var didMove = DoesMove;
            
            DoesMove = isMove;

            switch (didMove)
            {
                case false when isMove:
                    StartedMovement?.Invoke();
                    break;
                case true when !isMove:
                    StoppedMovement?.Invoke();
                    break;
            }
        }
    }
}