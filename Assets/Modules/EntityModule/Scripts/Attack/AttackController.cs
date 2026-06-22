using System;
using Modules.EntityModule.Scripts.Attack.FollowNearestDamageable;
using Modules.EntityModule.Scripts.Damageable;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;
using Random = System.Random;

namespace Modules.EntityModule.Scripts.Attack
{
    public class AttackController<TAttackType, TCustomAttackConfig> : IDisposable
        where TAttackType : Enum
        where TCustomAttackConfig : ICustomAttackConfig
    {
        public bool DoesMoveToTarget { get; private set; }
        public bool DealtDamageInCurrentAttack { get; private set; }

        public event Action<IDamageable, AttackConfig<TAttackType, TCustomAttackConfig>, bool> DealtDamage;

        private float _remainingTime;

        private readonly AttackModel<TAttackType, TCustomAttackConfig> _model;
        private readonly SelectDamageableController _selectDamageableController;
        private readonly Func<bool> _canSkipAttackFunc;
        private readonly HealthModel _healthModel;

        public AttackController(AttackModel<TAttackType, TCustomAttackConfig> model,
            DamageablesRepository damageablesRepository, Func<bool> canSkipAttackFunc, HealthModel healthModel = null)
        {
            _model = model;
            _canSkipAttackFunc = canSkipAttackFunc;
            _healthModel = healthModel;

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
            if (_remainingTime > 0 || !_model.IsNearToTarget() || (_healthModel != null && _healthModel.IsDied))
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
            DealtDamageInCurrentAttack = false;
        }

        private void WaitEndAttackAndEndAttackIfNeed(float deltaTime)
        {
            _remainingTime -= deltaTime;
            
            if (!DealtDamageInCurrentAttack &&
                (_model.ConcreteTargetAttackConfig.FullAttackTime - _model.ConcreteTargetAttackConfig.DoDamageTime) > _remainingTime)
            {
                DealtDamage?.Invoke(_model.SelectDamageableModel.TargetData.Value.Damageable,
                    _model.ConcreteTargetAttackConfig, _model.IsNearToTarget());

                DealtDamageInCurrentAttack = true;
            }

            if (_remainingTime <= 0 || _canSkipAttackFunc.Invoke())
            {
                _model.TryEndAttack();
                DealtDamageInCurrentAttack = false;
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