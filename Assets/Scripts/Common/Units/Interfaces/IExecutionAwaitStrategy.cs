using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Units.Interfaces
{
    public interface IExecutionAwaitStrategy
    {
        UniTask AwaitAsync(CancellationToken token);
    }
}