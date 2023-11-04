using System;
using Common.UI;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class MainMenuInstaller : MonoInstaller, IInitializable, IDisposable
    {
        [SerializeField] private UILayer _menuUILayer;
        
        public override void InstallBindings()
        {
            BindSelf();
        }
        
        public void Initialize()
        {
            UI ui = Container.Resolve<UI>();
            
            ui.AddSceneUILayer(_menuUILayer);
        }
        
        public void Dispose()
        {
            // UI ui = Container.Resolve<UI>();
            //
            // ui.RemoveSceneUILayer(_menuUILayer);
            // _menuUILayer.Dispose();
        }
        
        private void BindSelf() => Container.BindInterfacesAndSelfTo<MainMenuInstaller>().FromInstance(this).AsSingle();
    }
}