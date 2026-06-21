using System;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherLogicComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private PlayerArcherAttackComponent _attackComponent;
        
        private void Start()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
        
        private void Update()
        {
            _attackComponent.AttackController.Update(Time.deltaTime, out var waitForCooldown);
            
            if (_attackComponent.AttackModel.TargetData.HasValue)
            {
                transform.LookAt(_attackComponent.AttackModel.TargetData.Value.Transform);
            }
        }
        
        private void Subscribe()
        {
            _healthComponent.Model.Died += OnDie;
        }

        private void Unsubscribe()
        {
            _healthComponent.Model.Died -= OnDie;
        }
        
        private void OnDie()
        {
            _attackComponent.AttackController.OnEndAttacks();
        }
    }
}
