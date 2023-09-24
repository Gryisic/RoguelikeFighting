using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Gameplay.Waves
{
    public class TimerWaveRequirement : NextWaveRequirement, IDisposable
    {
        public override event Action Fulfilled;

        private readonly int _timer;

        private CancellationTokenSource _checkTokenSource;
        
        public TimerWaveRequirement(int timer)
        {
            _timer = timer;
        }

        public void Dispose()
        {
            _checkTokenSource?.Cancel();
            _checkTokenSource?.Dispose();
        }
        
        public override void StartChecking()
        {
            _checkTokenSource = new CancellationTokenSource();
            
            CheckAsync().Forget();
        }

        private async UniTask CheckAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_timer), cancellationToken: _checkTokenSource.Token);
            
            Fulfilled?.Invoke();
        }
    }
}