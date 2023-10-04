using Infrastructure.Utils;

namespace Common.Gameplay.Interfaces
{
    public interface IGameplayData
    {
        Enums.PauseType PauseType { get; }

        void SetPauseType(Enums.PauseType pauseType);
    }
}