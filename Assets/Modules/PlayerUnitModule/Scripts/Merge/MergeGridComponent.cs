using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeGridComponent : MonoBehaviour
    {
        [field: SerializeField] public MergeCellComponent[] CellComponents { get; private set; }
        [field: SerializeField] public MergeGridConfig GridConfig { get; private set; }
    }
}