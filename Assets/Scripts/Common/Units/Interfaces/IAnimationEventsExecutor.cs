using System.Threading;
using Common.Models.Actions;

namespace Common.Units.Interfaces
{
    public interface IAnimationEventsExecutor
    {
        void SetData(UnitAction action, CancellationToken token);
        
        void OnActionExecutionRequested();
        void OnParticlesEmitRequested();
        void OnMovingRequested();
    }
}