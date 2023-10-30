using System;
using System.Threading;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class ActionState : HeroState, IAttackExecutor, ISkillExecutor, IDisposable
    {
        private CancellationTokenSource _actionTokenSource;
        
        private bool _isExecuting;
        
        public ActionState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }
        
        public override void Enter()
        {
            _actionTokenSource = new CancellationTokenSource();

            ExecuteAsync().Forget();
        }

        public override void Exit()
        {
            internalData.ResetAction();
            internalData.Physics.UnSuppressManualVelocityChange();
            internalData.Physics.UnfreezeFalling();
            internalData.ActionsContainer.ResetAction();
            internalData.AnimationEventsReceiver.ResetSubscriptions();
        }

        public override void Update() { }
        
        public void Dispose()
        {
            if (_isExecuting)
            {
                _actionTokenSource.Cancel();
                _actionTokenSource.Dispose();
            }
        }

        public void Attack() => internalData.SetAction(Enums.HeroActionType.BasicAttack);

        public void Skill() => internalData.SetAction(Enums.HeroActionType.Skill);

        private async UniTask ExecuteAsync()
        {
            _isExecuting = true;

            while (_actionTokenSource.IsCancellationRequested == false && internalData.LastActionType != Enums.HeroActionType.None)
            {
                if (internalData.ActionsContainer.TryGetNextAction(out HeroAction action))
                {
                    if (internalData.InAir)
                        internalData.Physics.FreezeFalling();

                    AnimationClip currentClip = action.Data.ActionClip;

                    internalData.AnimationEventsReceiver.ActionExecutionRequested += internalData.AnimationEventsExecutor.OnActionExecutionRequested;
                    internalData.AnimationEventsReceiver.MovingRequested += internalData.AnimationEventsExecutor.OnMovingRequested;
                    internalData.AnimationEventsReceiver.ParticlesEmitRequested += internalData.AnimationEventsExecutor.OnParticlesEmitRequested;
                    
                    internalData.AnimationEventsExecutor.SetData(action, _actionTokenSource.Token);
                    internalData.Animator.PlayAnimationClip(currentClip);
                    internalData.ResetAction();
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(currentClip.length), cancellationToken: _actionTokenSource.Token);

                    internalData.AnimationEventsReceiver.ActionExecutionRequested -= internalData.AnimationEventsExecutor.OnActionExecutionRequested;
                    internalData.AnimationEventsReceiver.MovingRequested -= internalData.AnimationEventsExecutor.OnMovingRequested;
                    internalData.AnimationEventsReceiver.ParticlesEmitRequested -= internalData.AnimationEventsExecutor.OnParticlesEmitRequested;
                }
                else
                {
                    break;
                }
            }
            
            unitStatesChanger.ChangeState<IdleState>();

            _isExecuting = false;
        }
    }
}