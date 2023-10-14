using System;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    public class AnimationEventsReceiver : MonoBehaviour, IAnimationEventsReceiver
    {
        public event Action ActionExecutionRequested;
        public event Action MovingRequested;
        public event Action ParticlesEmitRequested;

        public void RequestActionExecution() => ActionExecutionRequested?.Invoke();
        
        public void RequestMoving() => MovingRequested?.Invoke();

        public void RequestParticlesEmit() => ParticlesEmitRequested?.Invoke();

        public void ResetSubscriptions()
        {
            ActionExecutionRequested = null;
            MovingRequested = null;
            ParticlesEmitRequested = null;
        }
    }
}