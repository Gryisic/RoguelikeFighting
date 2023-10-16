using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Models.Animators;
using Common.Scene.Cameras.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Gameplay.Rooms
{
    public abstract class Room : MonoBehaviour, IDisposable
    {
        [Header("Room Data")]
        [SerializeField] private Transform _heroInitialPosition;
        [SerializeField] private RoomChangeTrigger _changeTrigger;

        [Space, Header("Animation Data")]
        [SerializeField] protected RoomAnimator animator;
        
        [FormerlySerializedAs("cameraConfiner")]
        [Space, Header("Additional Data")]
        [SerializeField] private Collider2D _cameraConfiner;
        [FormerlySerializedAs("cameraEasing")] [SerializeField] private Enums.CameraEasingType _cameraEasing;
        
        protected abstract ICameraService CameraService { get; set; }

        public RoomChangeTrigger ChangeTrigger => _changeTrigger;
        public Transform HeroInitialPosition => _heroInitialPosition;
        
        public abstract Enums.RoomType Type { get; }

        public abstract void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService);
        public abstract void Dispose();

        public virtual void Enter()
        {
            gameObject.SetActive(true);
            ChangeTrigger.Deactivate();
            CameraService.SetEasingAndConfiner(_cameraEasing, _cameraConfiner);

            animator.Activate();
        }

        public virtual void Exit()
        {
            animator.Deactivate();
            
            gameObject.SetActive(false);
        }
    }
}