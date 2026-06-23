using Modules.EntityModule.Scripts.Damageable;
using Modules.EntityModule.Scripts.Health;
using Modules.SharedModule.Scripts.Audio;
using UnityEngine;

namespace Modules.EntityModule.Scripts
{
    public class EntitySoundsComponent : MonoBehaviour
    {
        [SerializeField] private DamageableComponent _damageableComponent;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioId[] _hitSoundIds, _deathSoundIds;
        
        private AudioService _audioService;

        public void Construct(AudioService audioService)
        {
            _audioService = audioService;

            _damageableComponent.Initializer.TryInitialize();
            _healthComponent.Initializer.TryInitialize();
            
            _damageableComponent.Model.DealDamage += PlayHitSound;
            _healthComponent.Model.Died += PlayDeathSound;
        }

        private void OnDisable()
        {
            _damageableComponent.Model.DealDamage -= PlayHitSound;
            _healthComponent.Model.Died -= PlayDeathSound;
        }

        private void PlayDeathSound()
        {
            foreach (var soundId in _deathSoundIds)
            {
                _audioService.TryPlayOneShot(_audioSource, soundId);
            }
        }

        private void PlayHitSound(int obj)
        {
            foreach (var soundId in _hitSoundIds)
            {
                _audioService.TryPlayOneShot(_audioSource, soundId);
            }
        }
    }
}