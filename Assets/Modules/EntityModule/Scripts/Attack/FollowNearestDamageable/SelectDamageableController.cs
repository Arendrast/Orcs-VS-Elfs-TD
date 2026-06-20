using System;
using Modules.EntityModule.Scripts.Damageable;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack.FollowNearestDamageable
{
    public class SelectDamageableController : IDisposable
    {
        private readonly SelectDamageableModel _model;
        private readonly DamageablesRepository _damageablesRepository;

        public SelectDamageableController(SelectDamageableModel model, DamageablesRepository damageablesRepository)
        {
            _model = model;
            _damageablesRepository = damageablesRepository;
            
            Subscribe();

            foreach (var pair in _damageablesRepository.Damageables)
            {
                TryAddDamageableToModel(pair.Key, pair.Value);
            }
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            _damageablesRepository.Added += TryAddDamageableToModel;
            _damageablesRepository.Removed += TryRemoveDamageableFromModel;
        }

        private void Subscribe()
        {
            _damageablesRepository.Added -= TryAddDamageableToModel;
            _damageablesRepository.Removed -= TryRemoveDamageableFromModel;
        }

        private void TryAddDamageableToModel(GameObject gameObject, IDamageable damageable)
        {
            _model.TryAddPotentialTargetInAttackZone(gameObject.transform, damageable);
        }

        private void TryRemoveDamageableFromModel(GameObject gameObject, IDamageable damageable)
        {
            _model.TryRemovePotentialTargetInAttackZone(gameObject.transform);
        }
    }
}