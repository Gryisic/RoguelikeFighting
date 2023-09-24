using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Models.Projectiles.Interfaces
{
    public interface ILaunchStrategy
    {
        UniTask LaunchAsync(Vector2 from, Vector2 to, CancellationToken token);
    }
}