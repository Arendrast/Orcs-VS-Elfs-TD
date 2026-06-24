using System;
using Modules.EntityModule.Scripts.Damageable;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherLogicComponent : MonoBehaviour
    {
        [field: SerializeField] public HealthComponent HealthComponent { get; private set; }
        [field: SerializeField] public DamageableComponent DamageableComponent { get; private set; }
        [field: SerializeField] public PlayerArcherAttackComponent AttackComponent { get; private set; }

        private void OnEnable()
        {
            HealthComponent.Initializer.TryInitialize();
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
        
        private void Update()
        {
            AttackComponent.AttackController.Update(Time.deltaTime, out var waitForCooldown);
            
            if (AttackComponent.AttackModel.TargetData.HasValue)
            {
                transform.LookAt(AttackComponent.AttackModel.TargetData.Value.Transform);
            }
        }
        
        private void Subscribe()
        {
            HealthComponent.Model.Died += OnDie;
        }

        private void Unsubscribe()
        {
            HealthComponent.Model.Died -= OnDie;
        }
        
        private void OnDie()
        {
            AttackComponent.AttackController.OnEndAttacks();
        }
    }
}
