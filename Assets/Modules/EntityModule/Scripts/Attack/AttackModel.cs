using System;
using System.Linq;
using Modules.EntityModule.Scripts.Attack.FollowNearestDamageable;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack
{
    public class AttackModel<TAttackType, TCustomAttackConfig> : IAttackModel // CustomAttackConfig сделал на будущее, чтобы была возможность добавлять доп параметры к конфигам атаки
        where TAttackType : Enum
        where TCustomAttackConfig : ICustomAttackConfig
    {
        public TargetData? TargetData => SelectDamageableModel.TargetData;

        public bool IsAttacking { get; private set; }
        public AttackConfig<TAttackType, TCustomAttackConfig> TargetAttackConfig { get; private set; }
        public SelectDamageableModel SelectDamageableModel { get; }

        public event Action<AttackConfig<TAttackType, TCustomAttackConfig>> StartedAttack, EndedAttack;
        public event Action<IAttackConfig> StartedAttackByConfig, EndedAttackByConfig;

        public readonly AttacksConfig<TAttackType, TCustomAttackConfig> AttacksConfig;
        private readonly HealthModel _healthModel;

        private readonly Func<Vector3> _positionFunc;

        public AttackModel(Func<Vector3> positionFunc, AttacksConfig<TAttackType, TCustomAttackConfig> attacksConfig,
            SelectDamageableModel.SelectTargetType selectTargetType, HealthModel healthModel)
        {
            _positionFunc = positionFunc;
            AttacksConfig = attacksConfig;
            _healthModel = healthModel;

            SelectDamageableModel = new SelectDamageableModel(_positionFunc, selectTargetType);
        }

        public bool TryStartAttack(AttackConfig<TAttackType, TCustomAttackConfig> config)
        {
            if (IsAttacking || _healthModel is { IsDied: true })
            {
                return false;
            }

            IsAttacking = true;

            StartedAttack?.Invoke(config);
            StartedAttackByConfig?.Invoke(config);

            TargetAttackConfig = config;

            return true;
        }

        public bool IsNearToTarget()
        {
            if (!TargetData.HasValue)
            {
                return false;
            }
            
            var sqrDistance =
                (TargetData.Value.Transform.position
                 - _positionFunc.Invoke()).sqrMagnitude;

            var result = SelectDamageableModel.TargetData.HasValue &&
                         AttacksConfig.AttackDistance * AttacksConfig.AttackDistance >= sqrDistance;

            return result;
        }

        public void TryEndAttack()
        {
            if (!IsAttacking)
            {
                return;
            }

            IsAttacking = false;

            EndedAttack?.Invoke(TargetAttackConfig);
            EndedAttackByConfig?.Invoke(TargetAttackConfig);

            TargetAttackConfig = null;
        }
    }
}