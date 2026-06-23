using System;
using Modules.SharedModule.Scripts.Audio;

namespace Modules.PlayerUnitModule.Scripts.Merge
{
    public class MergeGridSoundsController : IDisposable
    {
        private readonly DragAndDropGridController _dragAndDropGridController;
        private readonly MergeGridModel _mergeGridModel;
        private readonly AudioService _audioService;

        public MergeGridSoundsController(DragAndDropGridController dragAndDropGridController,
            MergeGridModel mergeGridModel, AudioService audioService)
        {
            _dragAndDropGridController = dragAndDropGridController;
            _mergeGridModel = mergeGridModel;
            _audioService = audioService;

            _mergeGridModel.UpgradedCellUnit += PlayMergeUnitSound;
            _dragAndDropGridController.SelectedUnitComponent += PlaySelectUnitSound;
            _dragAndDropGridController.DeselectedUnitComponent += PlayDeselectUnitSound;
        }

        private void PlayMergeUnitSound(MergeCellModel firstCell, MergeCellModel secondCell)
        {
            _audioService.TryPlayOneShotForMainAudioSource(AudioId.UnitsMerge);
        }

        private void PlayDeselectUnitSound(MergeUnitComponent unit)
        {
            _audioService.TryPlayOneShotForMainAudioSource(AudioId.UnitDeselect);
        }

        private void PlaySelectUnitSound(MergeUnitComponent unit)
        {
            _audioService.TryPlayOneShotForMainAudioSource(AudioId.UnitSelect);
        }

        public void Dispose()
        {
            _mergeGridModel.UpgradedCellUnit -= PlayMergeUnitSound;
            _dragAndDropGridController.SelectedUnitComponent -= PlaySelectUnitSound;
            _dragAndDropGridController.DeselectedUnitComponent -= PlayDeselectUnitSound;
        }
    }
}