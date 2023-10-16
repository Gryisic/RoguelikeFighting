﻿using System;
using System.Linq;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Rooms;
using Common.UI;
using Common.UI.Extensions;
using Common.UI.Gameplay;
using Common.UI.Gameplay.Hero;
using Common.UI.Gameplay.Rooms;
using Common.UI.Interfaces;
using Common.Utils.Interfaces;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.States
{
    public class GameplayMenuState : IGameplayState, IDeactivatable
    {
        private readonly IStateChanger<IGameplayState> _stateChanger;
        private readonly IGameplayData _gameplayData;
        private readonly IInputService _input;
        private readonly Player _player;
        private readonly IStageData _stageData;
        private readonly IRunData _runData;
        private readonly HeroView _heroView;
        private readonly ModifierCardsHandler _modifierCardsHandler;
        private readonly TradeCardsHandler _tradeCardsHandler;
        private readonly StorageSpinView _storageSpinView;

        private UIElement _activeUI;

        public GameplayMenuState(IStateChanger<IGameplayState> stateChanger, IGameplayData gameplayData, IServicesHandler servicesHandler, Player player, IStageData stageData, IRunData runData, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _gameplayData = gameplayData;
            _input = servicesHandler.InputService;
            _player = player;
            _stageData = stageData;
            _runData = runData;
            _heroView = ui.Get<HeroView>();
            _modifierCardsHandler = ui.Get<ModifierCardsHandler>();
            _tradeCardsHandler = ui.Get<TradeCardsHandler>();
            _storageSpinView = ui.Get<StorageSpinView>();
        }

        public void Activate()
        {
            SubscribeToEvents();
            ActivateUI();
            
            AttachInput();
        }

        public void Deactivate()
        {
            DeAttachInput();
            UnsubscribeToEvents();
            
            DeactivateUI();
        }
        
        private void ActivateUI()
        {
            _heroView.Deactivate();

            switch (_gameplayData.MenuType)
            {
                case Enums.MenuType.Menu:
                    break;
                
                case Enums.MenuType.ModifierSelection:
                    _activeUI = _modifierCardsHandler;
                    break;

                case Enums.MenuType.Trade:
                    _activeUI = _tradeCardsHandler;
                    break;
                
                case Enums.MenuType.Storage:
                    _activeUI = _storageSpinView;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_activeUI is ISelectableUIElement selectable)
                selectable.Exited += ToActiveState;

            _activeUI.Activate();
        }

        private void DeactivateUI()
        {
            if (_activeUI is ISelectableUIElement selectable)
                selectable.Exited -= ToActiveState;
            
            _activeUI.Deactivate();
            
            _heroView.Activate();
        }
        
        private void SubscribeToEvents()
        {
            _runData.Get<ModifiersData>(Enums.RunDataType.Modifiers).ModifierAdded += _player.AddModifier;
            
            _modifierCardsHandler.CardSelected += OnModifierCardSelected;
            _tradeCardsHandler.CardSelected += OnTradeCardSelected;
            _storageSpinView.SpinEnded += OnStorageSpinEnded;
        }

        private void UnsubscribeToEvents()
        {
            _runData.Get<ModifiersData>(Enums.RunDataType.Modifiers).ModifierAdded -= _player.AddModifier;
            
            _modifierCardsHandler.CardSelected -= OnModifierCardSelected;
            _tradeCardsHandler.CardSelected -= OnTradeCardSelected;
            _storageSpinView.SpinEnded -= OnStorageSpinEnded;
        }
        
        private void AttachInput()
        {
            _input.Input.Menu.Select.performed += _activeUI.Select;
            _input.Input.Menu.Back.performed += _activeUI.Undo;
            _input.Input.Menu.Up.performed += _activeUI.MoveUp;
            _input.Input.Menu.Down.performed += _activeUI.MoveDown;
            _input.Input.Menu.Left.performed += _activeUI.MoveLeft;
            _input.Input.Menu.Right.performed += _activeUI.MoveRight;

            _input.Input.Menu.Enable();
            _input.Input.Debug.Enable();
            
            Debug.Log(_input.Input.Menu.enabled);
        }

        private void DeAttachInput()
        {
            _input.Input.Menu.Disable();
            _input.Input.Debug.Disable();
            
            _input.Input.Menu.Select.performed -= _activeUI.Select;
            _input.Input.Menu.Back.performed -= _activeUI.Undo;
            _input.Input.Menu.Up.performed -= _activeUI.MoveUp;
            _input.Input.Menu.Down.performed -= _activeUI.MoveDown;
            _input.Input.Menu.Left.performed -= _activeUI.MoveLeft;
            _input.Input.Menu.Right.performed -= _activeUI.MoveRight;
        }
        
        private void OnModifierCardSelected(int index)
        {
            _runData.Get<ModifiersData>(Enums.RunDataType.Modifiers).GetModifierFromBuffer(index);

            ToActiveState();
        }

        private void OnTradeCardSelected(int index)
        {
            TradeRoom tradeRoom = _stageData.Rooms.First(r => r is TradeRoom) as TradeRoom;
            tradeRoom.ItemSelected(index);
        }
        
        private void OnStorageSpinEnded()
        {
            ToActiveState();
        }
        
        private void ToActiveState()
        {
            _stateChanger.ChangeState<GameplayActiveState>();
        }
    }
}