using System;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Units
{
    public class AnimationEventsReceiver : MonoBehaviour, IAnimationEventsReceiver
    {
        public event Action ActionExecutionRequested;
        public event Action MovingRequested;

        public void RequestActionExecution() => ActionExecutionRequested?.Invoke();
        
        public void RequestMoving() => MovingRequested?.Invoke();
        
        public void ResetSubscriptions()
        {
            ActionExecutionRequested = null;
            MovingRequested = null;
        }
    }
}