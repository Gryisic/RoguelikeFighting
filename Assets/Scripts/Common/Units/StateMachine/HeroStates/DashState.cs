using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Units.StateMachine.HeroStates
{
    public class DashState : HeroState, IAttackExecutor, IDisposable
    {
        private CancellationTokenSource _dashTokenSource;
        private CancellationTokenSource _dashRefreshingTokenSource;

        private bool _isDashing;
        private bool _isDashRefreshing;
        
        public DashState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

        public override void Enter()
        {
            _dashTokenSource = new CancellationTokenSource();
            _dashRefreshingTokenSource = new CancellationTokenSource();
            
            DashAsync().Forget();
        }

        public override void Exit()
        {
            
        }

        public override void Update() { }
        
        public void Dispose()
        {
            if (_isDashing && _dashTokenSource != null)
            {
                _dashTokenSource.Cancel();
                _dashTokenSource.Dispose();
            }
            
            if (_isDashRefreshing && _dashRefreshingTokenSource != null)
            {
                _dashRefreshingTokenSource.Cancel();
                _dashRefreshingTokenSource.Dispose();
            }
        }
        
        public void Attack()
        {
            if (_isDashing)
            {
                _dashTokenSource.Cancel();
                _dashTokenSource.Dispose();

                _isDashing = false;
                
                internalData.Physics.UpdateHorizontalVelocity(0);
                unitStatesChanger.ChangeState<ActionState>();
            }
        }

        private async UniTask DashAsync()
        {
            _isDashing = true;
            
            internalData.ParticlesPlayer.PlayGenericParticle(Enums.GenericParticle.Dash, internalData.FaceDirection.x);
            internalData.Animator.PlayAnimationClip(internalData.AnimationData.DashClip);
            internalData.SetAction(Enums.HeroActionType.Dash);
            internalData.DecreaseRemainingDashes();
            internalData.Physics.UpdateHorizontalVelocity(Constants.PlayerMovementSpeed * internalData.DashForce * internalData.FaceDirection.x);

            RefreshDashesAsync().Forget();
                    
            await UniTask.Delay(TimeSpan.FromSeconds(internalData.DashDistance / internalData.DashForce), cancellationToken: _dashTokenSource.Token);
            
            internalData.Physics.UpdateHorizontalVelocity(0);
            
            _dashTokenSource.Dispose();
            _isDashing = false;
            
            unitStatesChanger.ChangeState<IdleState>();
        }
        
        private async UniTask RefreshDashesAsync()
        {
            _isDashRefreshing = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DashRefreshingTime), cancellationToken: _dashRefreshingTokenSource.Token);
            
            internalData.IncreaseRemainingDashes();

            _dashRefreshingTokenSource.Dispose();
            _isDashRefreshing = false;
        }
    }
}