using System;
using Common.Models.Particles;
using Common.Units.Interfaces;
using Core.Extensions;
using UnityEngine;

namespace Common.Units
{
    public abstract class UnitInternalData : IUnitInternalData
    {
        public Transform Transform { get; }
        public Vector2 FaceDirection { get; private set; } = Vector2.right;
        public Vector2 MoveDirection { get; private set; }

        public bool InAir => Physics.IsGrounded == false;
        public bool IsInvincible { get; protected set; }

        public float StaggerTime { get; private set; }
        public IUnitStatsData StatsData { get; }
        
        public UnitParticlesPlayer ParticlesPlayer { get; }
        public UnitPhysics Physics { get; }

        public IAnimationEventsReceiver AnimationEventsReceiver { get; }
        public IAnimationEventsExecutor AnimationEventsExecutor { get; }
        public IActionsData ActionsData { get; }
        public Type Type { get; }
        
        public UnitAnimator Animator { get; }

        protected UnitInternalData(Transform transform, UnitPhysics physics, IUnitRendererData rendererData, IUnitStatsData statsData, IActionsData actionsData, Type type)
        {
            Transform = transform;
            ParticlesPlayer = rendererData.ParticlesPlayer;
            Physics = physics;
            Animator = rendererData.Animator;
            ActionsData = actionsData;
            AnimationEventsReceiver = rendererData.AnimationEventsReceiver;
            Type = type;
            StatsData = statsData;

            AnimationEventsExecutor = new AnimationEventsExecutor(this);
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

        public void SetInvincibility() => IsInvincible = true;

        public void ResetInvincibility() => IsInvincible = false;

        public void Flip(Vector2 direction)
        {
            float directionX = direction.x == 0 ? FaceDirection.x : direction.x.ToNearestNormal();
            float directionY = Mathf.Ceil(direction.y);
            
            Vector2 faceDirection = new Vector2(directionX, directionY);
            SetFaceDirection(faceDirection);
            
            float rotation = directionX > 0 ? 0 : 180;
            Quaternion quaternion = new Quaternion(0, rotation, 0, 0);

            Transform.rotation = quaternion;
        }
    }
}