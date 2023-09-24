using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.States;
using Common.Utils;
using Common.Utils.Extensions;
using Core.Interfaces;
using Infrastructure.Factories.GameplayStateFactory.Interfaces;

namespace Core.GameStates
{
    public class GameplayState : StatesChanger<IGameplayState>, IGameState, IDeactivatable, IResettable, IDisposable
    {
        private readonly IGameplayStateFactory _gameplayStateFactory;
        
        private bool _isInitialized;

        public GameplayState(IGameplayStateFactory gameplayStateFactory)
        {
            _gameplayStateFactory = gameplayStateFactory;
        }

        public void Dispose()
        {
            
        }
        
        public void Activate(GameStateArgs args)
        {
            if (_isInitialized == false)
            {
                CreateStates();
                ChangeState<GameplayInitializeState>();

                _isInitialized = true;
                
                return;
            }
            
            ChangeState<GameplayActiveState>();
        }

        public void Deactivate()
        {
            currentState.Deactivate();
        }
        
        public void Reset()
        {
            _isInitialized = false;
        }

        private void CreateStates()
        {
            _gameplayStateFactory.CreateAllStates();

            states = _gameplayStateFactory.States;
        }
    }
}