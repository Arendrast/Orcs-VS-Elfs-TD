using System;
using Modules.EntityModule.Scripts.Attack;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Animations
{
    public class EntityAttackAnimationsController : IDisposable
    {
        private static readonly int AttackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");
        
        private readonly Animator _animator;
        private readonly IAttackModel _attackModel;

        private readonly float _attackSpeedMultiplier;

        public EntityAttackAnimationsController(IAttackModel attackModel, Animator animator, float attackSpeedMultiplier)
        {
            _attackModel = attackModel;
            _animator = animator;
            _attackSpeedMultiplier = attackSpeedMultiplier;

            attackModel.StartedAttackByConfig += TryPlayAttackAnimation;

            if (attackModel.IsAttacking)
            {
                TryPlayAttackAnimation(attackModel.TargetAttackConfig);
            }
        }

        public void Dispose()
        {
            _attackModel.StartedAttackByConfig -= TryPlayAttackAnimation;
        }

        private void TryPlayAttackAnimation(IAttackConfig config)
        {
            _animator.SetFloat(AttackSpeedMultiplier, config.FullAttackTime * _attackSpeedMultiplier);
            _animator.CrossFade("Attack", 0.1f, -1, 0f);
        }
    }
}