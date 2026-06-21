namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeCellModel
    {
        public MergeUnitModel TargetUnit { get; set; }
        public readonly MergeCellComponent MergeCellComponent;
        
        public MergeCellModel(MergeUnitModel mergeUnitModel, MergeCellComponent mergeCellComponent = null)
        {
            TargetUnit = mergeUnitModel;
            MergeCellComponent = mergeCellComponent;
            
            TryReturnUnitToCellPoint();
        }
        
        public bool HasTargetUnit()
        {
            return TargetUnit != null;
        }

        public void TryReturnUnitToCellPoint()
        {
            if (TargetUnit != null && TargetUnit.Component != null && MergeCellComponent != null)
            {
                TargetUnit.Component.transform.position = MergeCellComponent.PositionTransform.position;
            }
        }
    }
}