using Cinemachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Scene.Cameras
{
    public abstract class Camera : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera camera;
<<<<<<< Updated upstream
=======
        [SerializeField] protected CinemachineImpulseListener impulseListener;
        [SerializeField] private CinemachineConfiner2D confiner2D;
>>>>>>> Stashed changes

        public void Activate()
        {
            camera.Priority = Constants.ActivatedCameraPriority;

            impulseListener.enabled = true;
        }

        public void Deactivate()
        {
            impulseListener.enabled = false;
            
            camera.Follow = null;
            camera.Priority = Constants.DeactivatedCameraPriority;
        }
        
        protected abstract float DistanceToSize(Enums.CameraDistanceType distanceType);
    }
}