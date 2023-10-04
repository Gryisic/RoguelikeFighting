using System;
using System.Threading;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Models.Actions
{
    public class EnemyAction : UnitAction
    {
        private readonly IEnemyInternalData _internalData;
        
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
            SubscribeToEvents();

            if (Data.IsChargeable)
                await PlayClipAndAwaitAsync(Data.ChargeClip, Data.ChargeTime, token);
            
            await PlayClipAndAwaitAsync(Data.ActionClip, Data.ActionClip.length, token);
            
            UnsubscribeToEvents();
            
            if (Data.ExecuteAfterClipPlayed)
                Execute();
        }

        private async UniTask PlayClipAndAwaitAsync(AnimationClip clip, float time, CancellationToken token)
        {
            _internalData.Animator.PlayAnimationClip(clip);
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
        }
        
        private void SubscribeToEvents()
        {
            _internalData.AnimationEventsReceiver.ActionExecutionRequested += Execute;
        }
        
        private void UnsubscribeToEvents()
        {
            _internalData.AnimationEventsReceiver.ActionExecutionRequested -= Execute;
        }
    }
}