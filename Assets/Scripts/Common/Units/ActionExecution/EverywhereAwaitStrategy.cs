using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Units.ActionExecution
{
    public class EverywhereAwaitStrategy : IExecutionAwaitStrategy
    {
        public async UniTask AwaitAsync(CancellationToken token) => await UniTask.Delay(TimeSpan.FromSeconds(Constants.DefaultEnemyAwaitTime), cancellationToken: token);
    }
}