using Common.Models.StatusEffects.Interfaces;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface ISharedUnitData 
    {
        Transform Transform { get; }
        IUnitStatsData StatsData { get; }
        IStatusEffectsHandler EffectsHandler { get; }
    }
}