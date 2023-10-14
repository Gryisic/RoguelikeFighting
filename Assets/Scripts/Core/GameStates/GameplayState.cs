using System;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.States;
using Common.UI;
using Common.UI.Gameplay;
using Common.UI.Gameplay.Hero;
using Common.Utils;
using Common.Utils.Extensions;
using Core.Interfaces;
using Infrastructure.Factories.GameplayStateFactory.Interfaces;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class GameplayState : StatesChanger<IGameplayState>, IGameState, IDeactivatable, IResettable, IDisposable
    {
        private readonly IGameplayStateFactory _gameplayStateFactory;
        private readonly IRunData _runData;
        private readonly HeroView _heroView;
        
        private bool _isInitialized;

        public GameplayState(IGameplayStateFactory gameplayStateFactory, IRunData runData, UI ui)
        {
            _gameplayStateFactory = gameplayStateFactory;
            _runData = runData;
            _heroView = ui.Get<HeroView>();
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
        
        private void SubscribeToEvents()
        {
            ExperienceData experienceData = _runData.Get<ExperienceData>(Enums.RunDataType.Experience);
            HealData healData = _runData.Get<HealData>(Enums.RunDataType.Heal);

            healData.ChargesUpdated += _heroView.UpdateHealCharges;
        }

        private void UnsubscribeToEvents()
        {
            ExperienceData experienceData = _runData.Get<ExperienceData>(Enums.RunDataType.Experience);
            HealData healData = _runData.Get<HealData>(Enums.RunDataType.Heal);

            healData.ChargesUpdated -= _heroView.UpdateHealCharges;
        }
    }
}