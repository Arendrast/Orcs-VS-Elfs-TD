using System.Collections;
using System.Linq;
using Modules.EntityModule.Scripts.Damageable;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Currencies;
using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class TutorialGameSubState
    {
        public bool CanMerge { get; private set; }

        private MergeGridModel _mergeGridModel;
        private BuyUnitModel _buyUnitModel;

        private readonly DamageablesRepository _enemyDamageablesRepository;
        private readonly TutorialGameSubStateComponents _components;
        private readonly MonoBehaviour _coroutineRunner;
        private readonly BuyMergeUnitConfig _buyMergeUnitConfig;
        private readonly Camera _camera;
        private readonly TimeScaleRepositoryService _timeScaleRepositoryService;
        private readonly CurrencyRepositoryService _currencyRepositoryService;
        private readonly BuyUnitPopupComponent _buyUnitPopupComponent;
        private readonly MergeUnitFactory _mergeUnitFactory;
        private readonly AudioService _audioService;

        public TutorialGameSubState(DamageablesRepository enemyDamageablesRepository,
            TutorialGameSubStateComponents components, BuyMergeUnitConfig buyMergeUnitConfig, Camera camera,
            MonoBehaviour coroutineRunner, TimeScaleRepositoryService timeScaleRepositoryService,
            CurrencyRepositoryService currencyRepositoryService, BuyUnitPopupComponent buyUnitPopupComponent,
            MergeUnitFactory mergeUnitFactory, AudioService audioService)
        {
            _enemyDamageablesRepository = enemyDamageablesRepository;
            _components = components;
            _buyMergeUnitConfig = buyMergeUnitConfig;
            _camera = camera;
            _coroutineRunner = coroutineRunner;
            _timeScaleRepositoryService = timeScaleRepositoryService;
            _currencyRepositoryService = currencyRepositoryService;
            _buyUnitPopupComponent = buyUnitPopupComponent;
            _mergeUnitFactory = mergeUnitFactory;
            _audioService = audioService;
        }

        public void Enter(MergeGridModel mergeGridModel)
        {
            _mergeGridModel = mergeGridModel;
            _coroutineRunner.StartCoroutine(StartEnterCoroutine());
        }

        private IEnumerator StartEnterCoroutine()
        {
            var firstEnemy = _enemyDamageablesRepository.Damageables.First().Value;

            yield return new WaitWhile(() => !firstEnemy.IsDied);

            yield return new WaitForSecondsRealtime(_components.DelayAfterKillFirstEnemyBeforeShowUI);

            _timeScaleRepositoryService.SetTimeScale(0);

            _buyUnitModel = new BuyUnitModel(_buyMergeUnitConfig, _currencyRepositoryService, _mergeGridModel);

            _components.BuyUnitTutorialPopupComponent.BuyUnitTutorialPopupController.Open(_buyMergeUnitConfig,
                _buyUnitModel, _audioService, _mergeUnitFactory, _buyMergeUnitConfig);
            _components.BuyUnitTutorialPopupComponent.BuyUnitTutorialPopupController.Closed += OnBuyFirstUnitTutorial;
        }

        private void OnBuyFirstUnitTutorial()
        {
            _coroutineRunner.StartCoroutine(StartOnBuyFirstUnitCoroutine());
        }

        private IEnumerator StartOnBuyFirstUnitCoroutine()
        {
            _components.BuyUnitTutorialPopupComponent.BuyUnitTutorialPopupController.Closed -= OnBuyFirstUnitTutorial;

            _timeScaleRepositoryService.SetTimeScale(1);

            var secondEnemy = _enemyDamageablesRepository.Damageables.Skip(1).First().Value;
            var thirdEnemy = _enemyDamageablesRepository.Damageables.Skip(2).First().Value;

            yield return new WaitWhile(() => !secondEnemy.IsDied || !thirdEnemy.IsDied);

            yield return new WaitForSecondsRealtime(_components.DelayAfterKillThirdEnemyBeforeShowUI);
            
            _timeScaleRepositoryService.SetTimeScale(0);

            CanMerge = true;

            var cellsWithTarget = _mergeGridModel.Cells.Where(cell => cell.HasTargetUnit()).ToList();

            _components.MergeUnitTutorialPopupComponent.PopupController.Open(cellsWithTarget[1].TargetUnit.Component,
                cellsWithTarget[0].TargetUnit.Component, _camera);

            _mergeGridModel.UpgradedCellUnit += OnUpgradeCellUnit;
        }

        private void OnUpgradeCellUnit(MergeCellModel firstCellModel, MergeCellModel secondCellModel)
        {
            _mergeGridModel.UpgradedCellUnit -= OnUpgradeCellUnit;

            _components.MergeUnitTutorialPopupComponent.PopupController.Close();

            _buyUnitPopupComponent.Construct(_buyUnitModel, _mergeUnitFactory, _buyMergeUnitConfig, _audioService,
                _currencyRepositoryService);
            _timeScaleRepositoryService.SetTimeScale(1);
        }
    }
}