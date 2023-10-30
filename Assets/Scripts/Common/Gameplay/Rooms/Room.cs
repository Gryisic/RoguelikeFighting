using System;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Models.Animators;
using Common.Scene.Cameras.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public abstract class Room : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _heroInitialPosition;
        [SerializeField] private RoomChangeTrigger _changeTrigger;
        [SerializeField] private ParticleSystem _enterParticle;
        [SerializeField] private ParticleSystem _exitParticle;

        [SerializeField] protected RoomAnimator animator;
        
        [Space, Header("Additional Data")]
        [SerializeField] private Collider2D _cameraConfiner;
        [SerializeField] private Enums.CameraEasingType _cameraEasing;
        
        protected abstract ICameraService CameraService { get; set; }
        
        private CancellationTokenSource _particleTokenSource;

        public RoomChangeTrigger ChangeTrigger => _changeTrigger;
        public Transform HeroInitialPosition => _heroInitialPosition;
        
        public abstract Enums.RoomType Type { get; }

        public abstract void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService);

        public virtual void Dispose()
        {
            _particleTokenSource?.Cancel();
            _particleTokenSource?.Dispose();
        }

        public virtual void Enter()
        {
            _particleTokenSource = new CancellationTokenSource();
            
            gameObject.SetActive(true);
            
            animator.Activate();
            CameraService.SetEasingAndConfiner(_cameraEasing, _cameraConfiner);

            DeactivateEnterParticleAsync().Forget();
        }

        public virtual void Exit()
        {
            animator.Deactivate();
            animator.Reset();
            
            _exitParticle.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        protected void ActivateExitParticles()
        {
            _exitParticle.gameObject.SetActive(true);
            _enterParticle.Play();
        }

        private async UniTask DeactivateEnterParticleAsync()
        {
            _enterParticle.gameObject.SetActive(true);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_enterParticle.main.duration), cancellationToken: _particleTokenSource.Token);
            
            _enterParticle.gameObject.SetActive(false);
        }
    }
}