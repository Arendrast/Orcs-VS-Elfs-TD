using System;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.EntityModule.Scripts.Health
{
    public class HealthBarComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Slider _sliderBar;

        private void OnEnable()
        {
            _healthComponent.Model.ChangedHealthPoints += UpdateBar;
            UpdateBar(_healthComponent.Model.HealthPoints);
        }

        private void OnDisable()
        {
            if (_healthComponent != null)
                _healthComponent.Model.ChangedHealthPoints -= UpdateBar;
        }

        private void UpdateBar(int healthPoints)
        {
            _sliderBar.maxValue = _healthComponent.Model.MaxHealthPoints;
            _sliderBar.value = healthPoints;
        }
    }
}