using System;
using UnityEngine;

namespace Modules.SharedModule.Scripts
{
    public class DisableObserverComponent : MonoBehaviour
    {
        public event Action<GameObject> Disabled;
        
        private void OnDisable()
        {
            Disabled?.Invoke(gameObject);
        }
    }
}