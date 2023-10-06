using System;
using System.Threading;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Actions
{
    public class EnemyAction : UnitAction
    {
        private readonly IEnemyInternalData _internalData;

        private CancellationToken _token;
        
        public new ActionTemplate Data { get; }

        public EnemyAction(ActionTemplate template, IEnemyInternalData internalData) : base(template)
        {
            Data = template;
            _internalData = internalData;

            executionBase = DefineExecutionBase(template.BaseEffect, internalData);
        }
        
        public override void Execute()
        {
            executionBase.Execute();
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            _token = token;

            SubscribeToEvents();

            if (Data.ExecutionPlacement == Enums.ActionExecutionPlacement.Ground)
                await UniTask.WaitUntil(() => _internalData.InAir == false, cancellationToken: token);
            
            if (Data.IsChargeable)
                await PlayClipAndAwaitAsync(Data.ChargeClip, Data.ChargeTime, token);
            
            await PlayClipAndAwaitAsync(Data.ActionClip, Data.ActionClip.length, token);
            
            UnsubscribeToEvents();
            
            if (Data.ExecuteAfterClipPlayed)
                Execute();
        }

        private void SubscribeToEvents()
        {
            _internalData.AnimationEventsReceiver.ActionExecutionRequested += Execute;
            _internalData.AnimationEventsReceiver.MovingRequested += OnMovingRequested;
        }

        private void UnsubscribeToEvents()
        {
            _internalData.AnimationEventsReceiver.ActionExecutionRequested -= Execute;
            _internalData.AnimationEventsReceiver.MovingRequested -= OnMovingRequested;
        }
        
        private void OnMovingRequested() => MoveAsync().Forget();

        private async UniTask PlayClipAndAwaitAsync(AnimationClip clip, float time, CancellationToken token)
        {
            _internalData.Animator.PlayAnimationClip(clip);
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
        }

        private async UniTask MoveAsync()
        {
            float time = Data.UseClipLengthAsTime ? Data.ActionClip.length : Data.MovingTime;
            float speed = Data.Distance / time;
            float horizontalSpeed = speed * _internalData.FaceDirection.x;
            Vector2 velocityVector = new Vector2(Data.Vector.x * horizontalSpeed, Data.Vector.y * speed);
            
            _internalData.Physics.UpdateVelocity(velocityVector);
            _internalData.Physics.SuppressManualVelocityChange();

            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _token);
            
            _internalData.Physics.UnSuppressManualVelocityChange();
            _internalData.Physics.UpdateVelocity(Vector2.zero);
        }
    }
}