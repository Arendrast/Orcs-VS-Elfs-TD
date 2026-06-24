using System;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    [Serializable]
    public class PathMovementConfig
    {
        [field: Header("Meters per second")] 
        [field: SerializeField]
        public float Speed { get; private set; } = 5;
    }
}