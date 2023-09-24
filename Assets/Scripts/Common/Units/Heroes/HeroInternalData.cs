using System;
using System.Threading;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Heroes
{
    public class HeroInternalData : UnitInternalData, IHeroInternalData, IDisposable
    {
        private CancellationTokenSource _actionUpdateTokenSource;
        private bool _isActionUpdating;
        
        public HeroAnimationData AnimationData { get; }
        public HeroActionsContainer ActionsContainer { get; private set; }

        public float DashDistance { get; private set; }
        public float DashForce { get; private set; }
        public int MaxDashesCount { get; private set; }
        public int RemainingDashes { get; private set; }
        
        public int MaxJumps { get; private set; }
        public int RemainingJumps { get; private set; }
        public bool JumpRequested { get; private set; }

        public Enums.HeroActionType LastActionType { get; private set; }
        
        public Enums.InputDirection InputDirection => DefineInputDirection();
        public Enums.HeroActionExecutionPlacement Placement => DefinePlacement();
        public bool CanDash => RemainingDashes > 0;
        
        public HeroInternalData(Transform transform, UnitPhysics physics, HeroAnimationData heroAnimationData, UnitAnimator animator, IUnitStatsData statsData, IActionsData actionsData, IAnimationEventsReceiver animationEventsReceiver, Type type) : base(transform, physics, animator, statsData, actionsData, animationEventsReceiver, type)
        {
            AnimationData = heroAnimationData;
        }

        public override void Dispose()
        {
            if (_isActionUpdating)
            {
                _actionUpdateTokenSource.Cancel();
                _actionUpdateTokenSource.Dispose();
            }
            
            ActionsData.Dispose();
        }

        public void SetActionsContainer(HeroActionsContainer container) => ActionsContainer = container;

        public void SetDashData(float distance, float force, int maxDashesCount)
        {
            DashDistance = distance;
            DashForce = force;
            MaxDashesCount = maxDashesCount;

            RemainingDashes = MaxDashesCount;
        }

        public void IncreaseRemainingDashes()
        {
            if (RemainingDashes < MaxDashesCount)
                RemainingDashes++;
        }

        public void DecreaseRemainingDashes()
        {
            if (RemainingDashes > 0)
                RemainingDashes--;
        }
        
        public void SetJumpData(int maxJumpsCount) => MaxJumps = maxJumpsCount;

        public void RequestJump() => JumpRequested = true;

        public void DecreaseRemainingJumps()
        {
            if (RemainingJumps > 0)
                RemainingJumps--;

            JumpRequested = false;
        }
        
        public void ResetJumps()
        {
            RemainingJumps = MaxJumps;

            JumpRequested = false;
        }

        public void SetAction(Enums.HeroActionType actionType)
        {
            LastActionType = actionType;
            
            if (_isActionUpdating)
            {
                _actionUpdateTokenSource.Cancel();
                _actionUpdateTokenSource.Dispose();
            }
            
            _actionUpdateTokenSource = new CancellationTokenSource();
            
            UpdateLastActionTypeAsync().Forget();
        }

        public void ResetAction()
        {
            if (_isActionUpdating)
            {
                _actionUpdateTokenSource.Cancel();
                _actionUpdateTokenSource.Dispose();
                
                _isActionUpdating = false;
            }
         
            LastActionType = Enums.HeroActionType.None;
        }
        
        private Enums.HeroActionExecutionPlacement DefinePlacement()
        {
            if (InAir == false)
                return Enums.HeroActionExecutionPlacement.Ground;

            return Enums.HeroActionExecutionPlacement.Air;
        }
        
        private Enums.InputDirection DefineInputDirection()
        {
            if (MoveDirection.y > 0)
                return Enums.InputDirection.Up;
            if (MoveDirection.y < 0)
                return Enums.InputDirection.Down;

            return Enums.InputDirection.Horizontal;
        }

        private async UniTask UpdateLastActionTypeAsync()
        {
            _isActionUpdating = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _actionUpdateTokenSource.Token);

            LastActionType = Enums.HeroActionType.None;
            
            _actionUpdateTokenSource.Dispose();
            _isActionUpdating = false;
        }
    }
}