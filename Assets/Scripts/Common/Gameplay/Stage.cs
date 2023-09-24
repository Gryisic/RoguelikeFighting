using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Rooms;
using Common.Units;
using UnityEngine;
using Zenject;

namespace Common.Gameplay
{
    public class Stage : MonoBehaviour, IStageData, IDisposable
    {
        [SerializeField] private Room[] _rooms;

        public UnitsHandler UnitsHandler { get; private set; }

        [Inject]
        private void Construct(UnitsHandler unitsHandler)
        {
            UnitsHandler = unitsHandler;

            foreach (var room in _rooms)
            {
                room.Initialize(this);
            }
        }
        
        public void Dispose()
        {
            foreach (var room in _rooms)
            {
                room.Dispose();
            }
        }
    }
}