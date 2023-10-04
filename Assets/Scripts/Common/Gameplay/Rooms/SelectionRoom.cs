using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Gameplay.Rooms
{
    public class SelectionRoom : Room
    {
        [SerializeField] private RoomSelectionTrigger[] _selectionTriggers;

        private IReadOnlyList<Enums.RoomType> _roomTypes;
        
        public override Enums.RoomType Type => Enums.RoomType.Selection;

        public override void Initialize(IStageData stageData)
        {
            if (ChangeTrigger is SelectionRoomTrigger == false)
                throw new Exception("Selection Room doesn't have 'Selection Room Trigger'");
            
            List<Enums.RoomType> types = Enum.GetValues(typeof(Enums.RoomType))
                .Cast<Enums.RoomType>()
                .Where(t => t != Enums.RoomType.Selection)
                .ToList();

            _roomTypes = types;

            foreach (var trigger in _selectionTriggers) 
                trigger.Triggered += OnRoomSelected;
        }

        public override void Dispose()
        {
            foreach (var trigger in _selectionTriggers) 
                trigger.Triggered -= OnRoomSelected;
        }

        public override void Enter()
        {
            SetTriggers();
            
            ChangeTrigger.Deactivate();

            base.Enter();
        }

        private void SetTriggers()
        {
            int triggersCount = Random.Range(1, _selectionTriggers.Length);

            for (int i = 0; i < triggersCount; i++)
            {
                Enums.RoomType type = _roomTypes.Random();

                _selectionTriggers[i].SetTypeAndIndex(type);
                _selectionTriggers[i].Activate();
            }
        }

        private void OnRoomSelected(Enums.RoomType type)
        {
            SelectionRoomTrigger changeTrigger = ChangeTrigger as SelectionRoomTrigger;
            changeTrigger.UpdateType(type);
            changeTrigger.Activate();
            
            foreach (var trigger in _selectionTriggers) 
                trigger.Deactivate();
        }
    }
}