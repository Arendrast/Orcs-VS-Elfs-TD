using System;
using Modules.EntityModule.Scripts.Health;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Damageable
{
    public class DamageableComponent : MonoBehaviour, IDamageable
    {
        public event Action Died
        {
            add => Model.Died += value;
            remove => Model.Died -= value;
        }
        
        public bool IsDied => Model?.IsDied ?? false;
        public Initializer Initializer => _initializer ??= new Initializer(TryInitialize);
        public DamageableModel Model { get; private set; }

        private Initializer _initializer;
        
        [SerializeField] private HealthComponent _healthComponent;
        
        private void OnEnable()
        {
            Initializer.TryInitialize();
        }

        private void OnDisable()
        {
            Initializer.Deinitialize();
        }

        public bool TryTakeDamage(int damage)
        {
            return Model.TryTakeDamage(damage);
        }

        public void SetIsDamageable(bool value)
        {
            Model.SetIsDamageable(value);
        }

        private void TryInitialize()
        {
            _healthComponent.Initializer.TryInitialize();
            Model = new DamageableModel(_healthComponent.Model);
        }
    }
}