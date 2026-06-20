using System;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Health
{
    [Serializable]
    public class HealthConfig
    {
        [field: SerializeField] public int MaxHealth { get; set; }
    }
}