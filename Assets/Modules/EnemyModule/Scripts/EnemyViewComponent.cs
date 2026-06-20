using System;
using System.Collections.Generic;
using Modules.EnemyModule.Scripts.Animations;
using Modules.EntityModule.Scripts.Attack;
using Modules.EntityModule.Scripts.Health;
using Modules.EntityModule.Scripts.Movement.Path;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using NUnit.Framework;
using UnityEngine;

namespace Modules.EnemyModule.Scripts
{
    public class EnemyViewComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private PathMovementComponent _pathMovementComponent;
        [SerializeField] private TargetPointMovementComponent _targetPointMovementComponent;
        [SerializeField] private AttackComponent _attackComponent;
        [SerializeField] private Animator _animator;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private void Start()
        {
            if (_targetPointMovementComponent != null)
            {
                _disposables.Add(new EnemyMovementAnimationsController(_targetPointMovementComponent.Model, _animator));
            }

            if (_pathMovementComponent != null)
            {
                _disposables.Add(
                    new EnemyMovementAnimationsController(
                        _pathMovementComponent.Controller.PointMovementController.Model, _animator));
            }

            if (_attackComponent != null)
            {
                _disposables.Add(new EnemyAttackAnimationsController(_attackComponent.AttackModel, _animator));
            }

            if (_healthComponent != null)
            {
                _disposables.Add(new EnemyDeathAnimationsController(_healthComponent.Model, _animator));
            }
        }

        private void OnDisable()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}