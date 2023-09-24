using System;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Units
{
    public abstract class UnitInternalData : IUnitInternalData
    {
        public Transform Transform { get; }
        public Vector2 FaceDirection { get; private set; } = Vector2.right;
        public Vector2 MoveDirection { get; private set; }

        public bool InAir => Physics.IsGrounded == false;

        public float StaggerTime { get; private set; }
        public IUnitStatsData StatsData { get; }
        public UnitPhysics Physics { get; }

        public IAnimationEventsReceiver AnimationEventsReceiver { get; }
        public IActionsData ActionsData { get; }
        public Type Type { get; }
        
        public UnitAnimator Animator { get; }

        protected UnitInternalData(Transform transform, UnitPhysics physics, UnitAnimator animator, IUnitStatsData statsData, IActionsData actionsData, IAnimationEventsReceiver animationEventsReceiver, Type type)
        {
            Transform = transform;
            Physics = physics;
            Animator = animator;
            ActionsData = actionsData;
            AnimationEventsReceiver = animationEventsReceiver;
            Type = type;
            StatsData = statsData;
        }
        
        public abstract void Dispose();
        
        public void SetFaceDirection(Vector2 direction) => FaceDirection = direction;
        
        public void SetMoveDirection(Vector2 direction) => MoveDirection = direction;

        public void SetStaggerTime(float time)
        {
            if (time < 0)
                throw new InvalidOperationException($"Trying to set stagger time to negative value. Value: {time}");

            StaggerTime = time;
        }
    }
}