using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeUnitFactory
    {
        private readonly MergeGridConfig _mergeGridConfig;

        public MergeUnitFactory(MergeGridConfig mergeGridConfig)
        {
            _mergeGridConfig = mergeGridConfig;
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
            
            var instance = Object.Instantiate(_mergeGridConfig.MergeUnitComponents[currentUnitId + 1],
                mergeCellForUpgrade.MergeCellComponent.PositionTransform.position, Quaternion.identity);

            return new MergeUnitModel(instance, upgradedUnitId);
        }
    }
}