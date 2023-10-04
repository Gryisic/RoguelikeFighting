using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Rooms;
using Common.Units;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;
using Zenject;

namespace Common.Gameplay
{
    public class Stage : MonoBehaviour, IStageData, IDisposable
    {
        [SerializeField] private Room[] _roomPrefabs;

        private Room _currentRoom;

        public event Action<Vector2> HeroPositionChangeRequested; 

        public UnitsHandler UnitsHandler { get; private set; }

        [Inject]
        private void Construct(UnitsHandler unitsHandler)
        {
            UnitsHandler = unitsHandler;

            InitializeRooms();
            
            _currentRoom = _roomPrefabs[0]; 
            _currentRoom.Enter();
        }

        public void Dispose()
        {
            foreach (var room in _roomPrefabs)
            {
                room.ChangeTrigger.Triggered -= ChangeRoom;
                room.Dispose();
            }
        }
        
        private void InitializeRooms()
        {
            foreach (var room in _roomPrefabs)
            {
                room.ChangeTrigger.Triggered += ChangeRoom;
                room.Initialize(this);

                if (room.gameObject.activeSelf)
                    room.gameObject.SetActive(false);
            }
        }
        
        private void ChangeRoom(Enums.RoomType roomType)
        {
            List<Room> possibleRooms = _roomPrefabs.Where(r => r.Type == roomType).ToList();
            Room nextRoom = possibleRooms.Random();
            
            _currentRoom.Exit();
            HeroPositionChangeRequested?.Invoke(nextRoom.HeroInitialPosition.position);
            nextRoom.Enter();
            
            _currentRoom = nextRoom;
        }
    }
}