using System;
using System.Collections;
using Modules.EntityModule.Scripts.Health;
using Modules.EntityModule.Scripts.Movement.Path;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Orc
{
    public class OrcEnemyLogicComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private PathMovementComponent _pathMovementComponent;
        [SerializeField] private TargetPointMovementComponent _targetPointMovementComponent;
        [SerializeField] private OrcEnemyAttackComponent _attackComponent;

        private void Start()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Update()
        {
            if (CantHandleAttack())
            {
                return;
            }

            if (_attackComponent.AttackController.ShouldMoveToTarget())
            {
                TryMoveToTarget();
                return;
            }
            
            _targetPointMovementComponent.Controller.TryStopMove();
            _attackComponent.AttackController.Update(Time.deltaTime, out var waitForCooldown);
        }

        private void TryMoveToTarget()
        {
            var targetPoint = _attackComponent.ConcreteAttackModel.TargetData.Value.Transform.position;

            if ((targetPoint - _targetPointMovementComponent.Model.TargetPoint).sqrMagnitude > ConstantsHolder.SqrEpsilon)
            {
                _targetPointMovementComponent.Controller.MoveToPoint(targetPoint, _pathMovementComponent.MovementConfig.Speed);
            }
        }

        private bool CantHandleAttack()
        {
            return _healthComponent.Model.IsDied || !_pathMovementComponent.Model.DoesEndPath() ||
                   _attackComponent.ConcreteAttackModel is { TargetData: null };
        }
        
        private void Subscribe()
        {
            _healthComponent.Model.Died += OnDie;
        }

        private void Unsubscribe()
        {
            _healthComponent.Model.Died -= OnDie;
        }
        
        private void OnDie()
        {
            _pathMovementComponent.Controller?.TryStopMove();
            _targetPointMovementComponent.Controller?.TryStopMove();
            _attackComponent.AttackController.OnEndAttacks();
        }
    }
}