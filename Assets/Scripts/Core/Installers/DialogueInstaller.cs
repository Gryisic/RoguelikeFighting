using System;
using Common.Dialogues;
using Zenject;

namespace Core.Installers
{
    public class DialogueInstaller : MonoInstaller, IDisposable
    {
        public override void InstallBindings()
        {
            BindSelf();
            BindDialogue();
        }

        private void BindSelf() => Container.BindInterfacesAndSelfTo<DialogueInstaller>().FromInstance(this).AsSingle();
        
        private void BindDialogue() => Container.BindInterfacesAndSelfTo<Dialogue>().AsSingle();

        public void Dispose()
        {
            Container.Resolve<Dialogue>().Dispose();
        }
    }
}