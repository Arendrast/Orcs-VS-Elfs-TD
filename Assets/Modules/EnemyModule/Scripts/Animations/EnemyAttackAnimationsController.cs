using System;
using Modules.EntityModule.Scripts.Attack;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Animations
{
    public class EnemyAttackAnimationsController : IDisposable
    {
        private readonly Animator _animator;
        private readonly IAttackModel _attackModel;

        public EnemyAttackAnimationsController(IAttackModel attackModel, Animator animator)
        {
            _attackModel = attackModel;
            _animator = animator;

            attackModel.StartedAttackWithoutArgs += PlayAttackAnimation;
        }

        public void Dispose()
        {
            _attackModel.StartedAttackWithoutArgs -= PlayAttackAnimation;
        }

        private void PlayAttackAnimation()
        {
            _animator.CrossFade("Attack", 0.1f);
        }
    }
}