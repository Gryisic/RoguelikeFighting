using System;
using System.Collections.Generic;
using System.Threading;
using Common.Units.Selection;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Common.UI.MainMenu.SelectionView
{
    public class SelectionStage : AnimatableUIElement, IDisposable
    {
        [SerializeField] private SelectionHero _hero;

        public void Dispose()
        {
            _hero.Dispose();
        }

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();

            await DOTween.Sequence()
                .AppendInterval(1f)
                .AppendCallback(() => _hero.Activate())
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            _hero.Deactivate();
            
            Deactivate();

            await UniTask.WaitForFixedUpdate();
        }

        public void PlayHeroAnimation(IReadOnlyList<Sprite> frames, bool isRepeating = false) => _hero.PlayAnimation(frames, isRepeating);
    }
}