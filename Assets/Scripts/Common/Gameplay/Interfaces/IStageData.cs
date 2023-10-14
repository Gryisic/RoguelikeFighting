using System;
using System.Collections.Generic;
using Common.Gameplay.Rooms;
using Common.Units;
using UnityEngine;

namespace Common.Gameplay.Interfaces
{
    public interface IStageData
    {
        event Action<Vector2> HeroPositionChangeRequested;

        UnitsHandler UnitsHandler { get; }
        IReadOnlyList<Room> Rooms { get; }
    }
}