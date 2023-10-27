using System;
using System.Threading;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class AirState : HeroState, IAttackExecutor, IJumpExecutor, ILegacySkillExecutor, IDisposable
    {
        private CancellationTokenSource _jumpTokenSource;

        private bool _isJumping;
        
        public AirState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

        public override void Enter()
        {
            _jumpTokenSource = new CancellationTokenSource();
            
            if (internalData.InAir)
                FallAsync().Forget();
            else if (internalData.JumpRequested)
                JumpAsync(Constants.JumpForce).Forget();
            else
                unitStatesChanger.ChangeState<IdleState>();
        }

        public override void Exit()
        {
            if (internalData.InAir == false)
                internalData.ResetJumps();
        }

        public override void Update()
        {
            Vector2 direction = internalData.MoveDirection * Constants.PlayerMovementSpeed;
                
            internalData.Physics.UpdateHorizontalVelocity(direction.x);
        }

        public void Dispose()
        {
            if (_isJumping)
            {
                _jumpTokenSource.Cancel();
                _jumpTokenSource.Dispose();
            }
        }

        public void Attack()
        {
            if (_isJumping)
            {
                _isJumping = false;
                
                _jumpTokenSource.Cancel();
            }
            
            internalData.SetAction(Enums.HeroActionType.BasicAttack);
            
            unitStatesChanger.ChangeState<ActionState>();
        }
        
        public void LegacySkill(Enums.HeroActionType skillType)
        {
            if (internalData.ActionsContainer.TryGetLegacyAction(skillType, out LegacyAction action))
                action.Start();
        }
        
        public void Jump()
        {
            if (internalData.RemainingJumps > 0)
            {
                _jumpTokenSource.Cancel();
                internalData.ParticlesPlayer.PlayGenericParticle(Enums.GenericParticle.MidAirJump, internalData.FaceDirection.x, false);

                _jumpTokenSource = new CancellationTokenSource();
            
                JumpAsync(Constants.JumpForce * 0.75f).Forget();
            }
        }
        
        private async UniTask JumpAsync(float jumpForce)
        {
            _isJumping = true;

            AnimationClip currentClip = internalData.AnimationData.RiseClip;
            Vector2 force = Vector2.up * jumpForce;
            
            internalData.DecreaseRemainingJumps();
            internalData.Physics.AddVerticalForce(force);
            internalData.Animator.PlayAnimationClip(currentClip);

            await UniTask.Delay(TimeSpan.FromSeconds(currentClip.length), cancellationToken: _jumpTokenSource.Token);
            
            FallAsync().Forget();
        }

        private async UniTask FallAsync()
        {
            _isJumping = true;
            
            internalData.Animator.PlayAnimationClip(internalData.AnimationData.CycleClip);

            await UniTask.WaitUntil(() => internalData.InAir == false, cancellationToken: _jumpTokenSource.Token);

            internalData.ParticlesPlayer.PlayGenericParticle(Enums.GenericParticle.Landing, internalData.FaceDirection.x, false);

            if (internalData.MoveDirection.x != 0)
            {
                _jumpTokenSource.Cancel();
                
                _isJumping = false;
                
                unitStatesChanger.ChangeState<MovingState>();

                return;
            }
            
            AnimationClip currentClip = internalData.AnimationData.LandingClip;
            
            internalData.Animator.PlayAnimationClip(currentClip);

            await UniTask.Delay(TimeSpan.FromSeconds(currentClip.length), cancellationToken: _jumpTokenSource.Token);

            _isJumping = false;
            
            unitStatesChanger.ChangeState<IdleState>();
        }
    }
}