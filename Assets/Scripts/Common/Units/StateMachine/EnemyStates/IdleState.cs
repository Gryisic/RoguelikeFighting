using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.EnemyStates
{
    public class IdleState : EnemyState, IDisposable
    {
        private CancellationTokenSource _waitingTokenSource;

        private float _distance;
        private bool _isWaiting;
        
        public IdleState(IUnitStatesChanger unitStatesChanger, IEnemyInternalData internalData) : base(unitStatesChanger, internalData) { }

        public void Dispose()
        {
            if (_isWaiting)
            {
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

        public override void Update()
        {
            if (internalData.HeroData == null)
                return;
            
            _distance = (internalData.HeroData.Transform.position - internalData.Transform.position).sqrMagnitude;
        }

        private async UniTask WaitAsync()
        {
            _isWaiting = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(internalData.Action.Data.CooldownTime), cancellationToken: _waitingTokenSource.Token);
            await UniTask.WaitUntil(() => _distance < 1, cancellationToken: _waitingTokenSource.Token);
            
            _waitingTokenSource.Dispose();
            
            _isWaiting = false;
            
            unitStatesChanger.ChangeState<ActionState>();
        }
    }
}