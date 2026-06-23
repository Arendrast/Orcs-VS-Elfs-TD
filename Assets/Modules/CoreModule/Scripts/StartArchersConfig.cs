using System;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Merge;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    [Serializable]
    public class StartArchersConfig
    {
        [field: SerializeField] public MergeCellComponent GridCellComponent { get; private set; }
        [field: SerializeField] public PlayerArcherComponents Prefab { get; private set; }
    }
}