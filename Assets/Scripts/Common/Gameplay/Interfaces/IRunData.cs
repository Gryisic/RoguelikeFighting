using Common.Gameplay.Data;
using Common.Units.Interfaces;

namespace Common.Gameplay.Interfaces
{
    public interface IRunData
    {
        ISharedUnitData SharedHeroData { get; }
        ModifiersData ModifiersData { get; }
    }
}