using System;
using Common.Units;
using UnityEngine;

namespace Common.Gameplay.Interfaces
{
    public interface IStageData
    {
        event Action<Vector2> HeroPositionChangeRequested;

        UnitsHandler UnitsHandler { get; }
    }
}