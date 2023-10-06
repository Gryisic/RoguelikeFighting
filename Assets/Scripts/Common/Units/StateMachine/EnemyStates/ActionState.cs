using System;
using System.Collections.Generic;
using System.Threading;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.EnemyStates
{
    public class ActionState : EnemyState, IDisposable
    {
        private CancellationTokenSource _executionTokenSource;

        private bool _isExecuting;

        public ActionState(IUnitStatesChanger unitStatesChanger, IEnemyInternalData internalData) : base(unitStatesChanger, internalData) { }

        public void Dispose()
        {
            if (_isExecuting)
            {
                _isExecuting = false;
                
                _executionTokenSource.Cancel();
                _executionTokenSource.Dispose();
            }
        }
        
        public override void Enter()
        {
            _executionTokenSource = new CancellationTokenSource();
            
            ExecuteAsync().Forget();
        }

        public override void Exit()
        {
            internalData.AnimationEventsReceiver.ResetSubscriptions();
            internalData.Physics.UnfreezeFalling();

            if (_isExecuting) 
                Dispose();
        }

        public override void Update() { }

        private async UniTask ExecuteAsync()
        {
            _isExecuting = true;

            for (var i = 0; i < internalData.Actions.Count; i++)
            {
                EnemyAction action = internalData.Actions[i];
                await action.ExecuteAsync(_executionTokenSource.Token);
            }

            _executionTokenSource.Dispose();
            
            _isExecuting = false;
            
            unitStatesChanger.ChangeState<IdleState>();
        }
    }
}