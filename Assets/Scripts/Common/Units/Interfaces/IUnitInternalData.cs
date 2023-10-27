using System;
using Common.Models.Particles;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitInternalData : IDisposable
    {
        Transform Transform { get; }
        Vector2 FaceDirection { get; }
        Vector2 MoveDirection { get; }
        
        bool InAir { get; }
        bool IsInvincible { get; }
        
        float StaggerTime { get; }
        IUnitStatsData StatsData { get; }
        
        UnitParticlesPlayer ParticlesPlayer { get; }
        UnitPhysics Physics { get; }
        
        IAnimationEventsReceiver AnimationEventsReceiver { get; }
        IAnimationEventsExecutor AnimationEventsExecutor { get; }
        IActionsData ActionsData { get; }
        Type Type { get; }

        UnitAnimator Animator { get; }

        void Flip(Vector2 direction);
        void SetFaceDirection(Vector2 direction);
        void SetMoveDirection(Vector2 direction);
        void SetStaggerTime(float time);
        void SetInvincibility();
        void ResetInvincibility();
    }
}