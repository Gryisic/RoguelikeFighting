using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface ILegacyUnitNavigationStrategy
    {
        void CalculatePositions(Vector2 unitPosition, float directionMultiplier, out Vector2 startPosition, out Vector2 endPosition);
    }
}