using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using Random = UnityEngine.Random;

namespace Common.Units.ActionExecution
{
    public class EverywhereAwaitStrategy : IExecutionAwaitStrategy
    {
        public async UniTask AwaitAsync(CancellationToken token)
        {
            float delay = Random.Range(Constants.DefaultEnemyAwaitTime - 0.5f, Constants.DefaultEnemyAwaitTime + 0.5f);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
        }
    }
}