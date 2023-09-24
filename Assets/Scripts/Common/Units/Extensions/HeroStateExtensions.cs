using Common.Units.Interfaces;

namespace Common.Units.Extensions
{
    public static class HeroStateExtensions
    {
        public static void Attack(this IHeroState state)
        {
            if (state is IAttackExecutor attackExecutor)
                attackExecutor.Attack();
        }
        
        public static void Skill(this IHeroState state)
        {
            if (state is ISkillExecutor skillExecutor)
                skillExecutor.Skill();
        }
        
        public static void LegacySkill(this IHeroState state)
        {
            if (state is ILegacySkillExecutor legacySkillExecutor)
                legacySkillExecutor.LegacySkill();
        }
        
        public static void Dash(this IHeroState state)
        {
            if (state is IDashExecutor dashExecutor)
                dashExecutor.Dash();
        }
        
        public static void Jump(this IHeroState state)
        {
            if (state is IJumpExecutor jumpExecutor)
                jumpExecutor.Jump();
        }
        
        public static void Interact(this IHeroState state)
        {
            if (state is IInteractExecutor interactExecutor)
                interactExecutor.Interact();
        }
    }
}