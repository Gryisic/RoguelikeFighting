using Common.Gameplay.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.Data
{
    public class GameplayData : IGameplayData
    {
        public Enums.MenuType MenuType { get; private set; }

        public void SetPauseType(Enums.MenuType menuType) => MenuType = menuType;
    }
}