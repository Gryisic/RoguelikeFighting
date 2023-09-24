using Common.Units.Stats.Interfaces;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface ISharedUnitData 
    {
        Transform Transform { get; }
        IUnitStatsData StatsData { get; }
    }
}