using Common.Gameplay.Modifiers;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class GameInitializeState : IGameState
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly ModifiersDataBase _modifiersDataBase;
        
        public GameInitializeState(IGameStateSwitcher stateSwitcher, ModifiersDataBase modifiersDataBase)
        {
            _stateSwitcher = stateSwitcher;
            _modifiersDataBase = modifiersDataBase;
        }
        
        public void Activate(GameStateArgs args)
        {
            _modifiersDataBase.Initialize();
            
            _stateSwitcher.SwitchState<SceneSwitchState>(new SceneSwitchArgs(Enums.SceneType.Arena, Enums.GameStateType.Gameplay));
        }
    }
}