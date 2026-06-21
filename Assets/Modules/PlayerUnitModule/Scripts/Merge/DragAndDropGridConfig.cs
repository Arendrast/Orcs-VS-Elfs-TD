using System;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    [Serializable]
    public class DragAndDropGridConfig
    {
        [field: SerializeField] public LayerMask UnitLayerMask { get; private set; }
        [field: SerializeField] public LayerMask GridLayerMask { get; private set; }
    }
}