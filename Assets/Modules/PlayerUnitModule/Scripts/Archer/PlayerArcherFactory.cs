using System.Collections.Generic;
using Modules.EntityModule.Scripts.Damageable;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts;
using Modules.SharedModule.Scripts.Audio;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherFactory
    {
        private readonly PlayerArcherArrowFactory _archerArrowFactory;
        private readonly DamageablesRepository _playerDamageablesRepository;
        private readonly DamageablesRepository _enemyDamageablesRepository;
        private readonly Pool<PlayerArcherComponents> _pool;
        private readonly AudioService _audioService;

        public PlayerArcherFactory(PlayerArcherArrowFactory archerArrowFactory,
            DamageablesRepository playerDamageablesRepository, DamageablesRepository enemyDamageablesRepository,
            AudioService audioService)
        {
            _archerArrowFactory = archerArrowFactory;
            _playerDamageablesRepository = playerDamageablesRepository;
            _enemyDamageablesRepository = enemyDamageablesRepository;
            _audioService = audioService;
            _pool = new Pool<PlayerArcherComponents>(null, null, null);
        }

        public MergeUnitComponent GetPlayerArcherMergeUnitComponent(MergeUnitComponent prefab, Vector3 position)
        {
            return GetPlayerArcher(prefab.GetComponent<PlayerArcherComponents>(), position).MergeUnitComponent;
        }

        public PlayerArcherComponents GetPlayerArcher(PlayerArcherComponents prefab, Vector3 position)
        {
            var instance = _pool.TryGet(prefab);
            instance.transform.position = position;
            InitializePlayerArcherInstance(instance);
            return instance;
        }

        public void InitializePlayerArcherInstance(PlayerArcherComponents components)
        {
            components.DisableObserverComponent.ClearDisabled();
            _playerDamageablesRepository.TryAdd(components.gameObject, components.LogicComponent.DamageableComponent);

            components.DisableObserverComponent.Disabled += TryRelease;
            
            components.LogicComponent.AttackComponent.Construct(_enemyDamageablesRepository,
                components.LogicComponent.AttackComponent.CanSkipAttack, subscribeToDoDamage: false);
            components.ArrowSpawnerComponent.Construct(_archerArrowFactory);
            components.EntitySoundsComponent.Construct(_audioService);
            
            return;

            void TryRelease(GameObject gameObject)
            {
                _pool.TryRelease(components);
            }
        }
    }
}