using Common.Gameplay.Data;
using Common.Units.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.Interfaces
{
    public interface IRunData
    {
        ISharedUnitData SharedHeroData { get; }

        T Get<T>(Enums.RunDataType type) where T : IConcreteRunData;
        
        void Clear();
    }
}