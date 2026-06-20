using System;
using Modules.CoreModule.Scripts.GameStates;
using Modules.EntityModule.Scripts.Damageable;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameplayComponents _gameplayComponents;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            var playerDamageablesRepository = new DamageablesRepository();
            var enemyDamageablesRepository = new DamageablesRepository();

            var gamePlayState = new GameplayGameState(playerDamageablesRepository, 
                enemyDamageablesRepository,
                _gameplayComponents);

            new BootstrapGameState(gamePlayState).Enter();
        }
    }
}