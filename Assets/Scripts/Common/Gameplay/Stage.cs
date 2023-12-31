﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Rooms;
using Common.Scene.Cameras.Interfaces;
using Common.Units;
using Core.Configs.Interfaces;
using Core.Extensions;
using Core.Interfaces;
using Core.Utils;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Common.Gameplay
{
    public class Stage : MonoBehaviour, IStageData, IDisposable
    {
        [SerializeField] private Room[] _rooms;

        private IRunData _runData;
        private IServicesHandler _servicesHandler;
        private Room _currentRoom;

        public event Action<Vector2> HeroPositionChangeRequested; 

        public UnitsHandler UnitsHandler { get; private set; }
        public IReadOnlyList<Room> Rooms => _rooms;

        [Inject]
        private void Construct(IRunData runData, IServicesHandler servicesHandler, UnitsHandler unitsHandler)
        {
            _runData = runData;
            _servicesHandler = servicesHandler;
            UnitsHandler = unitsHandler;
        }

        public void Initialize()
        {
            InitializeRooms();
            
            _currentRoom = _rooms[0]; 
            _currentRoom.Enter();
        }

        public void Dispose()
        {
            foreach (var room in _rooms)
            {
                room.ChangeTrigger.Triggered -= ChangeRoom;
                
                room.Dispose();
            }
        }
        
        private void InitializeRooms()
        {
            foreach (var room in _rooms)
            {
                room.ChangeTrigger.Triggered += ChangeRoom;
                
                room.Initialize(this, _runData, _servicesHandler.GetSubService<ICameraService>());

                if (room is BattleRoom battleRoom)
                    battleRoom.SetDifficultyData(_servicesHandler.ConfigsService.GetConfig<IDifficultyConfig>());
                
                if (room.gameObject.activeSelf)
                    room.gameObject.SetActive(false);
            }
        }
        
        private void ChangeRoom(Enums.RoomType roomType)
        {
            List<Room> possibleRooms = _rooms.Where(r => r.Type == roomType).ToList();
            Room nextRoom = possibleRooms.Random();
            
            _currentRoom.Exit();
            HeroPositionChangeRequested?.Invoke(nextRoom.HeroInitialPosition.position);
            nextRoom.Enter();
            
            _currentRoom = nextRoom;
        }
    }
}