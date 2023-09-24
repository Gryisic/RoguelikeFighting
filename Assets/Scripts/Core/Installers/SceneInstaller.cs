using Common.Gameplay;
using Common.Scene;
using Common.Scene.Cameras;
using Common.Scene.Cameras.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneInfo _sceneInfo;
        [SerializeField] private CameraService _cameraService;
        
        public override void InstallBindings()
        {
            BindSceneInfo();
            BindCameraService();
        }

        private void BindSceneInfo() => Container.Bind<SceneInfo>().FromInstance(_sceneInfo).AsSingle();

        private void BindCameraService() => Container.Bind<ICameraService>().FromInstance(_cameraService).AsSingle();
        
    }
}