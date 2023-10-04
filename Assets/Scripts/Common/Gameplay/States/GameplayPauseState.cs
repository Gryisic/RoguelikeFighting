using System;
using Common.Gameplay.Interfaces;
using Common.UI;
using Common.UI.Gameplay;
using Common.Utils.Interfaces;
using Core.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.States
{
    public class GameplayPauseState : IGameplayState, IDeactivatable
    {
        private readonly IStateChanger<IGameplayState> _stateChanger;
        private readonly IGameplayData _gameplayData;
        private readonly Player _player;
        private readonly IRunData _runData;
        private readonly GameplayUI _ui;

        private UIElement _activeUI;

        public GameplayPauseState(IStateChanger<IGameplayState> stateChanger, IGameplayData gameplayData, Player player, IRunData runData, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _gameplayData = gameplayData;
            _player = player;
            _runData = runData;
            _ui = ui.Gameplay;
        }

        public void Activate()
        {
            SubscribeToEvents();
            
            ActivateUI();
        }

        public void Deactivate()
        {
            UnsubscribeToEvents();
            
            DeactivateUI();
        }
        
        private void ActivateUI()
        {
            _ui.HeroView.Deactivate();

            switch (_gameplayData.PauseType)
            {
                case Enums.PauseType.Menu:
                    break;
                
                case Enums.PauseType.ModifierSelection:
                    _activeUI = _ui.CardsHandler;
                    _ui.CardsHandler.Activate();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DeactivateUI()
        {
            _activeUI.Deactivate();
            
            _ui.HeroView.Activate();
        }
        
        private void SubscribeToEvents()
        {
            _runData.ModifiersData.ModifierAdded += _player.AddModifier;
            
            _ui.CardsHandler.CardSelected += OnModifierCardSelected;
        }

        private void UnsubscribeToEvents()
        {
            _runData.ModifiersData.ModifierAdded -= _player.AddModifier;
            
            _ui.CardsHandler.CardSelected -= OnModifierCardSelected;
        }
        
        private void OnModifierCardSelected(int index)
        {
            _runData.ModifiersData.GetModifierFromBuffer(index);
            
            _stateChanger.ChangeState<GameplayActiveState>();
        }
    }
}