using System;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Animations
{
    public class EnemyMovementAnimationsController : IDisposable
    {
        private readonly TargetPointMovementModel _targetPointMovementModel;
        private readonly Animator _animator;

        private static readonly int MovementSpeedMultiplierHash = Animator.StringToHash("MovementSpeedMultiplier");
        
        private const float LocalMovementMultiplier = 0.2f;

        public EnemyMovementAnimationsController(TargetPointMovementModel targetPointMovementModel, Animator animator)
        {
            _targetPointMovementModel = targetPointMovementModel;
            _animator = animator;
            targetPointMovementModel.StartedMovement += PlayMoveAnimation;

            if (targetPointMovementModel.DoesMove)
            {
                PlayMoveAnimation();
            }
        }

        public void Dispose()
        {
            _targetPointMovementModel.StartedMovement -= PlayMoveAnimation;
        }

        private void PlayMoveAnimation()
        {
            _animator.CrossFade("Move", 0.1f, -1, 0f);
            _animator.SetFloat(MovementSpeedMultiplierHash, _targetPointMovementModel.TargetSpeed * LocalMovementMultiplier);
        }
    }
}