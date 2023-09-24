using Common.Units.Interfaces;

namespace Common.Gameplay.Interfaces
{
    public interface IRunData
    {
        ISharedUnitData SharedHeroData { get; }
    }
}