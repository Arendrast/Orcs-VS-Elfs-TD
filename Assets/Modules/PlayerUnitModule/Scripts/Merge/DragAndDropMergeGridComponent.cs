using System;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class DragAndDropMergeGridComponent : MonoBehaviour
    {
        public DragAndDropGridController Controller { get; private set; }
        
        [SerializeField] private DragAndDropGridConfig _config;
        [SerializeField] private MergeGridComponent _mergeGridComponent;
        [SerializeField] private UpgradeUnitVfxComponent _upgradeUnitVfxComponent;


        private void Update()
        {
            Controller.TryMoveSelectedUnit();
        }

        private void OnEnable()
        {
            Controller?.SubscribeToInputService();
        }

        private void OnDisable()
        {
            Controller?.Dispose();
        }

        public void Construct(Camera camera, MergeGridModel mergeGridModel, Func<bool> canMoveOrMergeFunc,
            IInputService inputService)
        {
            Controller = new DragAndDropGridController(camera, _config, mergeGridModel,
                canMoveOrMergeFunc, inputService, _upgradeUnitVfxComponent);
            Controller.SubscribeToInputService();
        }
    }
}