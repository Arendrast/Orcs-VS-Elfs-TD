using System;
using System.Collections;
using Modules.EntityModule.Scripts.Attack;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherArrowSpawnerComponent : MonoBehaviour
    {
        [field: SerializeField] public ArcherArrowMovementComponent ArcherPrefab { get; private set; }
        [SerializeField] private AttackComponent _attackComponent;
        [SerializeField] private Transform _spawnPointTransform;
        [SerializeField] private float _spawnDelayAfterStartAttack;
        [SerializeField] private float _maxFlyTime = 1f;
        [SerializeField] private float _distanceDividerForFlyTime = 25f;

        private PlayerArcherArrowFactory _playerArcherArrowFactory;

        private void OnDisable()
        {
            if (_attackComponent.AttackModel != null)
            {
                _attackComponent.AttackModel.StartedAttackByConfig -= TrySpawnArcher;
            }
        }

        public void Construct(PlayerArcherArrowFactory playerArcherArrowFactory)
        {
            _playerArcherArrowFactory = playerArcherArrowFactory;
            _attackComponent.AttackModel.StartedAttackByConfig += TrySpawnArcher;
        }

        private IEnumerator TryDelayedSpawnArcher(IAttackConfig attackConfig)
        {
            var targetData = _attackComponent.AttackModel.TargetData;

            if (!targetData.HasValue)
            {
                yield break;
            }
            
            yield return new WaitForSeconds(_spawnDelayAfterStartAttack);

            ArcherArrowMovementController archerArrowMovementController = null;
            var damage = attackConfig.Damage;

            if (targetData.Value.Damageable.IsDied)
            {
                yield break;
            }
                
            var sqrDistance =
                (targetData.Value.Transform.position - transform.position).sqrMagnitude;
                
            var flyTime = Mathf.Min(_maxFlyTime, Mathf.Max(sqrDistance, ConstantsHolder.Epsilon) / _distanceDividerForFlyTime);
                
            archerArrowMovementController = _playerArcherArrowFactory.GetArcherArrowMovementController(ArcherPrefab,
                targetData.Value.Transform, flyTime, _spawnPointTransform.position, 
                OnEndMovement);
            
            yield break;
            
            void OnEndMovement()
            {
                targetData.Value.Damageable.TryTakeDamage(damage);
                archerArrowMovementController.Transform.gameObject.SetActive(false);
            }
        }
        private void TrySpawnArcher(IAttackConfig attackConfig)
        {
            StartCoroutine(TryDelayedSpawnArcher(attackConfig));
        }
    }
}