using Infrastructure.Utils;

namespace Common.Units.Interfaces
{
    public interface IUnitStatsData
    {
        public int GetStatValue(Enums.Stat type);

        public void IncreaseStat(Enums.Stat type, int amount);
        public void DecreaseStat(Enums.Stat type, int amount);
    }
}