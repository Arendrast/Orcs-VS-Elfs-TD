using System;
using System.Linq;
using Modules.CoreModule.Scripts.GameStates;
using Modules.EnemyModule.Scripts.Orc;
using Modules.EntityModule.Scripts.Damageable;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;
using InputActions = Modules.SharedModule.Scripts.Input.InputActions;

namespace Modules.CoreModule.Scripts
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameplayStateComponents _gameplayStateComponents;
        [SerializeField] private BootstrapStateComponents _bootstrapStateComponents;
        [SerializeField] private Camera _camera;
        [SerializeField] private AudioSource _mainAudioSource;
        [SerializeField] private BuyMergeUnitConfig _priceConfig;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var playerDamageablesRepository = new DamageablesRepository();
            var enemyDamageablesRepository = new DamageablesRepository();

            var audioService = new AudioService(_gameplayStateComponents.AudioConfig, _mainAudioSource);
            
            var currencyRepositoryService = new CurrencyRepositoryService();
            
            var archerArrowFactory = new PlayerArcherArrowFactory();

            var playerArcherFactory = new PlayerArcherFactory(archerArrowFactory, playerDamageablesRepository,
                enemyDamageablesRepository, audioService);

            var orcEnemyFactory = new OrcEnemyFactory(playerDamageablesRepository, enemyDamageablesRepository,
                _gameplayStateComponents, _camera, audioService);

            var mergeUnitFactory = new MergeUnitFactory(_gameplayStateComponents.MergeGridComponent.GridConfig,
                playerArcherFactory.GetPlayerArcherMergeUnitComponent);

            var inputService = new NewInputSystemService(new InputActions());

            var timeScaleRepositoryService = new TimeScaleRepositoryService();

            var tutorialGameSubState = new TutorialGameSubState(enemyDamageablesRepository,
                _gameplayStateComponents.TutorialGameSubStateComponents, _gameplayStateComponents.BuyMergeUnitConfig,
                _camera, _gameplayStateComponents, timeScaleRepositoryService, currencyRepositoryService, 
                _gameplayStateComponents.BuyUnitPopupComponent, mergeUnitFactory);

            var gamePlayState = new GameplayGameState(_gameplayStateComponents, orcEnemyFactory,
                mergeUnitFactory, _camera, inputService, tutorialGameSubState, audioService);

            var bootstrapGameState = new BootstrapGameState(gamePlayState, _bootstrapStateComponents);
            bootstrapGameState.Enter();
        }
    }
}