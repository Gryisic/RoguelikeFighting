using System;
using System.Collections.Generic;
using System.Threading;
using Common.Models.Actions;
using Common.Models.Actions.Templates;
using Common.Models.Particles;
using Common.Models.Particles.Interfaces;
using Common.Units.Interfaces;
using Common.Units.Legacy;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units
{
    [RequireComponent(typeof(AnimationEventsReceiver), typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class LegacyUnit : Unit, ILegacyUnit
    {
        private CancellationTokenSource _moveTokenSource;

        public Vector2 FaceDirection { get; private set; } = Vector2.right;
        public Vector2 MoveDirection { get; private set; }
        public bool IsInvincible { get; } = true;
        public bool InAir { get; }
        public float StaggerTime { get; }
        public UnitParticlesPlayer ParticlesPlayer { get; private set; }
        public UnitPhysics Physics { get; private set; } 
        public IAnimationEventsReceiver AnimationEventsReceiver => animationEventsReceiver;
        public IAnimationEventsExecutor AnimationEventsExecutor { get; private set; }
        public IActionsData ActionsData => actionsData;
        public Type Type => GetType();
        public UnitAnimator Animator => animator;

        public LegacyActionTemplate ActionData { get; private set; }

        public override void Initialize(UnitTemplate template)
        {
            if (template is LegacyUnitTemplate legacyUnitTemplate)
                ActionData = legacyUnitTemplate.LegacyActionTemplate;
            else
                throw new InvalidOperationException("Trying to initialize LegacyUnit from non LegacyUnit Template");

            ParticlesPlayer = new UnitParticlesPlayer();

            List<IParticleData> particlesData = new List<IParticleData>
            {
                new ParticleData(legacyUnitTemplate.LegacyActionTemplate.ParticleForCopy, legacyUnitTemplate.LegacyActionTemplate.ParticleID)
            };

            internalData = this;
            
            ParticlesPlayer.Initialize(Transform, particlesData, genericParticlesData);
        }

        public void Activate(Vector2 position)
        {
            if (AnimationEventsExecutor == null)
                AnimationEventsExecutor = new AnimationEventsExecutor(this);
            
            if (Physics == null)
                Physics = new UnitPhysics(rigidbody, collider);

            Transform.position = position;
            gameObject.SetActive(true);
            physics.StartUpdating();
        }

        public void Deactivate()
        {
            physics.StopUpdating();
            gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            ActionsData.Dispose();
            
            _moveTokenSource?.Cancel();
            _moveTokenSource?.Dispose();
        }

        public void SetFaceDirection(Vector2 direction) => FaceDirection = direction;

        public void SetMoveDirection(Vector2 direction) => MoveDirection = direction;

        public void SetStaggerTime(float time) { }
        
        public void SetInvincibility() { }

        public void ResetInvincibility() { }

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

        public void MoveTo(Vector2 position, float speed)
        {
            _moveTokenSource = new CancellationTokenSource();
            
            MoveAsync(position, speed).Forget();
        }

        private async UniTask MoveAsync(Vector2 position, float speed)
        {
            float delta = position.x - Transform.position.x;
            float delay = Mathf.Abs(delta) / speed;
            Vector2 direction = new Vector2(delta.ToClamped(-1, 1), 0);
            
            SetFaceDirection(direction);
            Flip(direction);
            
            Physics.UpdateHorizontalVelocity(speed * direction.x);

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _moveTokenSource.Token);
            
            Physics.UnSuppressManualVelocityChange();
            Physics.UpdateHorizontalVelocity(0);
        }
    }
}