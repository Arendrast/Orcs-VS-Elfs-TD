using Modules.EntityModule.Scripts.Damageable;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherFactory
    {
        private readonly PlayerArcherArrowFactory _archerArrowFactory;
        private readonly DamageablesRepository _playerDamageablesRepository;
        private readonly DamageablesRepository _enemyDamageablesRepository;

        public PlayerArcherFactory(PlayerArcherArrowFactory archerArrowFactory,
            DamageablesRepository playerDamageablesRepository, DamageablesRepository enemyDamageablesRepository)
        {
            _archerArrowFactory = archerArrowFactory;
            _playerDamageablesRepository = playerDamageablesRepository;
            _enemyDamageablesRepository = enemyDamageablesRepository;
        }

        public PlayerArcherComponents GetCreatedPlayerArcher(PlayerArcherComponents prefab, Vector3 position)
        {
            var instance = Object.Instantiate(prefab, position, Quaternion.identity);
            InitializePlayerArcherInstance(instance);
            return instance;
        }

        public void InitializePlayerArcherInstance(PlayerArcherComponents components)
        {
            _playerDamageablesRepository.TryAdd(components.gameObject, components.LogicComponent.DamageableComponent);
            components.LogicComponent.AttackComponent.Construct(_enemyDamageablesRepository, subscribeToDoDamage: false);
            components.ArrowSpawnerComponent.Construct(_archerArrowFactory);
        }
    }
}