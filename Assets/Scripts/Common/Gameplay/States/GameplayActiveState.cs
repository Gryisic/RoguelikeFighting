using System;
using System.Collections.Generic;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers.Templates;
using Common.Gameplay.Rooms;
using Common.Models.Items;
using Common.Scene.Cameras.Interfaces;
using Common.UI.Gameplay;
using Common.UI.Gameplay.Hero;
using Common.UI.Gameplay.Rooms;
using Common.UI.Gameplay.RunData;
using Common.Units;
using Common.Utils.Interfaces;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine.InputSystem;

namespace Common.Gameplay.States
{
    public class GameplayActiveState : IGameplayState, IDeactivatable, IDisposable
    {
        private readonly IStateChanger<IGameplayState> _stateChanger;
        private readonly IGameplayData _gameplayData;
        private readonly IStageData _stageData;
        private readonly Player _player;
        private readonly IRunData _runData;
        private readonly IInputService _input;
        private readonly IServicesHandler _servicesHandler;
        private readonly UnitsHandler _unitsHandler;
        private readonly HeroView _heroView;
        private readonly RunDatasView _dataView;
        private readonly DamageView _damageView;
        private readonly ModifierCardsHandler _modifierCardsHandler;
        private readonly TradeCardsHandler _tradeCardsHandler;
        private readonly StorageSpinView _storageSpinView;
        
        public GameplayActiveState(IStateChanger<IGameplayState> stateChanger, IGameplayData gameplayData, IStageData stageData, Player player, IRunData runData, IServicesHandler servicesHandler, UnitsHandler unitsHandler, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _player = player;
            _gameplayData = gameplayData;
            _stageData = stageData;
            _runData = runData;
            _input = servicesHandler.InputService;
            _servicesHandler = servicesHandler;
            _unitsHandler = unitsHandler;
            _heroView = ui.Get<HeroView>();
            _dataView = ui.Get<RunDatasView>();
            _damageView = ui.Get<DamageView>();
            _modifierCardsHandler = ui.Get<ModifierCardsHandler>();
            _tradeCardsHandler = ui.Get<TradeCardsHandler>();
            _storageSpinView = ui.Get<StorageSpinView>();
        }

        public void Dispose()
        {
            
        }
        
        public void Activate()
        {
            AttachInput();
            SubscribeToEvents();
            
            _heroView.Activate();
            _dataView.Activate();
        }
        
        public void Deactivate()
        {
            DeAttachInput();
            UnsubscribeToEvents();
        }

        private void SetDebugExperience(InputAction.CallbackContext obj)
        {
            _runData.Get<ExperienceData>(Enums.RunDataType.Experience).Add(100);
        }
        
        private void DamageHero(InputAction.CallbackContext obj)
        {
            _unitsHandler.Hero.TakeDamage(1);
            _servicesHandler.GetSubService<ICameraService>().Shake();
        }
        
        private void SubscribeToEvents()
        {
            _unitsHandler.Hero.HealthUpdated += _heroView.UpdateHealth;

            _stageData.HeroPositionChangeRequested += _player.SetPosition;

            ModifiersData modifiersData = _runData.Get<ModifiersData>(Enums.RunDataType.Modifiers);
            ExperienceData experienceData = _runData.Get<ExperienceData>(Enums.RunDataType.Experience);

            modifiersData.ModifiersSelectionRequested += OnModifierSelectionRequested;
            experienceData.AmountOverflowed += modifiersData.OnExperienceOverflowed;
            
            foreach (var room in _stageData.Rooms)
            {
                if (room is TradeRoom tradeRoom)
                {
                    tradeRoom.TradeRequested += OnTradeRequested;
                    tradeRoom.ItemPurchased += _tradeCardsHandler.ConfirmSelection;
                    tradeRoom.ItemNotPurchased += _tradeCardsHandler.UndoSelection;
                }
                
                if (room is StorageRoom storageRoom)
                    storageRoom.StorageSpinRequested += OnSpinRequested;

                if (room is BattleRoom battleRoom)
                    battleRoom.ExperienceObtained += experienceData.Add;
            }
            
            foreach (var unit in _unitsHandler.Units)
            {
                unit.DamageTaken += _damageView.DisplayAt;
            }
        }

