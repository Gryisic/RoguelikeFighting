using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Units.ActionExecution
{
    public class NearAwaitStrategy : IExecutionAwaitStrategy
    {
        private readonly Transform _innerTransform;
        private readonly Transform _targetTransform;
        private readonly float _executionDistance;

        public NearAwaitStrategy(Transform innerTransform, Transform targetTransform, float distance)
        {
            _innerTransform = innerTransform;
            _targetTransform = targetTransform;
            _executionDistance = Mathf.Pow(distance, 2);
        }

        public async UniTask AwaitAsync(CancellationToken token)
        {
            float delay = Random.Range(Constants.DefaultEnemyAwaitTime - 0.5f, Constants.DefaultEnemyAwaitTime + 0.5f);

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            
            while (token.IsCancellationRequested == false)
            {
                float distance = (_targetTransform.position - _innerTransform.position).sqrMagnitude;

                if (distance < _executionDistance)
                    break;

                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}