using System;
using System.Linq;
using Modules.PlayerUnitModule.Scripts.Archer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeUnitFactory
    {
        private readonly MergeGridConfig _mergeGridConfig;
        private readonly Func<MergeUnitComponent, Vector3, MergeUnitComponent> _spawnMergeUnitComponent;

        public MergeUnitFactory(MergeGridConfig mergeGridConfig,
            Func<MergeUnitComponent, Vector3, MergeUnitComponent> spawnMergeUnitComponent)
        {
            _mergeGridConfig = mergeGridConfig;
            _spawnMergeUnitComponent = spawnMergeUnitComponent;
        }

        public MergeUnitModel GetMergeUnitModel(MergeUnitComponent prefab, Vector3 position)
        {
            if (prefab == null)
            {
                return null;
            }
            
            if (!_mergeGridConfig.MergeUnitComponents.Contains(prefab))
            {
                Debug.LogError("Missing unit prefab in merge grid config");
                return null;
            }
            
            var instance = _spawnMergeUnitComponent.Invoke(prefab, position); 
            
            return new MergeUnitModel(prefab.Id, instance);
        }
        
        public MergeUnitModel GetUpgradedMergeUnitModel(MergeCellModel mergeCellForUpgrade)
        {
            if (mergeCellForUpgrade == null || !mergeCellForUpgrade.MergeCellComponent ||
                mergeCellForUpgrade.TargetUnit == null || !mergeCellForUpgrade.TargetUnit.Component)
            {
                return null;
            }

            var currentUnitId = _mergeGridConfig.GetMergeUnitId(mergeCellForUpgrade.TargetUnit.Component);

            if (currentUnitId == -1)
            {
                return null;
            }

            var upgradedUnitId = currentUnitId + 1;

            if (_mergeGridConfig.GetMaxId() == upgradedUnitId)
            {
                return null;
            }

            var instance = _spawnMergeUnitComponent.Invoke(_mergeGridConfig.MergeUnitComponents[currentUnitId + 1],
                mergeCellForUpgrade.MergeCellComponent.PositionTransform.position);

            return new MergeUnitModel(upgradedUnitId, instance);
        }
    }
}