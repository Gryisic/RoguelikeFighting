using Common.Models.Actions;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IEnemyInternalData : IUnitInternalData
    {
        ISharedUnitData HeroData { get; }
        EnemyAction Action { get; }
        Transform Transform { get; }
    }
}