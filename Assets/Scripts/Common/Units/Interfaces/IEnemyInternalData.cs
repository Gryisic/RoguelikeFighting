using System.Collections.Generic;
using Common.Models.Actions;
using Common.Units.Enemies;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IEnemyInternalData : IUnitInternalData
    {
        ISharedUnitData HeroData { get; }
        EnemyTemplate Data { get; }
        IReadOnlyList<EnemyAction> Actions { get; }
        Transform Transform { get; }
    }
}