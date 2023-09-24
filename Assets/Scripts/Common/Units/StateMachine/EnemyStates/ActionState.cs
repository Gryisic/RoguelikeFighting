using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.EnemyStates
{
    public class ActionState : EnemyState, IDisposable
    {
        private CancellationTokenSource _executionToken;

        private bool _isExecuting;
        
        public ActionState(IUnitStatesChanger unitStatesChanger, IEnemyInternalData internalData) : base(unitStatesChanger, internalData) { }

        public void Dispose()
        {
            if (_isExecuting)
            {
                _isExecuting = false;
                
                _executionToken.Cancel();
                _executionToken.Dispose();
            }
        }
        
        public override void Enter()
        {
            _executionToken = new CancellationTokenSource();
            
            ExecuteAsync().Forget();
        }

        public override void Exit()
        {
            internalData.AnimationEventsReceiver.ResetSubscriptions();
            
            if (_isExecuting) 
                Dispose();
        }

        public override void Update() { }

        private async UniTask ExecuteAsync()
        {
            _isExecuting = true;
            
            if (internalData.Action.Data.RequireCharge)
                await PlayClipAndAwaitAsync(internalData.Action.Data.ChargeClip, internalData.Action.Data.ChargeTime);
            
            internalData.AnimationEventsReceiver.ActionExecutionRequested += internalData.Action.Execute;
            
            await PlayClipAndAwaitAsync(internalData.Action.Data.ActionClip, internalData.Action.Data.ActionClip.length);
            
            internalData.AnimationEventsReceiver.ActionExecutionRequested -= internalData.Action.Execute;
            
            _executionToken.Dispose();

            _isExecuting = false;
            
            unitStatesChanger.ChangeState<IdleState>();
        }
        
        private async UniTask PlayClipAndAwaitAsync(AnimationClip clip, float time)
        {
            internalData.Animator.PlayAnimationClip(clip);
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _executionToken.Token);
        }
    }
}