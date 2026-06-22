using System.Linq;
using Modules.EnemyModule.Scripts.Orc;
using Modules.EntityModule.Scripts.Damageable;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class GameplayGameState
    {
        private readonly GameplayComponents _gameplayComponents;

        private readonly PlayerArcherFactory _playerArcherFactory;
        private readonly OrcEnemyFactory _orcEnemyFactory;
        private readonly MergeUnitFactory _mergeUnitFactory;
        private readonly Camera _camera;
        private readonly IInputService _inputService;

        public GameplayGameState(GameplayComponents gameplayComponents,
            PlayerArcherFactory playerArcherFactory, OrcEnemyFactory orcEnemyFactory, MergeUnitFactory mergeUnitFactory,
            Camera camera, IInputService inputService)
        {
            _gameplayComponents = gameplayComponents;
            _playerArcherFactory = playerArcherFactory;
            _orcEnemyFactory = orcEnemyFactory;
            _mergeUnitFactory = mergeUnitFactory;
            _camera = camera;
            _inputService = inputService;
        }

        public void Enter()
        {
            InitializePlayerUnits();
            InitializeEnemies();
            InitializeMergeGrid();
        }

        private void InitializeMergeGrid()
        {
            var mergeCells = _gameplayComponents.MergeGridComponent.CellComponents.Select(cell =>
                new MergeCellModel(cell.StartUnit != null
                    ? new MergeUnitModel(
                        _gameplayComponents.MergeGridComponent.GridConfig.GetMergeUnitId(cell.StartUnit),
                        cell.StartUnit)
                    : null, cell)).ToArray();

            var mergeGridModel = new MergeGridModel(mergeCells, _mergeUnitFactory.GetUpgradedMergeUnitModel);

            _gameplayComponents.DragAndDropMergeGridComponent.Construct(_camera, mergeGridModel, CanMoveOrMerge, _inputService);
        }

        private bool CanMoveOrMerge()
        {
            return _gameplayComponents.OrcEnemyComponents.All(component => component.OrcEnemyLogicComponent.MovementComponent.Model == null ||
                                                                           !component.OrcEnemyLogicComponent.MovementComponent.Model.DoesEndPath());
        }

        private void InitializePlayerUnits()
        {
            foreach (var component in _gameplayComponents.PlayerArcherComponents)
            {
                _playerArcherFactory.InitializePlayerArcherInstance(component);
            }
        }

        private void InitializeEnemies()
        {
            foreach (var component in _gameplayComponents.OrcEnemyComponents)
            {
                _orcEnemyFactory.InitializeOrcEnemyInstance(component);
            }
        }
    }
}