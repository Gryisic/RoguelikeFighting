using System;
using System.Threading;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class CrouchState : HeroState, IAttackExecutor, IDashExecutor, IJumpExecutor, ISkillExecutor, IHealExecutor, ILegacySkillExecutor, IDisposable
    {
        private CancellationTokenSource _crouchTokenSource;
        
        public CrouchState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

        public override void Enter()
        {
            if (internalData.IsCrouching == false)
            {
                _crouchTokenSource = new CancellationTokenSource();
                
                CrouchAsync().Forget();
            }
            else
            {
                internalData.Animator.PlayAnimationClip(internalData.AnimationData.CrouchCycleClip);
            }
        }

        public override void Exit()
        {
            _crouchTokenSource?.Cancel();
        }

        public override void Update()
        {
            if (internalData.MoveDirection.y >= 0 && internalData.IsCrouching)
            {
                _crouchTokenSource?.Cancel();
                _crouchTokenSource = new CancellationTokenSource();
                
                StandAsync().Forget();
            }
        }
        
        public void Dispose()
        {
            if (internalData.IsCrouching == false)
                return;
            
            _crouchTokenSource?.Cancel();
            _crouchTokenSource?.Dispose();
        }
        
        public void Dash()
        {
            if (internalData.CanDash)
                unitStatesChanger.ChangeState<DashState>();
        }

        public void Jump()
        {
            if (internalData.InAir) 
                return;
            
            if (internalData.InputDirection == Enums.InputDirection.Down)
                internalData.Physics.DropThroughPlatform();

            unitStatesChanger.ChangeState<AirState>();
        }

        public void Attack() => SetActionAndChangeState(Enums.HeroActionType.BasicAttack);

        public void Skill() => SetActionAndChangeState(Enums.HeroActionType.Skill);

        public void Heal() => SetActionAndChangeState(Enums.HeroActionType.Heal);

        public void LegacySkill(Enums.HeroActionType skillType)
        {
            if (internalData.ActionsContainer.TryGetLegacyAction(skillType, out LegacyAction action))
                action.Start();
        }
        
        private void SetActionAndChangeState(Enums.HeroActionType actionType)
        {
            if (internalData.LastActionType == Enums.HeroActionType.None)
                internalData.SetAction(actionType);
            
            unitStatesChanger.ChangeState<ActionState>();
        }
        
        private async UniTask CrouchAsync()
        {
            internalData.SetCrouching(true);
            
            AnimationClip currentClip = internalData.AnimationData.CrouchClip;
            internalData.Animator.PlayAnimationClip(currentClip);

            await UniTask.Delay(TimeSpan.FromSeconds(currentClip.length), cancellationToken: _crouchTokenSource.Token);

            currentClip = internalData.AnimationData.CrouchCycleClip;
            
            internalData.Animator.PlayAnimationClip(currentClip);
        }
        
        private async UniTask StandAsync()
        {
            internalData.SetCrouching(false);
            
            AnimationClip currentClip = internalData.AnimationData.StandingClip;
            internalData.Animator.PlayAnimationClip(currentClip);

            await UniTask.Delay(TimeSpan.FromSeconds(currentClip.length), cancellationToken: _crouchTokenSource.Token);
            
            unitStatesChanger.ChangeState<IdleState>();
        }
    }
}