using Modules.PlayerUnitModule.Scripts.Merge;

namespace Modules.PlayerUnitModule.Scripts.Tests
{
    public static class Setup
    {
        private const int MaxUnitId = 2;
        private const int GridSize = 20;

        public static MergeGridModel MergeGrid(int[] mergeCellIds)
        {
            var models = new MergeCellModel[GridSize];

            for (var i = 0; i < models.Length; i++)
            {
                models[i] = new MergeCellModel(mergeCellIds.Length >= i + 1 ? new MergeUnitModel(mergeCellIds[i]) : null);
            }

            return new MergeGridModel(models, GetMergeUnitModel);
        }

        private static MergeUnitModel GetMergeUnitModel(MergeCellModel mergeCellForUpgrade)
        {
            if (mergeCellForUpgrade?.TargetUnit == null)
            {
                return null;
            }

            var upgradedUnitId = mergeCellForUpgrade.TargetUnit.Id + 1;

            if (upgradedUnitId == MaxUnitId)
            {
                return null;
            }

            return new MergeUnitModel(upgradedUnitId);
        }
    }
}