using System;
using System.Threading;

namespace Common.Gameplay.Waves
{
    public abstract class NextWaveRequirement
    {
        public abstract event Action Fulfilled;

        public abstract void StartChecking(CancellationToken token);
    }
}