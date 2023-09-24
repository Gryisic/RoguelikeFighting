using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Infrastructure.Factories.GameplayStateFactory.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories.GameplayStateFactory
{
    public class GameplayStatesFactory : IGameplayStateFactory
    {
        private DiContainer _container;
        
        public IReadOnlyList<IGameplayState> States { get; private set; }

        public GameplayStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public void UpdateContainer(DiContainer container) => _container = container;

        public void CreateAllStates() => States = _container.ResolveAll<IGameplayState>();
    }
}