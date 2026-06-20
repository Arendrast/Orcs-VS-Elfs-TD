using System;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    [Serializable]
    public class PathMovementConfig
    {
        [field: Header("Скорость в метрах в секунду")] 
        [field: SerializeField]
        public float Speed { get; private set; } = 5;
        public bool StartMovementOnAwake { get; private set; } = true;
    }
}