using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Units.ActionExecution
{
    public class OnLineExecutionAwaiter : IExecutionAwaitStrategy
    {
        private readonly Transform _innerTransform;
        private readonly Transform _targetTransform;

        public OnLineExecutionAwaiter(Transform innerTransform, Transform targetTransform)
        {
            _innerTransform = innerTransform;
            _targetTransform = targetTransform;
        }

        public async UniTask AwaitAsync(CancellationToken token)
        {
            float delay = Random.Range(Constants.DefaultEnemyAwaitTime - 0.5f, Constants.DefaultEnemyAwaitTime + 0.5f);

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            
            while (token.IsCancellationRequested == false)
            {
                bool isOnLineOrHigher = _innerTransform.position.y <= _targetTransform.position.y;

                if (isOnLineOrHigher)
                    break;

                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}