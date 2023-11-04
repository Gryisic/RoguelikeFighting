using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    public class TimerWaveRequirement : WaveRequirement
    {
        public override event Action Fulfilled;

        private readonly float _timer;
        
        public TimerWaveRequirement(float timer)
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