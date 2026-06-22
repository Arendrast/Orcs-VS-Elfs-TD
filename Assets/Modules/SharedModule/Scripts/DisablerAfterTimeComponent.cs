using System.Collections;
using UnityEngine;

namespace Modules.SharedModule.Scripts
{
    public class DisablerAfterTimeComponent : MonoBehaviour
    {
        [SerializeField] private float _timeBeforeDisable;
        
        private Coroutine _coroutine;

        private void OnEnable()
        {
            _coroutine = StartCoroutine(DisableAfterTime());
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);    
        }

        private IEnumerator DisableAfterTime()
        {
            yield return new WaitForSeconds(_timeBeforeDisable);
            gameObject.SetActive(false);
        }
    }
}