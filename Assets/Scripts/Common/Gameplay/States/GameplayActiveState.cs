using System;
using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers.Templates;
using Common.Scene.Cameras.Interfaces;
using Common.UI.Gameplay;
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
        private readonly GameplayUI _ui;
        
        private ICameraService _cameraService;
        
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
            _ui = ui.Gameplay;
        }

        public void Dispose()
        {
            
        }
        
        public void Activate()
        {
            _cameraService ??= _servicesHandler.GetSubService<ICameraService>();
                
            AttachInput();
            SubscribeToEvents();
            
            _cameraService.FollowUnit(_player.HeroTransform);
        }
        
        public void Deactivate()
        {
            DeAttachInput();
            UnsubscribeToEvents();
        }

        private void SetDebugMultiplier(InputAction.CallbackContext obj)
        {
            _runData.ModifiersData.AddExperience(100);
        }
        
        private void DamageHero(InputAction.CallbackContext obj)
        {
            _unitsHandler.Heroes[0].TakeDamage(10);
        }
        
        private void SubscribeToEvents()
        {
            _unitsHandler.Heroes[0].HealthUpdated += _ui.HeroView.UpdateHealth;

            _stageData.HeroPositionChangeRequested += _player.SetPosition;
            
            _runData.ModifiersData.ModifiersSelectionRequested += OnModifierSelectionRequested;
        }

        private void UnsubscribeToEvents()
        {
            _unitsHandler.Heroes[0].HealthUpdated -= _ui.HeroView.UpdateHealth;
            
            _stageData.HeroPositionChangeRequested -= _player.SetPosition;
            
            _runData.ModifiersData.ModifiersSelectionRequested -= OnModifierSelectionRequested;
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
            
            _input.Input.Debug.SetDebugModifier.performed += SetDebugMultiplier;
            _input.Input.Debug.DamageHero.performed += DamageHero;
            
            _input.Input.Gameplay.Enable();
            _input.Input.Debug.Enable();
        }

        private void DeAttachInput()
        {
            _input.Input.Gameplay.Disable();
            _input.Input.Debug.Disable();
            
            _input.Input.Debug.SetDebugModifier.performed -= SetDebugMultiplier;
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
        }
        
        private void OnModifierSelectionRequested(IReadOnlyList<ModifierTemplate> templates)
        {
            _ui.CardsHandler.SetCardsData(templates);
            _gameplayData.SetPauseType(Enums.PauseType.ModifierSelection);
            
            _stateChanger.ChangeState<GameplayPauseState>();
        }
    }
}