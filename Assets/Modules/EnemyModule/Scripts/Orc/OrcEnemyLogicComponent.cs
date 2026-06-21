using System;
using System.Collections;
using Modules.EntityModule.Scripts.Damageable;
using Modules.EntityModule.Scripts.Health;
using Modules.EntityModule.Scripts.Movement.Path;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Orc
{
    public class OrcEnemyLogicComponent : MonoBehaviour
    {
        [field: SerializeField] public HealthComponent HealthComponent { get; private set; }
        [field: SerializeField] public PathMovementComponent MovementComponent { get; private set; }
        [field: SerializeField] public TargetPointMovementComponent PointMovementComponent { get; private set; }
        [field: SerializeField] public OrcEnemyAttackComponent AttackComponent { get; private set; }
        [field: SerializeField] public DamageableComponent DamageableComponent { get; private set; }

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

            if (AttackComponent.AttackController.ShouldMoveToTarget())
            {
                TryMoveToTarget();
                return;
            }

            PointMovementComponent.Controller.TryStopMove();
            AttackComponent.AttackController.Update(Time.deltaTime, out var waitForCooldown);
        }

        private void TryMoveToTarget()
        {
            var targetPoint = AttackComponent.ConcreteAttackModel.TargetData.Value.Transform.position;

            if ((targetPoint - PointMovementComponent.Model.TargetPoint).sqrMagnitude > ConstantsHolder.SqrEpsilon)
            {
                PointMovementComponent.Controller.MoveToPoint(targetPoint, MovementComponent.MovementConfig.Speed);
            }
        }

        private bool CantHandleAttack()
        {
            return HealthComponent.Model.IsDied || !MovementComponent.Model.DoesEndPath() ||
                   AttackComponent.ConcreteAttackModel is { TargetData: null };
        }

        private void Subscribe()
        {
            HealthComponent.Model.Died += OnDie;
        }

        private void Unsubscribe()
        {
            if (HealthComponent != null)
                HealthComponent.Model.Died -= OnDie;
        }

        private void OnDie()
        {
            MovementComponent.Controller?.TryStopMove();
            PointMovementComponent.Controller?.TryStopMove();
            AttackComponent.AttackController.OnEndAttacks();
        }
    }
}