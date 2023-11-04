using System;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers;
using Common.UI;
using Common.UI.Gameplay;
using Common.Units.Selection;
using Core.Configs;
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
        [SerializeField] private ConfigsService _configsService;
        [SerializeField] private UI _ui;
        [SerializeField] private DebugHeroTemplate _debugHeroTemplate;
        [SerializeField] private SelectionHeroesDatabase _heroesDatabase;
        [SerializeField] private ModifiersDataBase _modifiersDataBase;
        
        public void Initialize() => _game.Initiate();

        public override void InstallBindings()
        {
            BindServices();
            BindSceneSwitcher();
            BindHeroes();
            BindModifiers();
            BindRunData();
            BindFactories();
            BindGameStates();
            BindUI();
            BindGame();
            BindSelf();
        }
        
        public void Dispose()
        {
            Container.Resolve<IInputService>().Dispose();
            
            _ui.Dispose();
            _game.Dispose();
        }
        
        private void BindSelf() => Container.BindInterfacesAndSelfTo<GameInstaller>().FromInstance(this).AsSingle();

        private void BindSceneSwitcher() => Container.Bind<SceneSwitcher>().AsSingle();

        private void BindRunData() => Container.Bind<IRunData>().To<RunData>().AsSingle();

        private void BindHeroes()
        {
            Container.Bind<SelectionHeroesDatabase>().FromInstance(_heroesDatabase).AsSingle();
        }
        
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

            Container.Bind<IConfigsService>().To<ConfigsService>().FromInstance(_configsService).AsSingle();

            Container.BindInterfacesTo<ServicesHandler>().AsSingle().CopyIntoDirectSubContainers();
        }
        
        private void BindUI()
        {
            _ui = Container.InstantiatePrefabForComponent<UI>(_ui);
            
            Container.Bind<UI>().FromInstance(_ui).AsSingle();
        }
        
        private void BindGame()
        {
            _game = Container.InstantiatePrefabForComponent<Game>(_game);

            Container.BindInterfacesTo<Game>().FromInstance(_game).AsSingle();
            Container.BindInterfacesTo<DebugHeroTemplate>().FromInstance(_debugHeroTemplate).AsSingle();
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
            Container.BindInterfacesAndSelfTo<MainMenuState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogueState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayState>().AsSingle();

            Container.Bind<IGameStatesFactory>().To<GameStatesFactory>().AsSingle();
        }
    }
}