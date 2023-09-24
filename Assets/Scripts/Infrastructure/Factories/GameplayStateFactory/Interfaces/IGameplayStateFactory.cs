using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Zenject;

namespace Infrastructure.Factories.GameplayStateFactory.Interfaces
{
    public interface IGameplayStateFactory 
    {
        IReadOnlyList<IGameplayState> States { get; }

        void UpdateContainer(DiContainer container);
        void CreateAllStates();
    }
}