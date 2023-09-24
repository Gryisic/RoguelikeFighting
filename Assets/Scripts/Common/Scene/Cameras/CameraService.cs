using Common.Scene.Cameras.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras
{
    public class CameraService : MonoBehaviour, ICameraService
    {
        [SerializeField] private FollowingCamera _followingCamera;
        [SerializeField] private FocusCamera _focusCamera;

        private Camera _activeCamera;
        
        public void FollowUnit(Transform unitTransform, Enums.CameraDistanceType distanceType = Enums.CameraDistanceType.Neutral)
        {
            ChangeCamera(_followingCamera);

            _followingCamera.FollowUnit(unitTransform, distanceType);
        }

        public void FocusOn(Transform transformToFocusOn, Enums.CameraDistanceType distanceType = Enums.CameraDistanceType.Neutral) => 
            FocusOn(transformToFocusOn.position, distanceType);

        public void FocusOn(Vector2 positionToFocusOn, Enums.CameraDistanceType distanceType = Enums.CameraDistanceType.Neutral)
        {
            ChangeCamera(_focusCamera);
            
            _focusCamera.FocusOn(positionToFocusOn, distanceType);
        }

        private void ChangeCamera(Camera newCamera)
        {
            if (_activeCamera != null) 
                _activeCamera.Deactivate();
            
            _activeCamera = newCamera;
            _activeCamera.Activate();
        }
    }
}