﻿using System;
using Common.Gameplay;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers;
using Common.Gameplay.States;
using Core.GameStates;
using Core.Interfaces;
using Core.PlayerInput;
using Core.Utils;
using Infrastructure.Factories.GameplayStateFactory;
using Infrastructure.Factories.GameplayStateFactory.Interfaces;
using Infrastructure.Factories.GameStatesFactory;
using Infrastructure.Factories.GameStatesFactory.Interfaces;
using UnityEngine;
using Zenject;
using Input = Core.PlayerInput.Input;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller, IInitializable, IDisposable
    {
        [SerializeField] private Game _game;
        [SerializeField] private DebugHeroTemplate _debugHeroTemplate;
        [SerializeField] private ModifiersDataBase _modifiersDataBase;
        
        public void Initialize() => _game.Initiate();

        public override void InstallBindings()
        {
            BindServices();
            BindSceneSwitcher();
            BindModifiers();
            BindRunData();
            BindFactories();
            BindGameStates();
            BindGame();
            BindSelf();
        }

        public void Dispose()
        {
            Container.Resolve<IInputService>().Dispose();
            
            _game.Dispose();
        }
        
        private void BindSelf() => Container.BindInterfacesAndSelfTo<GameInstaller>().FromInstance(this).AsSingle();

        private void BindSceneSwitcher() => Container.Bind<SceneSwitcher>().AsSingle();

        private void BindRunData() => Container.Bind<IRunData>().To<RunData>().AsSingle();

        private void BindModifiers()
        {
            Container.Bind<ModifiersDataBase>().FromInstance(_modifiersDataBase).AsSingle();
            Container.Bind<ModifiersHandler>().AsSingle();
            Container.Bind<ModifiersResolver>().AsSingle();
            Container.Bind<ModifiersData>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<Input>().WhenInjectedInto<InputService>();
            Container.BindInterfacesTo<InputService>().AsSingle();

            Container.BindInterfacesTo<ServicesHandler>().AsSingle().CopyIntoDirectSubContainers();
        }
        
        private void BindGame()
        {
            _game = Container.InstantiatePrefabForComponent<Game>(_game);

            Container.BindInterfacesTo<Game>().FromInstance(_game).AsSingle();
            Container.Bind<IInitialHeroData>().To<DebugHeroTemplate>().FromInstance(_debugHeroTemplate).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IGameplayStateFactory>().To<GameplayStatesFactory>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameplayStatesFactoryUpdater>().AsSingle().CopyIntoDirectSubContainers();
        }
        
        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<GameInitializeState>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneSwitchState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogueState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();

            Container.Bind<IGameStatesFactory>().To<GameStatesFactory>().AsSingle();
        }
    }
}