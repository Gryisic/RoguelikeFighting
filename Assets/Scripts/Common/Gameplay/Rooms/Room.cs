using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Models.Animators;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public abstract class Room : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _heroInitialPosition;
        [SerializeField] private RoomChangeTrigger _changeTrigger;

        [SerializeField] protected RoomAnimator animator;

        public RoomChangeTrigger ChangeTrigger => _changeTrigger;
        public Transform HeroInitialPosition => _heroInitialPosition;
        
        public abstract Enums.RoomType Type { get; }

        public abstract void Initialize(IStageData stageData, IRunData runData);
        public abstract void Dispose();

        public virtual void Enter()
        {
            gameObject.SetActive(true);
            
            animator.Activate();
        }

        public virtual void Exit()
        {
            animator.Deactivate();
            
            gameObject.SetActive(false);
        }
    }
}