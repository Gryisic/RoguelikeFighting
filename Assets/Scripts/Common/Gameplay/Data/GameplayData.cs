using Common.Gameplay.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.Data
{
    public class GameplayData : IGameplayData
    {
        public Enums.PauseType PauseType { get; private set; }

        public void SetPauseType(Enums.PauseType pauseType) => PauseType = pauseType;
    }
}