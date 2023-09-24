using Infrastructure.Utils;

namespace Common.Units.Interfaces
{
    public interface IUnitStatsData
    {
        public int GetStatValue(Enums.Stat type);
    }
}