using System;
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
            _dragAndDropGridController.TryMoveSelectedUnitAndMergeUnits();
        }

        public void Construct(Camera camera, MergeGridModel mergeGridModel)
        {
            _dragAndDropGridController = new DragAndDropGridController(camera, _config, mergeGridModel);
        }
    }
}