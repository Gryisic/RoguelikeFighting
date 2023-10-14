using Infrastructure.Utils;

namespace Common.Gameplay.Interfaces
{
    public interface IGameplayData
    {
        Enums.MenuType MenuType { get; }

        void SetPauseType(Enums.MenuType menuType);
    }
}