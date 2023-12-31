﻿using System;

namespace Infrastructure.Utils
{
    public static class Enums
    {
        public enum GameStateType
        {
            Initialize,
            SceneSwitch,
            MainMenu,
            Gameplay
        }
        
        public enum SceneType
        {
            MainMenu,
            Arena
        }

        public enum CameraDistanceType
        {
            Neutral,
            Far,
            Close
        }

        public enum CameraEasingType
        {
            Instant,
            Smooth
        }

        public enum UILayer
        {
            Overlay,
            Camera,
            World
        }
        
        public enum MenuType
        {
            Menu,
            ModifierSelection,
            Trade,
            Storage
        }
        
        public enum WaveRequirement
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

        public enum LegacyUnitNavigationStrategy
        {
            Linear
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
            FirstLegacySkill,
            SecondLegacySkill,
            Dash,
            Heal,
            TakeDamage
        }
        
        public enum InputDirection
        {
            Horizontal,
            Up,
            Down
        }

        public enum ActionExecutionPlacement
        {
            Ground,
            Air
        }

        public enum ActionEffect
        {
            MeleeAttack,
            Projectile,
            ChangeStance,
            Teleportation,
            Heal
        }
        
        public enum Knockback
        {
            None,
            Fixed,
            Free
        }
        
        public enum GenericParticle
        {
            Dash,
            Landing,
            TakeHit,
            AttackMarker,
            MidAirJump
        }

        public enum Stat
        {
            MaxHealth,
            Health,
            AttackMultiplier,
            DefenceMultiplier
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

        public enum StatusEffect
        {
            None,
            Freeze,
            Burn
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

        public enum RunDataType
        {
            Gald,
            Heal,
            Experience,
            Modifiers,
            None
        }
        
        public enum RunDataChange
        {
            Increase,
            Decrease
        }

        [Flags]
        public enum RoomType
        {
            Selection,
            Battle,
            Trade,
            Storage,
            ExtensiveBattle
        }
    }
}