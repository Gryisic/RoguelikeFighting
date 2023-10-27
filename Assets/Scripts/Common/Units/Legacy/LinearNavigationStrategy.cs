using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Legacy
{
    public class LinearNavigationStrategy : ILegacyUnitNavigationStrategy
    {
        private readonly LayerMask _floorMask;
        private readonly Vector2 _heroGroundedPosition;
        
        public LinearNavigationStrategy(LayerMask floorMask, Vector2 heroGroundedPosition)
        {
            _floorMask = floorMask;
            _heroGroundedPosition = heroGroundedPosition;
        }
        
        public void CalculatePositions(Vector2 unitPosition, float directionMultiplier, out Vector2 startPosition, out Vector2 endPosition)
        {
            float offset = 0.1f;
            
            TryGetFloorPositionWithOffset(unitPosition, Mathf.Infinity, offset, out Vector2 origin);
            
            unitPosition = new Vector2(unitPosition.x, _heroGroundedPosition.y);
            
            RaycastHit2D wallHit = Physics2D.Raycast(origin, Vector2.right * directionMultiplier, Constants.DefaultLegacyUnitSpawnDistance, _floorMask);
            
            float heightDelta = unitPosition.y - origin.y + offset;
            float horizontalPosition = GetPositionData(unitPosition, directionMultiplier, wallHit, out float positionY, out float delta);
            
            Vector2 position = new Vector2(unitPosition.x, positionY);
            Vector2 lastPosition = position;
            float step = 0.05f;
            int stepsCount = (int)(delta / step);
            
            for (int i = 0; i < stepsCount; i++)
            {
                if (TryGetFloorPosition(position, heightDelta, out Vector2 point))
                {
                    position = new Vector2(point.x, position.y);
                    lastPosition = position;
                }
                else
                {
                    TryGetFloorPosition(lastPosition, Mathf.Infinity, out startPosition);
                    TryGetFloorPosition(unitPosition, Mathf.Infinity, out endPosition);
                    startPosition = new Vector2(startPosition.x, unitPosition.y);
                    
                    return;
                }

                position = new Vector2(position.x + step * directionMultiplier, position.y);
            }

            startPosition = new Vector2(horizontalPosition, unitPosition.y);
            TryGetFloorPosition(unitPosition, Mathf.Infinity, out endPosition);
        }
        
        private float GetPositionData(Vector2 unitPosition, float directionMultiplier, RaycastHit2D wallHit, out float positionY, out float delta)
        {
            float horizontalPosition;
            
            if (wallHit)
            {
                horizontalPosition = wallHit.point.x;
                positionY = wallHit.point.y;
                delta = Mathf.Abs(horizontalPosition - unitPosition.x);
            }
            else
            {
                horizontalPosition = unitPosition.x + Constants.DefaultLegacyUnitSpawnDistance * directionMultiplier;
                positionY = _heroGroundedPosition.y;
                delta = Constants.DefaultLegacyUnitSpawnDistance;
            }

            return horizontalPosition;
        }

        private bool TryGetFloorPosition(Vector2 origin, float distance, out Vector2 point)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, _floorMask);
            
            if (hit)
            {
                point = hit.point;
                return true;
            }

            point = Vector2.zero;
            return false;
        }
        
        private bool TryGetFloorPositionWithOffset(Vector2 origin, float distance, float offset, out Vector2 point)
        {
            if (TryGetFloorPosition(origin, distance, out point))
            {
                point = new Vector2(point.x, point.y + offset);

                return true;
            }
            
            return false;
        }
    }
}