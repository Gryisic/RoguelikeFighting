using System;
using System.Threading;
using Common.Models.Actions;
using Common.Units.Heroes;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class ActionState : HeroState, IAttackExecutor, IDisposable
    {
        private CancellationTokenSource _actionTokenSource;

        private HeroAction _currentAction;

        private bool _isExecuting;
        
        public ActionState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }
        
        public override void Enter()
        {
            _actionTokenSource = new CancellationTokenSource();

            ExecuteAsync().Forget();
        }

        public override void Exit()
        {
            _currentAction = null;
            
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

        public void Attack()
        {
            internalData.SetAction(Enums.HeroActionType.BasicAttack);
        }
        
        private async UniTask ExecuteAsync()
        {
            _isExecuting = true;

            while (_actionTokenSource.IsCancellationRequested == false && internalData.LastActionType != Enums.HeroActionType.None)
            {
                if (internalData.ActionsContainer.TryGetNextAction(out HeroAction action))
                {
                    if (internalData.InAir)
                        internalData.Physics.FreezeFalling();
                    
                    _currentAction = action;
                    AnimationClip currentClip = action.Data.AnimationClip;

                    internalData.AnimationEventsReceiver.ActionExecutionRequested += action.Execute;
                    internalData.AnimationEventsReceiver.MovingRequested += OnMovingRequested;
                    
                    internalData.Animator.PlayAnimationClip(currentClip);
                    internalData.ResetAction();
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(currentClip.length), cancellationToken: _actionTokenSource.Token);
                    
                    internalData.AnimationEventsReceiver.ActionExecutionRequested -= action.Execute;
                    internalData.AnimationEventsReceiver.MovingRequested -= OnMovingRequested;
                }
                else
                {
                    break;
                }
            }
            
            unitStatesChanger.ChangeState<IdleState>();

            _actionTokenSource.Dispose();
            _isExecuting = false;
        }

        private void OnMovingRequested() => MoveAsync().Forget();

        private async UniTask MoveAsync()
        {
            float time = _currentAction.Data.UseClipLengthAsTime ? _currentAction.Data.AnimationClip.length : _currentAction.Data.Time;
            float speed = (_currentAction.Data.Distance / time) * internalData.FaceDirection.x;
            
            internalData.Physics.UpdateHorizontalVelocity(speed);
            
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _actionTokenSource.Token);
            
            internalData.Physics.UpdateHorizontalVelocity(0);
        }
    }
}