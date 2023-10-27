using Common.Units.Heroes;
using Common.Units.Legacy;

namespace Core.Interfaces
{
    public interface IInitialHeroData
    {
        HeroTemplate HeroTemplate { get; }
    }

    public interface IInitialLegacyUnitData
    {
        LegacyUnitTemplate FirstLegacyUnitTemplate { get; }
        LegacyUnitTemplate SecondLegacyUnitTemplate { get; }
    }
}