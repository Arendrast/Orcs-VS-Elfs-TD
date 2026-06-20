using System;
using Modules.EntityModule.Scripts.Health;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Damageable
{
    public class DamageableModel : IDamageable
    {
        public bool IsDied => _healthModel.IsDied;
        
        public event Action<int> DealDamage;
        
        private readonly HealthModel _healthModel;

        public DamageableModel(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }

        public bool TryTakeDamage(int damage)
        {
            if (damage <= 0 || !_healthModel.TrySetHealthPoints(Mathf.Max(0, _healthModel.HealthPoints - damage)))
            {
                return false;
            }
            
            DealDamage?.Invoke(damage);

            return true;
        }
    }
}