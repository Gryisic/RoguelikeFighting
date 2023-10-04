using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.ActionExecution
{
    public class OnLineExecutionAwaiter : IExecutionAwaitStrategy
    {
        private readonly Transform _innerTransform;
        private readonly Transform _targetTransform;
        private readonly float _executionDistance;

        public OnLineExecutionAwaiter(Transform innerTransform, Transform targetTransform, float executionDistance)
        {
            _innerTransform = innerTransform;
            _targetTransform = targetTransform;
            _executionDistance = Mathf.Pow(executionDistance, 2);
        }

        public async UniTask AwaitAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultEnemyAwaitTime), cancellationToken: token);
            
            while (token.IsCancellationRequested == false)
            {
                Vector3 innerPosition = _innerTransform.position;
                Vector3 targetPosition = _targetTransform.position;
                float distance = (targetPosition - innerPosition).sqrMagnitude;
                bool isOnLineOrHigher = innerPosition.y <= targetPosition.y;

                if (distance < _executionDistance && isOnLineOrHigher)
                    break;

                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}