using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeCellComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform PositionTransform { get; private set; }
        [field: SerializeField] public MergeUnitComponent StartUnit { get; private set; }
    }
}