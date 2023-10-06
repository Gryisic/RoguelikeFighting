using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public abstract class Room : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _heroInitialPosition;
        [SerializeField] private RoomChangeTrigger _changeTrigger;

        public RoomChangeTrigger ChangeTrigger => _changeTrigger;
        public Transform HeroInitialPosition => _heroInitialPosition;
        
        public abstract Enums.RoomType Type { get; }

        public abstract void Initialize(IStageData stageData, IRunData runData);
        public abstract void Dispose();

        public virtual void Enter() => gameObject.SetActive(true);

        public virtual void Exit() => gameObject.SetActive(false);
    }
}