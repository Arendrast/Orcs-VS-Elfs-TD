using System;
using System.Collections;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Health
{
    public class HealthComponent : MonoBehaviour
    {
        public HealthModel Model { get; private set; }
      
        public Initializer Initializer => _initializer ??= new Initializer(TryInitialize);
        
        private Initializer _initializer;
        
        [SerializeField] private HealthConfig _healthConfig;
        [SerializeField] private bool _disableAfterDie = true;
        [SerializeField] private float _timeBeforeDisableAfterDie = 2;

        private bool _initialized;
        
        private void OnEnable()
        {
            Initializer.TryInitialize();
        }

        private void OnDisable()
        {
            Model.Died -= StartDelayedDisable;
            Initializer.Deinitialize();
        }

        private void StartDelayedDisable()
        {
            StartCoroutine(DelayedDisable());
        }

        private IEnumerator DelayedDisable()
        {
            yield return new WaitForSeconds(_timeBeforeDisableAfterDie);
            gameObject.SetActive(false);
        }

        private void TryInitialize()
        {
            Model = new HealthModel(_healthConfig.MaxHealth);

            if (_disableAfterDie)
            {
                Model.Died += StartDelayedDisable;
            }
        }
    }
}
