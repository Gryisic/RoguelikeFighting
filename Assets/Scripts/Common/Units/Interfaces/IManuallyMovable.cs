using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IManuallyMovable
    {
        void StartMoving(Vector2 direction);
        void StopMoving();
    }
}