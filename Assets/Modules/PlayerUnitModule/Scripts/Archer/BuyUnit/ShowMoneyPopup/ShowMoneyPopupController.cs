using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Modules.EntityModule.Scripts.Damageable;
using Modules.SharedModule.Scripts;
using Modules.SharedModule.Scripts.Currencies;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup
{
    public class ShowMoneyPopupController : IDisposable
    {
        private readonly ShowMoneyPopupConfig _config;
        private readonly MoneyImageFactory _moneyImageFactory;
        private readonly BuyMergeUnitConfig _buyMergeUnitConfig;

        private readonly Dictionary<IDamageable, Action> _subscribedActions = new Dictionary<IDamageable, Action>();
        private readonly Camera _camera;
        private readonly CurrencyRepositoryService _currencyRepositoryService;

        public ShowMoneyPopupController(ShowMoneyPopupConfig config, MoneyImageFactory moneyImageFactory,
            DamageablesRepository enemyDamageablesRepository, Camera camera, BuyMergeUnitConfig buyMergeUnitConfig,
            CurrencyRepositoryService currencyRepositoryService, MonoBehaviour coroutineRunner)
        {
            _config = config;
            _moneyImageFactory = moneyImageFactory;
            _camera = camera;
            _buyMergeUnitConfig = buyMergeUnitConfig;
            _currencyRepositoryService = currencyRepositoryService;

            foreach (var damageablePair in enemyDamageablesRepository.Damageables)
            {
                var action = new Action(() => coroutineRunner.StartCoroutine(StartSpawnCoinsCoroutine(damageablePair.Key.transform.position)));

                _subscribedActions.Add(damageablePair.Value, action);
                damageablePair.Value.Died += action;
            }
        }

        public void Dispose()
        {
            foreach (var pair in _subscribedActions)
            {
                pair.Key.Died -= pair.Value;
            }

            _subscribedActions.Clear();
        }

        private IEnumerator StartSpawnCoinsCoroutine(Vector3 worldPoint)
        {
            var moneyCount = _buyMergeUnitConfig.FirstUnitBuyPrice;

            for (var i = 0; i < moneyCount; i++)
            {
                SpawnAndFlyCoin(_moneyImageFactory
                    .GetMoneyImage(_config.MoneyImagePrefab,
                        _camera.WorldToScreenPoint(worldPoint) + new Vector3(
                            Random.Range(-_config.MaximalMoneySpawnOffset.x, _config.MaximalMoneySpawnOffset.x), 
                            Random.Range(-_config.MaximalMoneySpawnOffset.y, _config.MaximalMoneySpawnOffset.y), 0), _config.CanvasTransform));
            }

            yield return new WaitForSeconds(_config.MoveMoneyDuration);

            _currencyRepositoryService.MakeOperationOnCurrencyNumber(_buyMergeUnitConfig.FirstUnitBuyPrice,
                CurrencyRepositoryService.SetCurrencyOperation.Increase);
        }

        private void SpawnAndFlyCoin(Image coin)
        {
            Vector3 startPos = coin.transform.position;
            Vector3 targetPosition = _config.MoneyAnimationEndPointTransform.position;

            var direction = (targetPosition - startPos).normalized;

            var perpendicular = new Vector3(-direction.y, direction.x, 0f);

            var backPoint = startPos - direction * _config.PullbackForce +
                            perpendicular * (_config.PullbackForce * 0.3f);

            var midPoint = Vector3.Lerp(startPos, targetPosition, 0.5f) + perpendicular * _config.ArcOffset;

            var endPoint = targetPosition;

            var path = new Vector3[] { backPoint, midPoint, endPoint };

            coin.rectTransform.localScale = Vector3.zero;
            coin.rectTransform.DOScale(1.2f, 0.2f).OnComplete(() => coin.rectTransform.DOScale(1f, 0.2f));

            coin.rectTransform.DOPath(path, _config.MoveMoneyDuration, PathType.CatmullRom)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _config.MoneyAnimationEndPointTransform.DOScale(1.2f, 0.1f).OnComplete(() =>
                        _config.MoneyAnimationEndPointTransform.DOScale(1f, 0.1f));
                    _moneyImageFactory.ReleaseMoneyImage(coin);
                });

            coin.rectTransform.DORotate(new Vector3(0, 0, 360), _config.MoveMoneyDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);

            coin.rectTransform.DOScale(0.5f, 0.2f).SetDelay(_config.MoveMoneyDuration - 0.2f);
        }
    }
}