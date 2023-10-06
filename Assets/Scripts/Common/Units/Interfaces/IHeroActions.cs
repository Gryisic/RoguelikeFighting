using Common.Gameplay.Modifiers;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IHeroActions
    {
        void Attack();
        void Skill();
        void Interact();
        void Jump();
        void Dash();
        void LegacySkill(Enums.HeroActionType actionType);
        void Heal();
        void StartMoving(Vector2 direction);
        void StopMoving();
        void AddModifier(Modifier modifier);
    }
}