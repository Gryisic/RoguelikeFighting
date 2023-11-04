using System;
using System.Collections.Generic;
using System.Threading;
using Common.Units;

namespace Common.Gameplay.Waves
{
    public class EnemiesDefeatedWaveRequirement : WaveRequirement
    {
        private IReadOnlyList<Enemy> _enemies;
        
        public override event Action Fulfilled;

        public override void StartChecking(CancellationToken token) { } 
    }
}