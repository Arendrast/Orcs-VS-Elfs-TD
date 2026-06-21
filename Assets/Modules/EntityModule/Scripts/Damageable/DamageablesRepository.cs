using System;
using System.Collections.Generic;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Damageable
{
    public class DamageablesRepository
    {
        public IReadOnlyDictionary<GameObject, IDamageable> Damageables => _damageables;
        
        public event Action<GameObject, IDamageable> Added, Removed;

        private readonly Dictionary<GameObject, IDamageable> _damageables = new Dictionary<GameObject, IDamageable>();

        public bool TryAdd(GameObject gameObject, IDamageable damageable)
        {
            if (!_damageables.TryAdd(gameObject, damageable))
            {
                return false;
            }

            gameObject.AddComponent<DisableObserverComponent>().Disabled += Remove;
            
            Added?.Invoke(gameObject, damageable);

            return true;
        }

        private void Remove(GameObject gameObject)
        {
            TryRemove(gameObject);
        }
        
        public bool TryRemove(GameObject gameObject)
        {
            if (!_damageables.Remove(gameObject, out var damageable))
            {
                return false;
            }
            
            Removed?.Invoke(gameObject, damageable);

            return true;
        }
    }
}