using System;
using Infrastructure.Factories.GameplayStateFactory.Interfaces;
using Zenject;

namespace Infrastructure.Factories.GameplayStateFactory
{
    public class GameplayStatesFactoryUpdater : IInitializable, IDisposable
    {
        private readonly IGameplayStateFactory _factory;

        private DiContainer _container;
        
        protected GameplayStatesFactoryUpdater([Inject(Source = InjectSources.Local)]DiContainer container, IGameplayStateFactory factory)
        {
            _factory = factory;
            _container = container;
        }

        public void Initialize() => _factory.UpdateContainer(_container);

        public void Dispose() => _container = null;
    }
}