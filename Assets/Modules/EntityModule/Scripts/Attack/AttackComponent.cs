using System;
using Modules.EntityModule.Scripts.Attack.FollowNearestDamageable;
using Modules.EntityModule.Scripts.Damageable;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack
{
    public abstract class AttackComponent : MonoBehaviour
    // В полях сериализация интерфейсов без одина затруднена, поэтому абстрактный класс
    {
        public abstract IAttackModel AttackModel { get; } 
        public abstract void Construct(DamageablesRepository damageablesRepository, Func<Vector3> positionFunc = null);
    }
    
    public class AttackComponent<TAttackType, TCustomAttackConfig> : AttackComponent
        where TAttackType : Enum // AttackType сделан чтобы идентифицировать атаки. Конечно, можно было сделать и просто int, но это неудобно
        where TCustomAttackConfig : ICustomAttackConfig
    {
        public override IAttackModel AttackModel => ConcreteAttackModel;
        public AttackModel<TAttackType, TCustomAttackConfig> ConcreteAttackModel { get; private set; }
        public AttackController<TAttackType, TCustomAttackConfig> AttackController { get; private set; }

        [SerializeField] private bool _autoUpdate;
        [SerializeField] private AttacksConfig<TAttackType, TCustomAttackConfig> _attacksConfig;
        [SerializeField] private SelectDamageableModel.SelectTargetType _selectTargetType;


        public override void Construct(DamageablesRepository damageablesRepository, Func<Vector3> positionFunc = null)
        {
            ConcreteAttackModel = new AttackModel<TAttackType, TCustomAttackConfig>(positionFunc ?? GetDefaultPosition, _attacksConfig, _selectTargetType);
            AttackController = new AttackController<TAttackType, TCustomAttackConfig>(ConcreteAttackModel, damageablesRepository);
            AttackController.DealtDamage += DoDamage;
        }

        private void DoDamage(IDamageable damageable, AttackConfig<TAttackType, TCustomAttackConfig> attackConfig, bool isNear)
        {
            damageable.TryTakeDamage(attackConfig.Damage);
        }

        private Vector3 GetDefaultPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (!_autoUpdate || AttackController == null || ConcreteAttackModel == null)
            {
                return;
            }
            
            AttackController.Update(Time.deltaTime, out var waitForCooldown);
        }

        private void OnDisable()
        {
            if (AttackController != null)
            {
                AttackController.Dispose();
                AttackController.DealtDamage -= DoDamage;
            }
        }
    }
}