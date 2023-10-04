using Common.Gameplay;
using Common.Scene;
using Common.Scene.Cameras;
using Common.Scene.Cameras.Interfaces;
using Core.Interfaces;
using Core.Utils;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class SceneInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private SceneInfo _sceneInfo;
        [SerializeField] private CameraService _cameraService;
        
        public override void InstallBindings()
        {
            BindSelf();
            BindSceneInfo();
        }

        private void BindSelf() => Container.BindInterfacesAndSelfTo<SceneInstaller>().FromInstance(this).AsSingle();

        private void BindSceneInfo() => Container.Bind<SceneInfo>().FromInstance(_sceneInfo).AsSingle();

        public void Initialize()
        {
            ServicesHandler handler = (ServicesHandler) Container.Resolve<IServicesHandler>();
            
            handler.RegisterService(_cameraService);
        }
    }
}