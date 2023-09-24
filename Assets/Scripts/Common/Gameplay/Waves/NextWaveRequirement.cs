using System;

namespace Common.Gameplay.Waves
{
    public abstract class NextWaveRequirement
    {
        public abstract event Action Fulfilled;

        public abstract void StartChecking();
    }
}