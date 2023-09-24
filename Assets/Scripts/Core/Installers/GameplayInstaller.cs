using System;
using Common.Gameplay;
using Common.Gameplay.States;
using Common.Units;
using Infrastructure.Factories.UnitsFactory;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GameplayInstaller : MonoInstaller, IDisposable
    {
        [SerializeField] private Stage _stage;

        public override void InstallBindings()
        {
            BindSelf();
            BindFactories();
            BindPlayer();
            BindStage();
            BindStates();
        }

        public void Dispose()
        {
            Container.Resolve<UnitsHandler>().Dispose();
            Container.Resolve<Stage>().Dispose();
        }

        private void BindSelf() => Container.BindInterfacesAndSelfTo<GameplayInstaller>().FromInstance(this).AsSingle();

        private void BindPlayer() => Container.Bind<Player>().AsSingle();
        
        private void BindStage() => Container.Bind<Stage>().FromInstance(_stage).AsSingle();
        
        private void BindFactories()
        {
            Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle();
            Container.Bind<UnitsHandler>().AsSingle();
        }
        
        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<GameplayInitializeState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayActiveState>().AsSingle();
        }
    }
}