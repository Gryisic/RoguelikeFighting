using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine
{
    public class StaggerState : IUnitState, IDisposable
    {
        private readonly IUnitStatesChanger _unitStatesChanger;
        private readonly IUnitInternalData _internalData;
        
        private CancellationTokenSource _staggerTokenSource;

        private bool _isStaggered;

        public StaggerState(IUnitStatesChanger unitStatesChanger, IUnitInternalData internalData)
        {
            _unitStatesChanger = unitStatesChanger;
            _internalData = internalData;
        }

        public void Dispose()
        {
            if (_isStaggered)
            {
                _isStaggered = false;
                
                _staggerTokenSource.Cancel();
                _staggerTokenSource.Dispose();
            }
        }
        
        public void Enter()
        {
            _staggerTokenSource = new CancellationTokenSource();
            
            StaggerAsync().Forget();
        }

        public void Exit()
        {
            _isStaggered = false;
                
            _staggerTokenSource.Cancel();
        }

        public void Update() { }

        private async UniTask StaggerAsync()
        {
            float timer = 0;

            _isStaggered = true;

            _internalData.Animator.PlayDefaultAnimation(Enums.DefaultAnimation.TakeHitMedium);
            
            while (timer < _internalData.StaggerTime && _staggerTokenSource.IsCancellationRequested == false)
            {
                timer += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }

            await UniTask.WaitUntil(() => _internalData.InAir == false, cancellationToken: _staggerTokenSource.Token);
            
            _isStaggered = false;
            
            switch (_internalData)
            {
                case IHeroInternalData _:
                    _unitStatesChanger.ChangeState<HeroStates.IdleState>();
                    break;
                
                case IEnemyInternalData _:
                    _unitStatesChanger.ChangeState<EnemyStates.IdleState>();
                    break;
            }
        }
    }
}