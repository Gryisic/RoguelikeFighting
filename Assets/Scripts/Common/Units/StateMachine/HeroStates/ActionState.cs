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
<<<<<<< Updated upstream

        private void OnParticlesEmitRequested()
        {
            HeroActionTemplate data = _currentAction.Data;
            IParticleData particleData = new ParticleData(data.ParticleForCopy, data.ParticleID, data.Rotation);
            
            internalData.ParticlesPlayer.Play(particleData);
        }

        private void OnMovingRequested() => MoveAsync().Forget();

        private async UniTask MoveAsync()
        {
            float freezeTime = _currentAction.Data.FreezeAfterMoving;
            float movingTime = _currentAction.Data.UseClipLengthAsTime ? _currentAction.Data.ActionClip.length : _currentAction.Data.MovingTime;
            float speed = _currentAction.Data.Distance / movingTime;
            float horizontalSpeed = speed * internalData.FaceDirection.x;
            Vector2 velocityVector = new Vector2(_currentAction.Data.Vector.x * horizontalSpeed, _currentAction.Data.Vector.y * speed);
            
            internalData.Physics.UpdateVelocity(velocityVector);
            internalData.Physics.SuppressManualVelocityChange();

            await UniTask.Delay(TimeSpan.FromSeconds(movingTime), cancellationToken: _actionTokenSource.Token);
            
            internalData.Physics.FreezeFalling();
            
            await UniTask.Delay(TimeSpan.FromSeconds(freezeTime), cancellationToken: _actionTokenSource.Token);
            
            internalData.Physics.UnfreezeFalling();
            internalData.Physics.UnSuppressManualVelocityChange();
            internalData.Physics.UpdateVelocity(Vector2.zero);
        }
=======
>>>>>>> Stashed changes
    }
}