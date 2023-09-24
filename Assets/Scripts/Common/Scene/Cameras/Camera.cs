using Cinemachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras
{
    public abstract class Camera : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera camera;

        public void Activate() => camera.Priority = Constants.ActivatedCameraPriority;
        
        public void Deactivate()
        {
            camera.Follow = null;
            camera.Priority = Constants.DeactivatedCameraPriority;
        }
        
        protected abstract float DistanceToSize(Enums.CameraDistanceType distanceType);
    }
}