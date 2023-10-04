﻿using System;

namespace Infrastructure.Utils
{
    public static class Enums
    {
        public enum GameStateType
        {
            Initialize,
            SceneSwitch,
            Gameplay
        }
        
        public enum SceneType
        {
            Arena
        }

        public enum CameraDistanceType
        {
            Neutral,
            Far,
            Close
        }
        
        public enum PauseType
        {
            Menu,
            ModifierSelection
        }
        
        public enum NextWaveRequirement
        {
            EnemiesDefeated,
            Timer
        }
        
        public enum Hero
        {
            Kirito,
            Asuna
        }
        
        public enum Enemy
        {
            Rinko,
            Ninja
        }
        
        public enum ActionExecutionAwait
        {
            Everywhere,
            OnLineOrHigher,
            Near
        }

        public enum HeroActionType
        {
            None,
            BasicAttack,
            Skill,
            LegacySkill,
            Dash,
            TakeDamage
        }
        
        public enum InputDirection
        {
            Horizontal,
            Up,
            Down
        }

        public enum HeroActionExecutionPlacement
        {
            Ground,
            Air
        }

        public enum ActionEffect
        {
            MeleeAttack,
            Projectile,
            ChangeStance
        }
        
        public enum Knockback
        {
            None,
            Fixed,
            Free
        }

        public enum Stat
        {
            MaxHealth,
            Health
        }

        public enum LegacySkillType
        {
            First,
            Second
        }

        public enum Modifier
        {
            RingOfFire,
            Freeze
        }
        
        public enum ModifierExecutionCondition
        {
            HeroAction,
            Time
        }
        
        public enum ModifierRarity
        {
            Common,
            Rare,
            Epic,
            Legendary
        }

        public enum DefaultAnimation
        {
            Idle,
            Walk,
            Run,
            TakeHitLight,
            TakeHitMedium,
            TakeHitHeavy,
            Death
        }

        public enum EventTriggerType
        {
            Dialogue,
            Battle
        }
        
        public enum EventTriggerActivationType
        {
            Automatic,
            Manual
        }
        
        [Flags]
        public enum RoomType
        {
            Selection,
            Battle,
        }
    }
}