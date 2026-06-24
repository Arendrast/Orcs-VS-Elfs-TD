using System;
using System.Collections;
using System.Collections.Generic;
using Modules.EntityModule.Scripts.Animations;
using Modules.EntityModule.Scripts.Attack;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts
{
    public class PlayerUnitAnimationsComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private AttackComponent _attackComponent;
        [SerializeField] private Animator _animator;
        
        [SerializeField] private float _attackSpeedAnimationMultiplier;

        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private void OnEnable()
        {
            StartCoroutine(SkipFrameAndSubscribe());
        }

        private IEnumerator SkipFrameAndSubscribe()
        {
            yield return null;
            
            if (_attackComponent != null)
            {
                _disposables.Add(new EntityAttackAnimationsController(_attackComponent.AttackModel, _animator, _attackSpeedAnimationMultiplier));
            }

            if (_healthComponent != null)
            {
                _disposables.Add(new EntityDeathAnimationsController(_healthComponent.Model, _animator));
            }
        }

        private void OnDisable()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            
            _disposables.Clear();
        }
    }
}