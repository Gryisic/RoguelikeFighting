using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.EnemyStates
{
    public class StaggerState : EnemyState, IDisposable
    {
        private CancellationTokenSource _staggerTokenSource;

        private bool _isStaggered;
        
        public StaggerState(IUnitStatesChanger unitStatesChanger, IEnemyInternalData internalData) : base(unitStatesChanger, internalData) { }

        public void Dispose()
        {
            if (_isStaggered)
            {
                _isStaggered = false;
                
                _staggerTokenSource.Cancel();
                _staggerTokenSource.Dispose();
            }
        }
        
        public override void Enter()
        {
            _staggerTokenSource = new CancellationTokenSource();
            
            StaggerAsync().Forget();
        }

        public override void Exit()
        {
            if (_isStaggered)
            {
                _isStaggered = false;
                
                _staggerTokenSource.Cancel();
            }
        }

        public override void Update() { }

        private async UniTask StaggerAsync()
        {
            float timer = 0;

            _isStaggered = true;

            internalData.Animator.PlayDefaultAnimation(Enums.DefaultAnimation.TakeHitMedium);
            
            while (timer < internalData.StaggerTime && _staggerTokenSource.IsCancellationRequested == false)
            {
                timer += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }

            await UniTask.WaitUntil(() => internalData.InAir == false, cancellationToken: _staggerTokenSource.Token);
            
            _staggerTokenSource.Dispose();
            _isStaggered = false;
            
            unitStatesChanger.ChangeState<IdleState>();
        }
    }
}