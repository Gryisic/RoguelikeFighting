using System;
using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Units.ActionExecution
{
    public class ExecutionAwaiter
    {
        private readonly IEnemyInternalData _internalData;
        private readonly IExecutionAwaitStrategy _awaitStrategy;

        public ExecutionAwaiter(IEnemyInternalData internalData)
        {
            _internalData = internalData;
            _awaitStrategy = DefineStrategy(internalData.Data.ExecutionType);
        }
        
        public async UniTask AwaitAsync(CancellationToken token) => await _awaitStrategy.AwaitAsync(token);

        private IExecutionAwaitStrategy DefineStrategy(Enums.ActionExecutionAwait type)
        {
            switch (type)
            {
                case Enums.ActionExecutionAwait.Everywhere:
                    return new EverywhereAwaitStrategy();
                
                case Enums.ActionExecutionAwait.OnLineOrHigher:
                    return new OnLineExecutionAwaiter(_internalData.Transform, _internalData.HeroData.Transform);
                
                case Enums.ActionExecutionAwait.Near:
                    return new NearAwaitStrategy(_internalData.Transform, _internalData.HeroData.Transform, _internalData.Data.AttackDistance);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}