        private void UnsubscribeToEvents()
        {
            _unitsHandler.Hero.HealthUpdated -= _heroView.UpdateHealth;

            _stageData.HeroPositionChangeRequested -= _player.SetPosition;
            
            ModifiersData modifiersData = _runData.Get<ModifiersData>(Enums.RunDataType.Modifiers);
            ExperienceData experienceData = _runData.Get<ExperienceData>(Enums.RunDataType.Experience);

            modifiersData.ModifiersSelectionRequested -= OnModifierSelectionRequested;
            experienceData.AmountOverflowed -= modifiersData.OnExperienceOverflowed;
            
            foreach (var room in _stageData.Rooms)
            {
                if (room is TradeRoom tradeRoom)
                {
                    tradeRoom.TradeRequested -= OnTradeRequested;
                    tradeRoom.ItemPurchased -= _tradeCardsHandler.ConfirmSelection;
                    tradeRoom.ItemNotPurchased -= _tradeCardsHandler.UndoSelection;
                }
                
                if (room is StorageRoom storageRoom)
                    storageRoom.StorageSpinRequested -= OnSpinRequested;
                
                if (room is BattleRoom battleRoom)
                    battleRoom.ExperienceObtained -= experienceData.Add;
            }
            
            foreach (var unit in _unitsHandler.Units)
            {
                unit.DamageTaken -= _damageView.DisplayAt;
            }
        }

        private void AttachInput()
        {
            _input.Input.Gameplay.Move.performed += _player.StartMoving;
            _input.Input.Gameplay.Move.canceled += _player.StopMoving;
            _input.Input.Gameplay.Attack.performed += _player.Attack;
            _input.Input.Gameplay.Skill.performed += _player.Skill;
            _input.Input.Gameplay.Interact.performed += _player.Interact;
            _input.Input.Gameplay.Jump.performed += _player.Jump;
            _input.Input.Gameplay.Dash.performed += _player.Dash;
            _input.Input.Gameplay.FirstLegacySkill.performed += _player.FirstLegacySkill;
            _input.Input.Gameplay.SecondLegacySkill.performed += _player.SecondLegacySkill;
            _input.Input.Gameplay.Heal.performed += _player.Heal;
            
            _input.Input.Debug.SetDebugModifier.performed += SetDebugExperience;
            _input.Input.Debug.DamageHero.performed += DamageHero;
            
            _input.Input.Gameplay.Enable();
            _input.Input.Debug.Enable();
        }

        private void DeAttachInput()
        {
            _input.Input.Gameplay.Disable();
            _input.Input.Debug.Disable();
            
            _input.Input.Debug.SetDebugModifier.performed -= SetDebugExperience;
            _input.Input.Debug.DamageHero.performed -= DamageHero;
            
            _input.Input.Gameplay.Move.performed -= _player.StartMoving;
            _input.Input.Gameplay.Move.canceled -= _player.StopMoving;
            _input.Input.Gameplay.Attack.performed -= _player.Attack;
            _input.Input.Gameplay.Skill.performed -= _player.Skill;
            _input.Input.Gameplay.Interact.performed -= _player.Interact;
            _input.Input.Gameplay.Jump.performed -= _player.Jump;
            _input.Input.Gameplay.Dash.performed -= _player.Dash;
            _input.Input.Gameplay.FirstLegacySkill.performed -= _player.FirstLegacySkill;
            _input.Input.Gameplay.SecondLegacySkill.performed -= _player.SecondLegacySkill;
            _input.Input.Gameplay.Heal.performed -= _player.Heal;
        }
        
        private void OnTradeRequested(IReadOnlyList<TradeItemData> templates)
        {
            GaldData galdData = _runData.Get<GaldData>(Enums.RunDataType.Gald);
            
            _tradeCardsHandler.SetCardsData(templates, galdData.Amount);
            _gameplayData.SetPauseType(Enums.MenuType.Trade);

            ToMenuState();
        }
        
        private void OnModifierSelectionRequested(IReadOnlyList<ModifierTemplate> templates)
        {
            _modifierCardsHandler.SetCardsData(templates);
            _gameplayData.SetPauseType(Enums.MenuType.ModifierSelection);
            
            ToMenuState();
        }
        
        private void OnSpinRequested(IReadOnlyList<StorageItemData> data, StorageItemData selectedItem)
        {
            _storageSpinView.SetData(data, selectedItem);
            _gameplayData.SetPauseType(Enums.MenuType.Storage);
            
            ToMenuState();
        }

        private void ToMenuState()
        {
            _stateChanger.ChangeState<GameplayMenuState>();
        }
    }
}