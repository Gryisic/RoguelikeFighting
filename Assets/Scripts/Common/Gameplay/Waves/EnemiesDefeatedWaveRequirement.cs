using System;
using System.Threading;

namespace Common.Gameplay.Waves
{
    public class EnemiesDefeatedWaveRequirement : NextWaveRequirement
    {
        public override event Action Fulfilled;

        public override void StartChecking(CancellationToken token) { }
    }
}