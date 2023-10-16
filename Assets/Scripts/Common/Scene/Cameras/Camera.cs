using Cinemachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras
{
    public abstract class Camera : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera camera;
        [SerializeField] private CinemachineConfiner2D confiner2D;

        public void Activate() => camera.Priority = Constants.ActivatedCameraPriority;
        
        public void Deactivate()
        {
            camera.Follow = null;
            camera.Priority = Constants.DeactivatedCameraPriority;
        }
        
        public void SetConfiner(Collider2D confiner) => confiner2D.m_BoundingShape2D = confiner;

        protected abstract float DistanceToSize(Enums.CameraDistanceType distanceType);
    }
}