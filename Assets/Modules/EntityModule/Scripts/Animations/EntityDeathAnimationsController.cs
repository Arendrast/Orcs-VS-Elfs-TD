using System;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Animations
{
    public class EntityDeathAnimationsController : IDisposable
    {
        private readonly HealthModel _healthModel;
        private readonly Animator _animator;

        public EntityDeathAnimationsController(HealthModel healthModel, Animator animator)
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
            _animator.CrossFade("Death", 0.1f, -1, 0f);
        }
    }
}