using System;
using System.Collections.Generic;
using System.Threading;
using Common.UI.Interfaces;
using Common.Units.Selection;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.MainMenu.SelectionView
{
    public class UnitSelectionView : AnimatableUIElement, IHorizontallyNavigatableUIElement, ISelectableUIElement, IDisposable
    {
        [SerializeField] private SelectionCardsHandler _cardsHandler;
        [SerializeField] private SelectionStage _selectionStage;

        private IReadOnlyList<SelectionHeroTemplate> _templates;

        public event Action<SelectionHeroTemplate> UnitSelected;
        public event Action Selected;
        public event Action Backed;

        public void Dispose()
        {
            _selectionStage.Dispose();
        }
        
        public override void Activate()
        {
            base.Activate();
            
            _cardsHandler.HoverUpdated += OnCardHoverUpdated;
            _cardsHandler.Selected += OnCardSelected;
            
            _cardsHandler.Activate();
            _selectionStage.Activate();
        }

        public override void Deactivate()
        {
            _cardsHandler.Deactivate();
            _selectionStage.Deactivate();
            
            _cardsHandler.HoverUpdated -= OnCardHoverUpdated;
            _cardsHandler.Selected -= OnCardSelected;
            
            base.Deactivate();
        }
        
        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            UniTask task1 = _selectionStage.ActivateAsync(token);
            UniTask task2 = _cardsHandler.ActivateAsync(token);

            await UniTask.WhenAll(task1, task2);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            UniTask task1 = _selectionStage.DeactivateAsync(token);
            UniTask task2 = _cardsHandler.DeactivateAsync(token);

            await UniTask.WhenAll(task1, task2);
            
            Deactivate();
        }

        public void MoveLeft() => _cardsHandler.MoveLeft();

        public void MoveRight() => _cardsHandler.MoveRight();

        public void Select() => _cardsHandler.Select();

        public void Back() => Backed?.Invoke();

        public void SetData(IReadOnlyList<SelectionHeroTemplate> templates)
        {
            _templates = templates;
            
            _cardsHandler.SetData(templates);
        }

        private void PlayHeroAnimation(IReadOnlyList<Sprite> frames, bool isRepeating = true) => _selectionStage.PlayHeroAnimation(frames, isRepeating);
        
        private void OnCardHoverUpdated(int index)
        {
            index = index % 2 == 0 ? 0 : 1;
            
            PlayHeroAnimation(_templates[index].IdleAnimation);
        }
        
        private void OnCardSelected(int index)
        {
            index = index % 2 == 0 ? 0 : 1;
            
            PlayHeroAnimation(_templates[index].SelectedAnimation, false);
            
            UnitSelected?.Invoke(_templates[index]);
        }
    }
}