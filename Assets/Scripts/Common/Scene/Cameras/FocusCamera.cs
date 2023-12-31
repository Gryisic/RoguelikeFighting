﻿using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras
{
    public class FocusCamera : Camera
    {
        public void FocusOn(Vector2 positionToFocusOn, Enums.CameraDistanceType distanceType)
        {
            virtualCamera.m_Lens.OrthographicSize = DistanceToSize(distanceType);

            Vector3 finalPosition = positionToFocusOn;
            finalPosition.z = -10;
            
            transform.position = finalPosition;
        }

        protected override float DistanceToSize(Enums.CameraDistanceType distanceType)
        {
            float size = distanceType switch
            {
                Enums.CameraDistanceType.Far => Constants.FarFocusCameraSize,
                Enums.CameraDistanceType.Close => Constants.CloseFocusCameraSize,
                _ => Constants.NeutralFocusCameraSize
            };

            return size;
        }
    }
}