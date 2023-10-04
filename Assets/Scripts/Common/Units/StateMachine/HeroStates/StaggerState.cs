using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class StaggerState : HeroState, IDisposable
    {
        private CancellationTokenSource _staggerTokenSource;

        private bool _isStaggered;
        
        public StaggerState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

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
            _isStaggered = false;
                
            _staggerTokenSource.Cancel();
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
            
            _isStaggered = false;
            
            unitStatesChanger.ChangeState<IdleState>();
        }
    }
}