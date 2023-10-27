using System;
using System.Threading;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Common.Units.Legacy;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Actions
{
    public class LegacyAction : UnitAction
    {
        private readonly IHeroInternalData _heroInternalData;
        private readonly ILegacyUnit _unit;
        
        private ILegacyUnitNavigationStrategy _navigationStrategy;
        private CancellationTokenSource _executionTokenSource;
        
        public new LegacyActionTemplate Data { get; }
        
        public LegacyAction(IHeroInternalData heroInternalData, ILegacyUnit legacyUnit) : base(legacyUnit.ActionData)
        {
            Data = legacyUnit.ActionData;
            _heroInternalData = heroInternalData;
            _unit = legacyUnit;

            executionBase = DefineExecutionBase(Data.BaseEffect, _unit);
        }

        public override void Dispose()
        {
            _executionTokenSource?.Cancel();
            _executionTokenSource?.Dispose();
            
            base.Dispose();
        }

        public override void Execute() => executionBase.Execute();

        public void Start()
        {
            _executionTokenSource = new CancellationTokenSource();

            _navigationStrategy = DefineNavigationStrategy(Data.NavigationStrategy);
            
            ExecuteAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _unit.AnimationEventsReceiver.ActionExecutionRequested += _unit.AnimationEventsExecutor.OnActionExecutionRequested;
            _unit.AnimationEventsReceiver.ParticlesEmitRequested += _unit.AnimationEventsExecutor.OnParticlesEmitRequested;
            _unit.AnimationEventsReceiver.MovingRequested += _unit.AnimationEventsExecutor.OnMovingRequested;
        }

        private void UnsubscribeToEvents()
        {
            _unit.AnimationEventsReceiver.ActionExecutionRequested -= _unit.AnimationEventsExecutor.OnActionExecutionRequested;
            _unit.AnimationEventsReceiver.ParticlesEmitRequested -= _unit.AnimationEventsExecutor.OnParticlesEmitRequested;
            _unit.AnimationEventsReceiver.MovingRequested -= _unit.AnimationEventsExecutor.OnMovingRequested;
        }

        private void CalculateSpaceData(out Vector2 spawnPoint, out Vector2 finalPosition, out float distance)
        {
            float directionMultiplier = _heroInternalData.FaceDirection.x.Reversed();
            Vector3 unitPosition = _heroInternalData.Transform.position;

            _navigationStrategy.CalculatePositions(unitPosition, directionMultiplier, out spawnPoint, out finalPosition);

            distance = Mathf.Abs(spawnPoint.x - finalPosition.x);
        }

        private ILegacyUnitNavigationStrategy DefineNavigationStrategy(Enums.LegacyUnitNavigationStrategy dataNavigationStrategy)
        {
            switch (dataNavigationStrategy)
            {
                case Enums.LegacyUnitNavigationStrategy.Linear:
                    return new LinearNavigationStrategy(Data.FloorMask, _heroInternalData.Physics.LastGroundedPosition);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataNavigationStrategy), dataNavigationStrategy, null);
            }
        }

        private async UniTask ExecuteAsync()
        {
            CalculateSpaceData(out Vector2 spawnPoint, out Vector2 finalPosition, out float distance);
            
            _unit.Activate(spawnPoint);
            _unit.AnimationEventsExecutor.SetData(this, _executionTokenSource.Token);
            
            SubscribeToEvents();

            float delay = distance / Data.MovementsSpeed;

            _unit.MoveTo(finalPosition, Data.MovementsSpeed);
            _unit.Animator.PlayAnimationClip(Data.MoveClip);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _executionTokenSource.Token);
            
            _unit.Animator.PlayAnimationClip(Data.ActionClip);
            
            await UniTask.Delay(TimeSpan.FromSeconds(Data.ActionClip.length), cancellationToken: _executionTokenSource.Token);
            
            _unit.MoveTo(spawnPoint, Data.MovementsSpeed);
            _unit.Animator.PlayAnimationClip(Data.MoveClip);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _executionTokenSource.Token);
            
            UnsubscribeToEvents();
            
            _unit.Deactivate();
        }
    }
}