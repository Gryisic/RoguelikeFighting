using System;
using System.Threading;
using Common.Models.Actions;
using Common.Models.Actions.Templates;
using Common.Models.Particles;
using Common.Models.Particles.Interfaces;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units
{
    public class AnimationEventsExecutor : IAnimationEventsExecutor
    {
        private readonly IUnitInternalData _internalData;
        private UnitAction _action;
        private ActionTemplate _template;
        private CancellationToken _token;

        public AnimationEventsExecutor(IUnitInternalData internalData)
        {
            _internalData = internalData;
        }

        public void SetData(UnitAction action, CancellationToken token)
        {
            _action = action;
            _template = action.Data;
            _token = token;
        }

        public void OnActionExecutionRequested() => _action.Execute();

        public void OnMovingRequested() => MoveAsync().Forget();
        
        public void OnParticlesEmitRequested()
        {
            IParticleData particleData = new ParticleData(_template.ParticleForCopy, _template.ParticleID);
            
            _internalData.ParticlesPlayer.Play(particleData, _internalData.FaceDirection.x);
        }

        private async UniTask MoveAsync()
        {
            float freezeTime = _template.FreezeAfterMoving;
            float movingTime = _template.UseClipLengthAsTime ? _template.ActionClip.length : _template.MovingTime;
            float speed = _template.Distance / movingTime;
            float horizontalSpeed = speed * _internalData.FaceDirection.x;
            Vector2 velocityVector = new Vector2(_template.Vector.x * horizontalSpeed, _template.Vector.y * speed);
            
            _internalData.Physics.UpdateVelocity(velocityVector);
            _internalData.Physics.SuppressManualVelocityChange();

            await UniTask.Delay(TimeSpan.FromSeconds(movingTime), cancellationToken: _token);
            
            _internalData.Physics.FreezeFalling();
            
            await UniTask.Delay(TimeSpan.FromSeconds(freezeTime), cancellationToken: _token);
            
            _internalData.Physics.UnfreezeFalling();
            _internalData.Physics.UnSuppressManualVelocityChange();
            _internalData.Physics.UpdateVelocity(Vector2.zero);
        }
    }
}