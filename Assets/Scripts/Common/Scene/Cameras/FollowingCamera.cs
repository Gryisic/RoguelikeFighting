using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras
{
    public class FollowingCamera : Camera
    {
        public void FollowUnit(Transform unitTransform, Enums.CameraDistanceType distanceType)
        {
            camera.m_Lens.OrthographicSize = DistanceToSize(distanceType);

            camera.Follow = unitTransform;
        }

        protected override float DistanceToSize(Enums.CameraDistanceType distanceType)
        {
            float size = distanceType switch
            {
                Enums.CameraDistanceType.Far => Constants.FarFollowCameraSize,
                Enums.CameraDistanceType.Close => Constants.CloseFollowCameraSize,
                _ => Constants.NeutralFollowCameraSize
            };

            return size;
        }
    }
}