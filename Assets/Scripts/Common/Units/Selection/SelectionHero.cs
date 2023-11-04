using System;
using System.Collections.Generic;
using System.Threading;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units.Selection
{
    public class SelectionHero : MonoBehaviour, IDisposable
    {
        [SerializeField] private SpriteRenderer _renderer;

        private CancellationTokenSource _animationTokenSource;
        
        public void Dispose()
        {
            _animationTokenSource?.Cancel();
            _animationTokenSource?.Dispose();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
            
            _animationTokenSource?.Cancel();
        }

        public void PlayAnimation(IReadOnlyList<Sprite> frames, bool isRepeating = false)
        {
            _animationTokenSource?.Cancel();
            _animationTokenSource = new CancellationTokenSource();
            
            PlayAnimationAsync(frames, isRepeating).Forget();
        }

        private async UniTask PlayAnimationAsync(IReadOnlyList<Sprite> frames, bool isRepeating)
        {
            if (isRepeating == false)
            {
                await PlayAnimationCycleAsync(frames, 0.085f);
                
                return;
            }
            
            while (_animationTokenSource.IsCancellationRequested == false) 
                await PlayAnimationCycleAsync(frames, 0.085f);
        }

        private async UniTask PlayAnimationCycleAsync(IReadOnlyList<Sprite> frames, float frameRate)
        {
            foreach (var sprite in frames)
            {
                _renderer.sprite = sprite;
                
                await UniTask.Delay(TimeSpan.FromSeconds(frameRate), cancellationToken: _animationTokenSource.Token);
            }
        }
    }
}