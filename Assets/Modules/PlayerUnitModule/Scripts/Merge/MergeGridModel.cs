using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeGridModel // Сделал все через модели, чтобы была возможность запускать тесты мёрджа без юнити цикла.
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

            if (mergeCellModel1.HasTargetUnit() && mergeCellModel2.HasTargetUnit())
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

            BeforeUpgradeCellUnit?.Invoke(mergeCellModel1, mergeCellModel2);
            
            mergeCellModel1.TargetUnit = null;
            mergeCellModel2.TargetUnit = upgradedUnit;

            UpgradedCellUnit?.Invoke(mergeCellModel1, mergeCellModel2);
            
            return true;
        }

        private void MoveUnitBetweenCells(MergeCellModel mergeCellModel1, MergeCellModel mergeCellModel2)
        {
            if (!mergeCellModel1.HasTargetUnit())
            {
                mergeCellModel1.TargetUnit = mergeCellModel2.TargetUnit;
            }
            else
            {
                mergeCellModel2.TargetUnit = mergeCellModel1.TargetUnit;
            }
        }
    }
}