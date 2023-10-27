using System;
using System.Threading;
using Common.Units.ActionExecution;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.EnemyStates
{
    public class IdleState : EnemyState, IDisposable
    {
        private readonly ExecutionAwaiter _awaiter;
        private CancellationTokenSource _waitingTokenSource;

        private bool _isWaiting;

        public IdleState(IUnitStatesChanger unitStatesChanger, IEnemyInternalData internalData) : base(unitStatesChanger, internalData)
        {
            _awaiter = new ExecutionAwaiter(internalData);
        }

        public void Dispose()
        {
            if (_isWaiting)
            {
                _isWaiting = false;
                
                _waitingTokenSource.Cancel();
                _waitingTokenSource.Dispose();
            }
        }
        
        public override void Enter()
        {
            internalData.Animator.PlayDefaultAnimation(Enums.DefaultAnimation.Idle);
            
            _waitingTokenSource = new CancellationTokenSource();
            
            WaitAsync().Forget();
        }

        public override void Exit()
        {
            if (_isWaiting) 
                Dispose();
        }

        public override void Update() { }

        private async UniTask WaitAsync()
        {
            _isWaiting = true;

            await _awaiter.AwaitAsync(_waitingTokenSource.Token);

            _isWaiting = false;
            
            unitStatesChanger.ChangeState<ActionState>();
        }
    }
}