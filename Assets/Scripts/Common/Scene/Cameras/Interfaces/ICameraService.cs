using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras.Interfaces
{
    public interface ICameraService
    {
        void FollowUnit(Transform unitTransform, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);

        void FocusOn(Transform transformToFocusOn, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);
        void FocusOn(Vector2 positionToFocusOn, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);
    }
}