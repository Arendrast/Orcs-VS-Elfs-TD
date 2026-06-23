using System;
using TMPro;
using UnityEngine;

namespace Modules.SharedModule.Scripts
{
    public class FPSTextComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _updateInterval = 0.5f;

        private float _remainingUpdateTime;
        
        private void Update()
        {
            if (_remainingUpdateTime <= 0)
            {
                _remainingUpdateTime = _updateInterval;
                var fps = 1f / Time.unscaledDeltaTime;
                _text.text = fps > 60 ? "60+" : fps.ToString("f0");
                return;
            }
            
            _remainingUpdateTime -= Time.unscaledDeltaTime;
        }
    }
}