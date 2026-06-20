using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Modules.SharedModule
{
    public class ActivatorAfterDelayComponent : MonoBehaviour
    {
        [SerializeField] private float _timeBeforeActivate;

        public void SetTimeBeforeActivate(float timeBeforeActivate)
        {
            _timeBeforeActivate = timeBeforeActivate;
        }

        public IEnumerator DelayedActivate()
        {
            yield return new WaitForSeconds(_timeBeforeActivate);
            gameObject.SetActive(true);
        }
    }
}