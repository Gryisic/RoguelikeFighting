using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Units.Knockback.Interfaces
{
    public interface IKnockbackStrategy
    {
        UniTask ExecuteAsync(float time, CancellationToken token);
    }
}