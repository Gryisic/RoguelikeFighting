using Common.Gameplay.Data;
using Common.Units.Heroes;
using Common.Units.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.Interfaces
{
    public interface IRunData
    {
        HeroTemplate HeroTemplate { get; }
        ISharedUnitData SharedHeroData { get; }
        
        int VisitedRoomsAmount { get; }

        T Get<T>(Enums.RunDataType type) where T : IConcreteRunData;

        void SetHeroTemplate(HeroTemplate heroTemplate);
        
        void IncreaseVisitedRoomsAmount();
        
        void Clear();
    }
}