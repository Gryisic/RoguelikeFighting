﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Scene.Cameras.Interfaces;
using Common.UI.Gameplay.Rooms;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Common.Gameplay.Rooms
{
    public class SelectionRoom : Room
    {
        [SerializeField] private RoomSelectionTrigger[] _selectionTriggers;
        [SerializeField] private RoomTemplate[] _generalTemplates;
        [SerializeField] private TriggersLayout _layout;

        [Space, Header("Room View")]
        [SerializeField] private SelectionRoomView _roomView;
        
        [Space, Header("Additional Data")]
        [SerializeField] protected Transform _cameraFocusPoint;

        private IReadOnlyList<Enums.RoomType> _roomTypes;

        protected override ICameraService CameraService { get; set; }
        public override Enums.RoomType Type => Enums.RoomType.Selection;

        public override void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService)
        {
            if (ChangeTrigger is SelectionRoomTrigger == false)
                throw new Exception("Selection Room doesn't have 'Selection Room Trigger'");
            
            List<Enums.RoomType> types = Enum.GetValues(typeof(Enums.RoomType))
                .Cast<Enums.RoomType>()
                .Where(t => t != Enums.RoomType.Selection && t != Enums.RoomType.ExtensiveBattle)
                .ToList();

            _roomTypes = types;
            
            CameraService = cameraService;

            foreach (var trigger in _selectionTriggers)
            {
                trigger.Triggered += OnRoomSelected;
                trigger.HeroEntered += OnHeroEnteredTrigger;
                trigger.HeroExited += OnHeroExitedTrigger;
            }
        }

        public override void Dispose()
        {
            foreach (var trigger in _selectionTriggers)
            {
                trigger.Triggered -= OnRoomSelected;
                trigger.HeroEntered -= OnHeroEnteredTrigger;
                trigger.HeroExited -= OnHeroExitedTrigger;
            }
            
            base.Dispose();
        }

        public override void Enter()
        {
            SetTriggers();
            
            _roomView.Activate();
            ChangeTrigger.Deactivate();
            CameraService.FocusOn(_cameraFocusPoint);

            base.Enter();
        }

        public override void Exit()
        {
            _roomView.Deactivate();
            
            base.Exit();
        }

        private void SetTriggers()
        {
            int triggersAmount = Random.Range(1, _selectionTriggers.Length + 1);
            HashSet<Enums.RoomType> usedTypes = new HashSet<Enums.RoomType>();

            if (triggersAmount > _roomTypes.Count)
                triggersAmount = _roomTypes.Count;
            
            _layout.SetTriggersAmount(triggersAmount);

            for (int i = 0; i < triggersAmount; i++)
            {
                Enums.RoomType type = GetRandomType(usedTypes);
                RoomTemplate template = _generalTemplates.First(t => t.Type == type);
                Vector2 position = _layout.GetNextTriggerPosition();

                _roomView.SelectionMarkersHandler.SetDataAndActivate(i, template);
                _selectionTriggers[i].SetPosition(position);
                _selectionTriggers[i].SetIndexAndType(i, type);
                _selectionTriggers[i].Activate();
            }
            
            _layout.Reset();
        }

        private Enums.RoomType GetRandomType(HashSet<Enums.RoomType> usedTypes)
        {
            while (true)
            {
                Enums.RoomType type = _roomTypes.Random();

                if (usedTypes.Add(type) == false) 
                    continue;
                
                return type;
            }
        }

        private void OnRoomSelected(Enums.RoomType type)
        {
            SelectionRoomTrigger changeTrigger = ChangeTrigger as SelectionRoomTrigger;
            
            changeTrigger.UpdateType(type);
            changeTrigger.Activate();
            
            foreach (var trigger in _selectionTriggers) 
                trigger.Deactivate();
            
            CameraService.Shake();
            animator.PlayNext();
            _roomView.Deactivate();
            
            ActivateExitParticles();
        }
        
        private void OnHeroEnteredTrigger(int index) => _roomView.SelectionMarkersHandler.Expand(index);

        private void OnHeroExitedTrigger(int index) => _roomView.SelectionMarkersHandler.Fold(index);

        [Serializable]
        private class TriggersLayout
        {
            [SerializeField] private AmountPositionMap[] _amountPositionMaps;
            
            private int _index;
            private Dictionary<int, Vector2> _indexPositionMap = new Dictionary<int, Vector2>();

            public void SetTriggersAmount(int amount)
            {
                AmountPositionMap positionMap = _amountPositionMaps[amount - 1];
                
                _indexPositionMap.Add(0, positionMap.FirstPosition);
                _indexPositionMap.Add(1, positionMap.SecondPosition);
                _indexPositionMap.Add(2, positionMap.ThirdPosition);
            }

            public void Reset()
            {
                _index = 0;
                
                _indexPositionMap.Clear();
            }

            public Vector2 GetNextTriggerPosition()
            {
                Vector2 position = _indexPositionMap[_index];
                
                _index++;

                return position;
            }
            
            [Serializable]
            private struct AmountPositionMap
            {
                [SerializeField] private Vector2 _firstPosition;
                [SerializeField] private Vector2 _secondPosition;
                [SerializeField] private Vector2 _thirdPosition;

                public Vector2 FirstPosition => _firstPosition;
                public Vector2 SecondPosition => _secondPosition;
                public Vector2 ThirdPosition => _thirdPosition;
            }
        }
    }
}