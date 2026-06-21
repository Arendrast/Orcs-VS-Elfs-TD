using System;
using System.Collections.Generic;
using System.Linq;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class DragAndDropGridController : IDisposable
    {
        private readonly IReadOnlyDictionary<MergeCellComponent, MergeCellModel> _mergeCellModelByComponent;

        private MergeCellModel _selectedMergeCellModel;
        private bool _isHolding;
        
        private readonly Camera _camera;
        private readonly DragAndDropGridConfig _config;
        private readonly MergeGridModel _mergeGridModel;
        private readonly Func<bool> _canMoveOrMergeFunc;
        private readonly IInputService _inputService;

        public DragAndDropGridController(Camera camera, DragAndDropGridConfig config,
            MergeGridModel mergeGridModel, Func<bool> canMoveOrMergeFunc, IInputService inputService)
        {
            _camera = camera;
            _config = config;
            _mergeGridModel = mergeGridModel;
            _canMoveOrMergeFunc = canMoveOrMergeFunc;
            _inputService = inputService;

            _mergeCellModelByComponent =
                mergeGridModel.Cells.ToDictionary(cell => cell.MergeCellComponent, cell => cell);

            _mergeGridModel.BeforeUpgradeCellUnit += DisableUnits;
        }

        private void DisableUnits(MergeCellModel cellModel1, MergeCellModel cellModel2)
        {
            cellModel1.TargetUnit.Component.gameObject.SetActive(false);
            cellModel2.TargetUnit.Component.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            UnsubscribeFromInputService();
            _mergeGridModel.BeforeUpgradeCellUnit -= DisableUnits;
        }

        public void SubscribeToInputService()
        {
            _inputService.MouseClickInputAction.performed += TrySelectMergeCell;
            _inputService.MouseClickInputAction.performed += SetIsHolding;
            _inputService.MouseClickInputAction.canceled += SetIsHolding;
            _inputService.MouseClickInputAction.canceled += TryMergeOrMoveUnits;
        }

        public void UnsubscribeFromInputService()
        {
            _inputService.MouseClickInputAction.performed -= TrySelectMergeCell;
            _inputService.MouseClickInputAction.performed -= SetIsHolding;
            _inputService.MouseClickInputAction.canceled -= SetIsHolding;
            _inputService.MouseClickInputAction.canceled -= TryMergeOrMoveUnits;
            _isHolding = false;
        }

        public void TryMoveSelectedUnit()
        {
            if (!CanMoveOrMerge())
            {
                return;
            }
            
            if (_selectedMergeCellModel?.TargetUnit != null && Raycast(out var hit, _config.GridLayerMask))
            {
                _selectedMergeCellModel.TargetUnit.Component.transform.position = hit.point;
            }
        }

        private bool CanMoveOrMerge()
        {
            if (_canMoveOrMergeFunc.Invoke())
            {
                return true;
            }
            
            _selectedMergeCellModel?.TryReturnUnitToCellPoint();
            _selectedMergeCellModel = null;
            return false;
        }

        private void TryMergeOrMoveUnits(InputAction.CallbackContext callbackContext)
        {
            if (_selectedMergeCellModel == null)
            {
                return;
            }
            
            if (RaycastValidCellComponent(out var raycastHit, out var mergeCellModel) &&
                _mergeGridModel.TryMergeCells(_selectedMergeCellModel, mergeCellModel, out var didMoveTargetUnit) && didMoveTargetUnit)
            {
                _selectedMergeCellModel = mergeCellModel;
            }

            _selectedMergeCellModel.TryReturnUnitToCellPoint();
            _selectedMergeCellModel = null;
        }

        private void SetIsHolding(InputAction.CallbackContext callbackContext)
        {
            _isHolding = !_isHolding;
        }

        private void TrySelectMergeCell(InputAction.CallbackContext callbackContext)
        {
            if (_selectedMergeCellModel == null &&
                RaycastValidCellComponent(out var raycastHit, out var mergeCellModel))
            {
                _selectedMergeCellModel = mergeCellModel;
            }
        }

        private bool RaycastValidCellComponent(out RaycastHit hit, out MergeCellModel mergeCellModel)
        {
            mergeCellModel = null;
            return Raycast(out hit, _config.CellLayerMask) &&
                   hit.transform.TryGetComponent<MergeCellComponent>(out var mergeCellComponent) &&
                   _mergeCellModelByComponent.TryGetValue(mergeCellComponent, out mergeCellModel);
        }

        private bool Raycast(out RaycastHit hit, LayerMask layerMask)
        {
            #if UNITY_STANDALONE
            var mousePosition = Mouse.current.position.ReadValue();
            #else
            var mousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
            #endif
            
            return Physics.Raycast(_camera.ScreenPointToRay(mousePosition),
                out hit, _config.RaycastMaxDistance, layerMask,
                QueryTriggerInteraction.Collide);
        }
    }
}