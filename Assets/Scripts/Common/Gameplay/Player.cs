using System;
using Common.Gameplay.Modifiers;
using Common.Units;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Gameplay
{
    public class Player
    {
        private IHeroActions _hero;
        
        public Transform HeroTransform { get; private set; }

        public void UpdateHero(Hero hero)
        {
            if (hero == null)
                throw new ArgumentNullException(nameof(hero));

            _hero = hero;
            HeroTransform = hero.Transform;
            
            hero.Invoke(nameof(hero.Activate), 1f);
        }

        public void SetPosition(Vector2 position) => HeroTransform.position = position;

        public void StartMoving(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            
            _hero.StartMoving(direction);
        }
        
        public void StopMoving(InputAction.CallbackContext context) => _hero.StopMoving();

        public void Attack(InputAction.CallbackContext context) => _hero.Attack();

        public void Skill(InputAction.CallbackContext context) => _hero.Skill();

        public void Jump(InputAction.CallbackContext context) => _hero.Jump();

        public void Dash(InputAction.CallbackContext context) => _hero.Dash();

        public void Interact(InputAction.CallbackContext context) => _hero.Interact();

        public void FirstLegacySkill(InputAction.CallbackContext context) => _hero.LegacySkill(Enums.LegacySkillType.First);

        public void SecondLegacySkill(InputAction.CallbackContext context) => _hero.LegacySkill(Enums.LegacySkillType.Second);

        public void AddModifier(Modifier modifier) => _hero.AddModifier(modifier);
    }
}