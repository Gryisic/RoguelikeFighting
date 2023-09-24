using System;
using Common.Gameplay.Interfaces;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public abstract class Room : MonoBehaviour, IDisposable
    {
        public abstract void Initialize(IStageData stageData);
        public abstract void Dispose();
    }
}