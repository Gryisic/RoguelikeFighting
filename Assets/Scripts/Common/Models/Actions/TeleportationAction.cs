using System;
using System.Threading;
using Common.Models.Actions.Templates;
using Common.Units;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Models.Actions
{
    public class TeleportationAction : ActionBase, IDisposable
    {
        private readonly UnitPhysics _physics;
        private readonly Transform _innerTransform;
        private readonly Transform _targetTransform;

        private CancellationTokenSource _teleportationTokenSource;

        public TeleportationAction(IUnitInternalData internalData, ActionTemplate data, ActionBase wrappedBase = null) : base(data, wrappedBase)
        {
            _physics = internalData.Physics;
            _innerTransform = internalData.Transform;
            
            if (internalData is IEnemyInternalData enemyInternalData)
                _targetTransform = enemyInternalData.HeroData.Transform;
        }
        
        public void Dispose()
        {
            _teleportationTokenSource?.Cancel();
            _teleportationTokenSource?.Dispose();
        }
        
        public override void Execute()
        {
            _teleportationTokenSource = new CancellationTokenSource();
            
            TeleportAsync().Forget();
        }

        private async UniTask TeleportAsync()
        {
            Vector2 newPosition = _targetTransform == null 
                ? (Vector2) _innerTransform.position + data.PositionRelativeToTarget 
                : (Vector2) _targetTransform.position + data.PositionRelativeToTarget;

            _innerTransform.position = newPosition;

            _physics.FreezeFalling();
            
            await UniTask.Delay(TimeSpan.FromSeconds(data.FreezeAfterMoving));
            
            _physics.UnfreezeFalling();
        }
    }
}