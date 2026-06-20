using UnityEngine;

namespace Modules.CoreModule.Scripts.GameStates
{
    public class BootstrapGameState
    {
        private readonly GameplayGameState _gameplayGameState;

        public BootstrapGameState(GameplayGameState gameplayGameState)
        {
            _gameplayGameState = gameplayGameState;
        }

        public void Enter()
        {
            Application.targetFrameRate = 60;
            
            _gameplayGameState.Enter();
        }
    }
}