using System;

namespace Common.Units.Interfaces
{
    public interface IAnimationEventsReceiver
    {
        event Action ActionExecutionRequested;
        event Action MovingRequested;
        event Action ParticlesEmitRequested;

        void ResetSubscriptions();
        void RequestParticlesEmit();
    }
}