using System;
using Infrastructure.Utils;

namespace Common.Units.Interfaces
{
    public interface IUnitStatsData
    {
        event Action<int, int> HealthChanged; 

        public float GetStatValue(Enums.Stat type);
        public int GetStatValueAsInt(Enums.Stat type);

        public void IncreaseStat(Enums.Stat type, float amount);
        public void DecreaseStat(Enums.Stat type, float amount);
    }
}