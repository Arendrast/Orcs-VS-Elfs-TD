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
        public event Action<MergeUnitComponent> SelectedUnitComponent, DeselectedUnitComponent;

        private readonly IReadOnlyDictionary<MergeCellComponent, MergeCellModel> _mergeCellModelByComponent;

        private MergeCellModel _selectedMergeCellModel;
        private bool _isHolding;

        private readonly Camera _camera;
        private readonly DragAndDropGridConfig _config;
        private readonly MergeGridModel _mergeGridModel;
        private readonly Func<bool> _canMoveOrMergeFunc;
        private readonly IInputService _inputService;
        private readonly UpgradeUnitVfxComponent _upgradeUnitVfxComponent;

        public DragAndDropGridController(Camera camera, DragAndDropGridConfig config,
            MergeGridModel mergeGridModel, Func<bool> canMoveOrMergeFunc, IInputService inputService,
            UpgradeUnitVfxComponent upgradeUnitVfxComponent)
        {
            _camera = camera;
            _config = config;
            _mergeGridModel = mergeGridModel;
            _canMoveOrMergeFunc = canMoveOrMergeFunc;
            _inputService = inputService;
            _upgradeUnitVfxComponent = upgradeUnitVfxComponent;

            _mergeCellModelByComponent =
                mergeGridModel.Cells.ToDictionary(cell => cell.MergeCellComponent, cell => cell);

            _mergeGridModel.BeforeUpgradeCellUnit += DisableUnitsAndEnableVfx;
        }

        private void DisableUnitsAndEnableVfx(MergeCellModel cellModel1, MergeCellModel cellModel2)
        {
            cellModel1.TargetUnit.Component.gameObject.SetActive(false);
            cellModel2.TargetUnit.Component.gameObject.SetActive(false);
            EnableVfx(cellModel2);
        }

        private void EnableVfx(MergeCellModel cellModel2)
        {
            _upgradeUnitVfxComponent.gameObject.SetActive(false);
            _upgradeUnitVfxComponent.transform.position =
                cellModel2.TargetUnit.Component.transform.position + Vector3.up / 2;
            _upgradeUnitVfxComponent.gameObject.SetActive(true);
        }

        public void Dispose()
        {
            UnsubscribeFromInputService();
            _mergeGridModel.BeforeUpgradeCellUnit -= DisableUnitsAndEnableVfx;
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
            if (_selectedMergeCellModel?.TargetUnit == null || !CanMoveOrMerge())
            {
                return;
            }
            

            if (RaycastValidCellComponent(out var raycastHit, out var mergeCellModel) &&
                _mergeGridModel.TryMergeCells(_selectedMergeCellModel, mergeCellModel, out var didMoveTargetUnit))
            {
                _selectedMergeCellModel = mergeCellModel;
            }
            
            var unit = _selectedMergeCellModel.TargetUnit.Component;
            
            _selectedMergeCellModel.TryReturnUnitToCellPoint();
            _selectedMergeCellModel = null;
            
            DeselectedUnitComponent?.Invoke(unit);
        }

        private void SetIsHolding(InputAction.CallbackContext callbackContext)
        {
            _isHolding = !_isHolding;
        }

        private void TrySelectMergeCell(InputAction.CallbackContext callbackContext)
        {
            if (_selectedMergeCellModel == null && CanMoveOrMerge() &&
                RaycastValidCellComponent(out var raycastHit, out var mergeCellModel) && mergeCellModel.TargetUnit != null)
            {
                _selectedMergeCellModel = mergeCellModel;
                SelectedUnitComponent?.Invoke(_selectedMergeCellModel.TargetUnit.Component);
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