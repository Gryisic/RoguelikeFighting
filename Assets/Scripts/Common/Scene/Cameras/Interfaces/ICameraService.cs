using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras.Interfaces
{
    public interface ICameraService : IService
    {
        void FollowUnit(Transform unitTransform, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);

        void FocusOn(Transform transformToFocusOn, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);
        void FocusOn(Vector2 positionToFocusOn, Enums.CameraDistanceType cameraDistanceType = Enums.CameraDistanceType.Neutral);
<<<<<<< Updated upstream
=======

        void SetEasingAndConfiner(Enums.CameraEasingType easingType, Collider2D confiner);

        void Shake();
>>>>>>> Stashed changes
    }
}