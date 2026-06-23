using Modules.EntityModule.Scripts.Damageable;
using Modules.SharedModule.Scripts.Audio;
using Unity.VisualScripting;
using UnityEngine;

namespace Modules.EnemyModule.Scripts.Orc
{
    public class OrcEnemyFactory
    {
        private readonly DamageablesRepository _playerDamageablesRepository;
        private readonly DamageablesRepository _enemyDamageablesRepository;
        private readonly MonoBehaviour _coroutineRunner;
        private readonly Camera _camera;
        private readonly AudioService _audioService;

        public OrcEnemyFactory(DamageablesRepository playerDamageablesRepository,
            DamageablesRepository enemyDamageablesRepository, MonoBehaviour coroutineRunner, Camera camera,
            AudioService audioService)
        {
            _playerDamageablesRepository = playerDamageablesRepository;
            _enemyDamageablesRepository = enemyDamageablesRepository;
            _coroutineRunner = coroutineRunner;
            _camera = camera;
            _audioService = audioService;
        }

        // Я отказался от спавна врагов в пользу производительности
        public OrcEnemyComponents GetCreatedOrcEnemy(OrcEnemyComponents prefab, Vector3 position)
        {
            var instance = Object.Instantiate(prefab, position, Quaternion.identity);
            InitializeOrcEnemyInstance(instance);
            return instance;
        }

        public void InitializeOrcEnemyInstance(OrcEnemyComponents components)
        {
            _enemyDamageablesRepository.TryAdd(components.gameObject,
                components.OrcEnemyLogicComponent.DamageableComponent);

            components.OrcEnemyLogicComponent.AttackComponent.Construct(_playerDamageablesRepository,
                components.OrcEnemyLogicComponent.AttackComponent.CanSkipAttack);
            _coroutineRunner.StartCoroutine(components.ActivatorAfterDelayComponent.DelayedActivate());
            components.EntitySoundsComponent.Construct(_audioService);
            components.ToCameraLookerComponent.Construct(_camera);
        }
    }
}