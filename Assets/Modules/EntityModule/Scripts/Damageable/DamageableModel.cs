using System;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Damageable
{
    public class DamageableModel : IDamageable
    {
        public bool IsDied => _healthModel.IsDied;
        
        public event Action Died
        {
            add => _healthModel.Died += value;
            remove => _healthModel.Died -= value;
        }
        
        public event Action<int> DealDamage;

        private bool _isDamageable = true;
        
        private readonly HealthModel _healthModel;

        public DamageableModel(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }

        public bool TryTakeDamage(int damage)
        {
            if (!_isDamageable)
            {
                DealDamage?.Invoke(0);
                return true;
            }
            
            if (damage < 0 || !_healthModel.TrySetHealthPoints(Mathf.Max(0, _healthModel.HealthPoints - damage)))
            {
                return false;
            }
            
            DealDamage?.Invoke(damage);

            return true;
        }

        public void SetIsDamageable(bool value)
        {
            _isDamageable = value;
        }
    }
}