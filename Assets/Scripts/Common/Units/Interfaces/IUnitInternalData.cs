using System;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitInternalData : IDisposable
    {
        Transform Transform { get; }
        Vector2 FaceDirection { get; }
        Vector2 MoveDirection { get; }
        
        bool InAir { get; }
        
        float StaggerTime { get; }
        IUnitStatsData StatsData { get; }
        
        UnitPhysics Physics { get; }
        
        IAnimationEventsReceiver AnimationEventsReceiver { get; }
        IActionsData ActionsData { get; }
        Type Type { get; }

        UnitAnimator Animator { get; }

        void SetFaceDirection(Vector2 direction);
        void SetMoveDirection(Vector2 direction);
        void SetStaggerTime(float time);
    }
}