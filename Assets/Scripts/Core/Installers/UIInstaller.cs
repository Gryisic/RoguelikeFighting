using Common.UI;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UI _ui;
        
        public override void InstallBindings()
        {
            BindUI();
        }

        private void BindUI() => Container.Bind<UI>().FromInstance(_ui).AsSingle();
    }
}