using System;
using Modules.CoreModule.Scripts.GameStates;
using Modules.EnemyModule.Scripts.Orc;
using Modules.EntityModule.Scripts.Damageable;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameplayComponents _gameplayComponents;
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var playerDamageablesRepository = new DamageablesRepository();
            var enemyDamageablesRepository = new DamageablesRepository();

            var archerArrowFactory = new PlayerArcherArrowFactory();
            var playerArcherFactory = new PlayerArcherFactory(archerArrowFactory, playerDamageablesRepository,
                enemyDamageablesRepository);

            var orcEnemyFactory = new OrcEnemyFactory(playerDamageablesRepository, enemyDamageablesRepository,
                _gameplayComponents, _camera);

            var mergeUnitFactory = new MergeUnitFactory(_gameplayComponents.MergeGridComponent.GridConfig,
                playerArcherFactory.GetCreatedPlayerArcherMergeUnitComponent);

            var inputService = new NewInputSystemService(new InputActions());

            var gamePlayState = new GameplayGameState(_gameplayComponents, playerArcherFactory, orcEnemyFactory,
                mergeUnitFactory, _camera, inputService);

            new BootstrapGameState(gamePlayState).Enter();
        }
    }
}