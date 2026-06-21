using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class
        MergeGridModel // Сделал все через модели, чтобы была возможность запускать тесты мёрджа без юнити цикла.
    {
        public IReadOnlyList<MergeCellModel> Cells => _cells;

        public event Action<MergeCellModel, MergeCellModel> BeforeUpgradeCellUnit, UpgradedCellUnit;

        private readonly Func<MergeCellModel, MergeUnitModel> _getNewMergeUnitModelFunc;

        private readonly MergeCellModel[] _cells;

        public MergeGridModel(MergeCellModel[] cells, Func<MergeCellModel, MergeUnitModel> getNewMergeUnitModelFunc)
        {
            _cells = cells;
            _getNewMergeUnitModelFunc = getNewMergeUnitModelFunc;
        }

        public bool TryMergeCells(MergeCellModel mergeCellModel1, MergeCellModel mergeCellModel2,
            out bool didMoveTargetUnit)
        {
            didMoveTargetUnit = false;

            if (mergeCellModel1 == null || mergeCellModel2 == null || mergeCellModel1 == mergeCellModel2 ||
                !mergeCellModel1.HasTargetUnit() && !mergeCellModel2.HasTargetUnit())
            {
                return false;
            }

            if (!mergeCellModel1.HasTargetUnit() || !mergeCellModel2.HasTargetUnit() ||
                mergeCellModel1.TargetUnit.Id != mergeCellModel2.TargetUnit.Id)
            {
                MoveUnitBetweenCells(mergeCellModel1, mergeCellModel2);
                didMoveTargetUnit = true;
                return true;
            }

            var upgradedUnit = _getNewMergeUnitModelFunc.Invoke(mergeCellModel2);

            if (upgradedUnit == null)
            {
                return false;
            }

            MergeCells(mergeCellModel1, mergeCellModel2, upgradedUnit);
            return true;
        }

        private void MergeCells(MergeCellModel mergeCellModel1, MergeCellModel mergeCellModel2,
            MergeUnitModel upgradedUnit)
        {
            BeforeUpgradeCellUnit?.Invoke(mergeCellModel1, mergeCellModel2);

            mergeCellModel1.TargetUnit = null;
            mergeCellModel2.TargetUnit = upgradedUnit;

            UpgradedCellUnit?.Invoke(mergeCellModel1, mergeCellModel2);
        }

        private void MoveUnitBetweenCells(MergeCellModel mergeCellModel1, MergeCellModel mergeCellModel2)
        {
            var firstTargetUnit = mergeCellModel1.TargetUnit;
            var secondTargetUnit = mergeCellModel2.TargetUnit;

            mergeCellModel1.TargetUnit = secondTargetUnit;
            mergeCellModel2.TargetUnit = firstTargetUnit;
        }
    }
}