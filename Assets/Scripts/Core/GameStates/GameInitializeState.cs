using System;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class GameInitializeState : IGameState
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        
        public GameInitializeState(IGameStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        
        public void Activate(GameStateArgs args)
        {
            if (Debugging.DebugData.ShowGameStateData)
                Debug.Log("Init!");
            
            _stateSwitcher.SwitchState<SceneSwitchState>(new SceneSwitchArgs(Enums.SceneType.Arena));
        }
    }
}