using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers;
using Common.Scene.Cameras.Interfaces;
using Common.UI.Gameplay;
using Common.Units;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine.InputSystem;

namespace Common.Gameplay.States
{
    public class GameplayActiveState : IGameplayState, IDeactivatable, IDisposable
    {
        private readonly IInputService _input;
        private readonly ICameraService _cameraService;
        private readonly UnitsHandler _unitsHandler;
        private readonly Player _player;
        private readonly ModifiersResolver _modifiersResolver;
        private readonly UI.UI _ui;
        
        public GameplayActiveState(Player player, IInputService input, ICameraService cameraService, UnitsHandler unitsHandler, ModifiersResolver modifiersResolver, UI.UI ui)
        {
            _player = player;
            _input = input;
            _cameraService = cameraService;
            _unitsHandler = unitsHandler;
            _modifiersResolver = modifiersResolver;
            _ui = ui;
        }

        public void Dispose()
        {
            
        }
        
        public void Activate()
        {
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
            _player.AddModifier(_modifiersResolver.GetModifier(Enums.Modifier.Freeze));
        }
        
        private void DamageHero(InputAction.CallbackContext obj)
        {
            _unitsHandler.Heroes[0].TakeDamage(10);
        }
        
        private void SubscribeToEvents()
        {
            _unitsHandler.Heroes[0].HealthUpdated += _ui.GetElementAndCastToType<GameplayUI>().HealthBarsView.UpdateHeroHealth;
        }
        
        private void UnsubscribeToEvents()
        {
            _unitsHandler.Heroes[0].HealthUpdated -= _ui.GetElementAndCastToType<GameplayUI>().HealthBarsView.UpdateHeroHealth;
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
    }
}