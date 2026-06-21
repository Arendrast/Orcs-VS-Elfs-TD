using System;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    [CreateAssetMenu(fileName = nameof(MergeGridConfig), menuName = "Configs/" + nameof(MergeGridConfig))]
    public class MergeGridConfig : ScriptableObject
    {
        [field: SerializeField] public MergeUnitComponent[] MergeUnitComponents { get; private set; }

        public int GetMergeUnitId(MergeUnitComponent mergeUnitComponent)
        {
            return Array.IndexOf(MergeUnitComponents, mergeUnitComponent);
        }

        public int GetMaxId()
        {
            return MergeUnitComponents.Length;
        }
    }
}