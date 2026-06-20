using System;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Animations
{
    public class EnemyDeathAnimationsController : IDisposable
    {
        private readonly HealthModel _healthModel;
        private readonly Animator _animator;

        public EnemyDeathAnimationsController(HealthModel healthModel, Animator animator)
        {
            _healthModel = healthModel;
            _animator = animator;
            
            healthModel.Died += PlayDeathAnimation;
        }
        
        public void Dispose()
        {
            _healthModel.Died -= PlayDeathAnimation;
        }

        private void PlayDeathAnimation()
        {
            _animator.CrossFade("Death", 0.1f);
        }
    }
}