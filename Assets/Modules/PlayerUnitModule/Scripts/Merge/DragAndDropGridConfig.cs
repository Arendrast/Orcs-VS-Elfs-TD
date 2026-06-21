using System;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    [Serializable]
    public class DragAndDropGridConfig
    {
        [field: SerializeField] public float RaycastMaxDistance { get; private set; } = 1000;
        [field: SerializeField] public LayerMask CellLayerMask { get; private set; }
        [field: SerializeField] public LayerMask GridLayerMask { get; private set; }
    }
}