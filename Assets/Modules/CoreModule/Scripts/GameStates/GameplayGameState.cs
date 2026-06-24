using System.Collections;
using System.Linq;
using Modules.EnemyModule.Scripts.Orc;
using Modules.EntityModule.Scripts.Damageable;
using Modules.PlayerUnitModule.Scripts.Archer;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Currencies;
using Modules.SharedModule.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class GameplayGameState
    {
        private bool _endedGame;

        private readonly GameplayStateComponents _gameplayStateComponents;
        private readonly TutorialGameSubState _tutorialGameSubState;

        private BackgroundMusicController _backgroundMusicController;
        private readonly OrcEnemyFactory _orcEnemyFactory;
        private readonly MergeUnitFactory _mergeUnitFactory;
        private readonly Camera _camera;
        private readonly IInputService _inputService;
        private readonly AudioService _audioService;
        private readonly MoneyImageFactory _moneyImageFactory;
        private readonly CurrencyRepositoryService _currencyRepositoryRepository;
        private readonly DamageablesRepository _enemyDamageablesRepository;
        private readonly DamageablesRepository _playerDamageablesRepository;
        private readonly TimeScaleRepositoryService _timeScaleRepository;
        private readonly CTAService _ctaService;

        public GameplayGameState(GameplayStateComponents gameplayStateComponents,
            OrcEnemyFactory orcEnemyFactory,
            MergeUnitFactory mergeUnitFactory, Camera camera, IInputService inputService,
            TutorialGameSubState tutorialGameSubState, AudioService audioService, MoneyImageFactory moneyImageFactory,
            DamageablesRepository enemyDamageablesRepository, DamageablesRepository playerDamageablesRepository,
            CurrencyRepositoryService currencyRepositoryRepository, TimeScaleRepositoryService timeScaleRepository,
            CTAService ctaService)
        {
            _gameplayStateComponents = gameplayStateComponents;
            _orcEnemyFactory = orcEnemyFactory;
            _mergeUnitFactory = mergeUnitFactory;
            _camera = camera;
            _inputService = inputService;
            _tutorialGameSubState = tutorialGameSubState;
            _audioService = audioService;
            _moneyImageFactory = moneyImageFactory;
            _enemyDamageablesRepository = enemyDamageablesRepository;
            _currencyRepositoryRepository = currencyRepositoryRepository;
            _timeScaleRepository = timeScaleRepository;
            _ctaService = ctaService;
            _playerDamageablesRepository = playerDamageablesRepository;
        }

        public void Enter()
        {
            InitializeEnemies();
            InitializeMergeGrid(out var mergeGridModel);

            _backgroundMusicController = new BackgroundMusicController(_audioService, _gameplayStateComponents);

            _gameplayStateComponents.ShowMoneyPopupComponent.Construct(_moneyImageFactory, _enemyDamageablesRepository,
                _camera,
                _gameplayStateComponents.BuyMergeUnitConfig, _currencyRepositoryRepository);

            _gameplayStateComponents.CurrenciesPopupComponent.Construct(_currencyRepositoryRepository);
            _gameplayStateComponents.CurrenciesPopupComponent.Controller.Open();

            _tutorialGameSubState.Enter(mergeGridModel);

            _backgroundMusicController.PlaySounds();
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
            return _tutorialGameSubState.CanMerge && !_endedGame;
        }

        private void InitializeEnemies()
        {
            foreach (var component in _gameplayStateComponents.OrcEnemyComponents)
            {
                _orcEnemyFactory.InitializeOrcEnemyInstance(component);
                component.OrcEnemyLogicComponent.MovementComponent.Initializer.TryInitialize();
                component.OrcEnemyLogicComponent.HealthComponent.Initializer.TryInitialize();
                component.OrcEnemyLogicComponent.MovementComponent.Controller.Model.EndedAllPath += OnLoose;
                component.OrcEnemyLogicComponent.HealthComponent.Model.Died += TryWin;
            }
        }

        private void TryWin()
        {
            if (_endedGame)
            {
                return;
            }

            if (_enemyDamageablesRepository.Damageables.All(damageable => damageable.Value.IsDied))
            {
                _gameplayStateComponents.StartCoroutine(StartOnWinCoroutine());
                _endedGame = true;
            }
        }

        private IEnumerator StartOnWinCoroutine()
        {
            _gameplayStateComponents.BuyUnitPopupComponent.Controller.SetCanBuyUnit(false);

            yield return new WaitForSecondsRealtime(_gameplayStateComponents.TimeAfterWinBeforeShowPopup);

            _gameplayStateComponents.EndPopupComponent.Controller.Open(_timeScaleRepository, true);
            _inputService.MouseClickInputAction.performed += InvokeCTA;
        }

        private IEnumerator StartOnLooseCoroutine()
        {
            yield return new WaitForSecondsRealtime(_gameplayStateComponents.TimeAfterReachEnemyEndPointBeforeLoose);

            if (_endedGame)
            {
                yield break;
            }

            if (AllWhoEndPathIsDead())
            {
                yield break;
            }
            
            _endedGame = true;

            _gameplayStateComponents.BuyUnitPopupComponent.Controller.SetCanBuyUnit(false);

            foreach (var enemyPair in _enemyDamageablesRepository.Damageables)
            {
                enemyPair.Value.SetIsDamageable(false);
            }

            yield return new WaitWhile(() =>
                _playerDamageablesRepository.Damageables.Any(damageable => !damageable.Value.IsDied));

            yield return new WaitForSecondsRealtime(_gameplayStateComponents.TimeAfterLooseBeforeShowPopup);

            _gameplayStateComponents.EndPopupComponent.Controller.Open(_timeScaleRepository, false);
            _inputService.MouseClickInputAction.performed += InvokeCTA;
        }

        private bool AllWhoEndPathIsDead()
        {
            return _gameplayStateComponents.OrcEnemyComponents
                .Where(component =>
                    component.OrcEnemyLogicComponent.MovementComponent.Model.DoesEndPath())
                .All(component =>
                    component.OrcEnemyLogicComponent.HealthComponent.Model.IsDied);
        }

        private void OnLoose()
        {
            _gameplayStateComponents.StartCoroutine(StartOnLooseCoroutine());
        }

        private void InvokeCTA(InputAction.CallbackContext ctx)
        {
            _ctaService.InvokeCTA();
        }
    }
}