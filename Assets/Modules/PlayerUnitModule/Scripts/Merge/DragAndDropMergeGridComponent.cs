using System;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class DragAndDropMergeGridComponent : MonoBehaviour
    {
        [SerializeField] private DragAndDropGridConfig _config;
        [SerializeField] private MergeGridComponent _mergeGridComponent;

        private DragAndDropGridController _dragAndDropGridController;

        private void Update()
        {
            _dragAndDropGridController.TryMoveSelectedUnit();
        }

        private void OnEnable()
        {
            _dragAndDropGridController?.SubscribeToInputService();
        }

        private void OnDisable()
        {
            _dragAndDropGridController?.Dispose();
        }

        public void Construct(Camera camera, MergeGridModel mergeGridModel, Func<bool> canMoveOrMergeFunc,
            IInputService inputService)
        {
            _dragAndDropGridController = new DragAndDropGridController(camera, _config, mergeGridModel, canMoveOrMergeFunc, inputService);
            _dragAndDropGridController.SubscribeToInputService();
        }
    }
}