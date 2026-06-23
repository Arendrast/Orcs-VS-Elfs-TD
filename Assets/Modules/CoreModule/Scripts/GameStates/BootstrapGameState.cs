using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class BootstrapGameState
    {
        private readonly GameplayGameState _gameplayGameState;
        private readonly BootstrapStateComponents _bootstrapStateComponents;

        public BootstrapGameState(GameplayGameState gameplayGameState, BootstrapStateComponents bootstrapStateComponents)
        {
            _gameplayGameState = gameplayGameState;
            _bootstrapStateComponents = bootstrapStateComponents;
        }

        public void Enter()
        {
            Application.targetFrameRate = _bootstrapStateComponents.TargetFrameRate;
            
            _gameplayGameState.Enter();
        }
    }
}