using System;
using Modules.EntityModule.Scripts.Attack.FollowNearestDamageable;
using Modules.EntityModule.Scripts.Damageable;
using UnityEngine;
using Random = System.Random;

namespace Modules.EntityModule.Scripts.Attack
{
    public class AttackController<TAttackType, TCustomAttackConfig> : IDisposable
        where TAttackType : Enum
        where TCustomAttackConfig : ICustomAttackConfig
    {
        public bool DoesMoveToTarget { get; private set; }

        public event Action<IDamageable, AttackConfig<TAttackType, TCustomAttackConfig>, bool> DealtDamage;

        private float _remainingTime;
        private bool _dealtDamageInCurrentAttack;

        private readonly AttackModel<TAttackType, TCustomAttackConfig> _model;
        private readonly SelectDamageableController _selectDamageableController;

        public AttackController(AttackModel<TAttackType, TCustomAttackConfig> model,
            DamageablesRepository damageablesRepository)
        {
            _model = model;

            _selectDamageableController = new SelectDamageableController(model.SelectDamageableModel, damageablesRepository);
        }
        
        public void Dispose()
        {
            _selectDamageableController?.Dispose();
        }

        public void Update(float deltaTime, out bool waitForCooldown)
        {
            if (_model.IsAttacking)
            {
                WaitEndAttackAndEndAttackIfNeed(deltaTime);
                waitForCooldown = false;
                return;
            }

            _model.SelectDamageableModel.AppointTarget();
            
            WaitCooldownAndTryStartAttack(deltaTime);
            waitForCooldown = true;
        }

        public bool ShouldMoveToTarget()
        {
            return !_model.IsAttacking && _model.SelectDamageableModel.TargetData != null && !_model.IsNearToTarget();
        }

        public void TryStartAttack()
        {
            if (_remainingTime > 0 || !_model.IsNearToTarget())
            {
                return;
            }

            var config = _model.AttacksConfig.AttacksConfigs.Count == 0
                ? null
                : _model.AttacksConfig.AttacksConfigs[
                    UnityEngine.Random.Range(0, _model.AttacksConfig.AttacksConfigs.Count)];

            if (config == null)
            {
                return;
            }

            if (_model.TryStartAttack(config))
            {
                _remainingTime = config.FullAttackTime;
            }
        }

        public void OnEndAttacks()
        {
            DoesMoveToTarget = false;
            _remainingTime = 0;
            _model.TryEndAttack();
            _dealtDamageInCurrentAttack = false;
        }

        private void WaitEndAttackAndEndAttackIfNeed(float deltaTime)
        {
            _remainingTime -= deltaTime;

            if (!_dealtDamageInCurrentAttack &&
                (_model.TargetAttackConfig.FullAttackTime - _model.TargetAttackConfig.DoDamageTime) > _remainingTime)
            {
                DealtDamage?.Invoke(_model.SelectDamageableModel.TargetData.Value.Damageable,
                    _model.TargetAttackConfig, _model.IsNearToTarget());

                _dealtDamageInCurrentAttack = true;
            }

            if (_remainingTime <= 0)
            {
                _model.TryEndAttack();
                _dealtDamageInCurrentAttack = false;
                _remainingTime = _model.AttacksConfig.CooldownBetweenAttacks;
            }
        }

        private void WaitCooldownAndTryStartAttack(float deltaTime)
        {
            _remainingTime -= deltaTime;
            TryStartAttack();
        }
    }
}