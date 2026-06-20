using Modules.EntityModule.Scripts.Damageable;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class GameplayGameState
    {
        private readonly DamageablesRepository _playerDamageablesRepository;
        private readonly DamageablesRepository _enemyDamageablesRepository;
        private readonly GameplayComponents _gameplayComponents;

        public GameplayGameState(DamageablesRepository playerDamageablesRepository,
            DamageablesRepository enemyDamageablesRepository,
            GameplayComponents gameplayComponents)
        {
            _playerDamageablesRepository = playerDamageablesRepository;
            _enemyDamageablesRepository = enemyDamageablesRepository;
            _gameplayComponents = gameplayComponents;
        }

        public void Enter()
        {
            InitializePlayerUnits();
            InitializeEnemies();
        }

        private void InitializePlayerUnits()
        {
            foreach (var damageableComponent in _gameplayComponents.PlayerDamageableComponents)
            {
                _playerDamageablesRepository.TryAdd(damageableComponent.gameObject, damageableComponent);
            }

            foreach (var attackComponent in _gameplayComponents.PlayerAttackComponents)
            {
                attackComponent.Construct(_playerDamageablesRepository);
            }
        }

        private void InitializeEnemies()
        {
            foreach (var damageableComponent in _gameplayComponents.EnemyDamageableComponents)
            {
                _enemyDamageablesRepository.TryAdd(damageableComponent.gameObject, damageableComponent);
            }

            foreach (var attackComponent in _gameplayComponents.EnemyAttackComponents)
            {
                attackComponent.Construct(_playerDamageablesRepository);
            }
            
            foreach (var activator in _gameplayComponents.EnemyActivatorAfterDelayComponents)
            {
                _gameplayComponents.StartCoroutine(activator.DelayedActivate());
            }
        }
    }
}