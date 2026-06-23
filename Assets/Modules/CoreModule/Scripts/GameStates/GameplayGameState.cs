using System.Linq;
using Modules.EnemyModule.Scripts.Orc;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class GameplayGameState
    {
        private readonly GameplayStateComponents _gameplayStateComponents;
        private readonly TutorialGameSubState _tutorialGameSubState;

        private readonly OrcEnemyFactory _orcEnemyFactory;
        private readonly MergeUnitFactory _mergeUnitFactory;
        private readonly Camera _camera;
        private readonly IInputService _inputService;
        private readonly AudioService _audioService;

        public GameplayGameState(GameplayStateComponents gameplayStateComponents,
            OrcEnemyFactory orcEnemyFactory,
            MergeUnitFactory mergeUnitFactory, Camera camera, IInputService inputService,
            TutorialGameSubState tutorialGameSubState, AudioService audioService)
        {
            _gameplayStateComponents = gameplayStateComponents;
            _orcEnemyFactory = orcEnemyFactory;
            _mergeUnitFactory = mergeUnitFactory;
            _camera = camera;
            _inputService = inputService;
            _tutorialGameSubState = tutorialGameSubState;
            _audioService = audioService;
        }

        public void Enter()
        {
            InitializeEnemies();
            InitializeMergeGrid(out var mergeGridModel);

            new BackgroundMusicController(_audioService, _gameplayStateComponents);
            
            _tutorialGameSubState.Enter(mergeGridModel);
        }

        private void InitializeMergeGrid(out MergeGridModel mergeGridModel)
        {
            var mergeCells = _gameplayStateComponents.MergeGridComponent.CellComponents.Select(cell =>
                new MergeCellModel(
                    _mergeUnitFactory.GetMergeUnitModel(cell.StartUnitPrefab, cell.PositionTransform.position),
                    cell)).ToArray();

            mergeGridModel = new MergeGridModel(mergeCells, _mergeUnitFactory.GetUpgradedMergeUnitModel);

            _gameplayStateComponents.DragAndDropMergeGridComponent.Construct(_camera, mergeGridModel, CanMoveOrMerge,
                _inputService);

            new MergeGridSoundsController(_gameplayStateComponents.DragAndDropMergeGridComponent.Controller,
                mergeGridModel, _audioService);
        }

        private bool CanMoveOrMerge()
        {
            return _tutorialGameSubState.CanMerge && _gameplayStateComponents.OrcEnemyComponents.All(component =>
                component.OrcEnemyLogicComponent.MovementComponent.Model == null ||
                !component.OrcEnemyLogicComponent.MovementComponent.Model.DoesEndPath());
        }

        private void InitializeEnemies()
        {
            foreach (var component in _gameplayStateComponents.OrcEnemyComponents)
            {
                _orcEnemyFactory.InitializeOrcEnemyInstance(component);
            }
        }
    }
}