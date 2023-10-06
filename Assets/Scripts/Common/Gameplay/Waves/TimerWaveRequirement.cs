using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Gameplay.Waves
{
    public class TimerWaveRequirement : NextWaveRequirement
    {
        public override event Action Fulfilled;

        private readonly int _timer;
        
        public TimerWaveRequirement(int timer)
        {
            _timer = timer;
        }

        public override void StartChecking(CancellationToken token) => CheckAsync(token).Forget();

        private async UniTask CheckAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timer), cancellationToken: token);
            
            Fulfilled?.Invoke();
        }
    }
}