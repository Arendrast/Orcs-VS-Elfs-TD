using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class DragAndDropGridController
    {
        private IReadOnlyDictionary<MergeCellComponent, MergeCellModel> _mergeCellModelByComponent;
        
        private MergeCellModel _selectedMergeCellModel;

        private readonly Camera _camera;
        private readonly DragAndDropGridConfig _config;
        private readonly MergeGridModel _mergeGridModel;

        public DragAndDropGridController(Camera camera, DragAndDropGridConfig config,
            MergeGridModel mergeGridModel)
        {
            _camera = camera;
            _config = config;
            _mergeGridModel = mergeGridModel;
            
            _mergeCellModelByComponent =
                mergeGridModel.Cells.ToDictionary(cell => cell.MergeCellComponent, cell => cell);
        }

        public void TryMoveSelectedUnitAndMergeUnits()
        {
            TryMoveSelectedUnit();
            
            if (TrySelectMergeCell())
            {
                return;
            }
            
            TryMergeOrMoveUnits();
        }

        private void TryMoveSelectedUnit()
        {
            if (_selectedMergeCellModel != null && RaycastGrid(out var hit))
            {
                _selectedMergeCellModel.TargetUnit.Component.transform.position = hit.point;
            }
        }

        private void TryMergeOrMoveUnits()
        {
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            if (RaycastValidCellComponent(out var raycastHit, out var mergeCellModel) &&
                _mergeGridModel.TryMergeCells(_selectedMergeCellModel, mergeCellModel, out var didMoveTargetUnit) &&
                didMoveTargetUnit)
            {
                _selectedMergeCellModel = mergeCellModel;
            }

            _selectedMergeCellModel.TryReturnUnitToCellPoint();
            _selectedMergeCellModel = null;
        }

        private bool TrySelectMergeCell()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return false;
            }

            if (_selectedMergeCellModel == null &&
                RaycastValidCellComponent(out var raycastHit, out var mergeCellModel))
            {
                _selectedMergeCellModel = mergeCellModel;
            }

            return true;
        }

        private bool RaycastValidCellComponent(out RaycastHit hit, out MergeCellModel mergeCellModel)
        {
            mergeCellModel = null;
            return RaycastGrid(out hit) &&
                   hit.transform.TryGetComponent<MergeCellComponent>(out var mergeCellComponent) &&
                   _mergeCellModelByComponent.TryGetValue(mergeCellComponent, out mergeCellModel);
        }

        private bool RaycastGrid(out RaycastHit hit)
        {
            return Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition),
                out hit, 1000, _config.GridLayerMask,
                QueryTriggerInteraction.Collide);
        }
    }
}