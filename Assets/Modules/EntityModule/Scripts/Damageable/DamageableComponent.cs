using Modules.EntityModule.Scripts.Health;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Damageable
{
    public class DamageableComponent : MonoBehaviour, IDamageable
    {
        public bool IsDied => Model?.IsDied ?? false;
        public Initializer ComponentInitializer => _componentInitializer ??= new Initializer(TryInitialize);
        public DamageableModel Model { get; private set; }

        private Initializer _componentInitializer;
        
        [SerializeField] private HealthComponent _healthComponent;
        
        private void Awake()
        {
            ComponentInitializer.TryInitialize();
        }

        public bool TryTakeDamage(int damage)
        {
            return Model.TryTakeDamage(damage);
        }

        private void TryInitialize()
        {
            _healthComponent.Initializer.TryInitialize();
            Model = new DamageableModel(_healthComponent.Model);
        }
    }
}