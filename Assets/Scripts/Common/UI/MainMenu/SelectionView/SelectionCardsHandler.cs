using System;
using System.Collections.Generic;
using System.Threading;
using Common.Units.Selection;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Common.UI.MainMenu.SelectionView
{
    public class SelectionCardsHandler : AnimatableUIElement
    {
        [SerializeField] private SelectionCard[] _cards;

        [SerializeField] private Vector3 _offScreenPosition;
        [SerializeField] private int _centralCardIndex;
        
        private int _hoveredCardIndex;
        private int _maxIndex;

        public event Action<int> HoverUpdated; 
        public event Action<int> Selected;

        public override void Deactivate()
        {
            base.Deactivate();
            
            foreach (var card in _cards)
            {
                card.Deactivate();
                card.ResetPosition();
            }
        }
        
        public override async UniTask ActivateAsync(CancellationToken token)
        {
            _maxIndex = _cards.Length - 1;
            _hoveredCardIndex = _centralCardIndex;

            Vector3 leftBorder = _cards[0].Transform.localPosition;
            Vector3 rightBorder = _cards[_cards.Length - 1].Transform.localPosition;
            
            foreach (var card in _cards)
            {
                card.SetBorders(leftBorder, rightBorder);
                card.Activate();
            }

            await DOTween.Sequence()
                .Append(Transform.DOMove(Vector3.zero, 1f).From(_offScreenPosition))
                .ToUniTask(cancellationToken: token);

            UpdateHover(_hoveredCardIndex - 1, _hoveredCardIndex);
            Activate();
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            _cards[_hoveredCardIndex].UnHover();
            
            await DOTween.Sequence()
                    .Append(Transform.DOMove(_offScreenPosition, 1f))
                    .ToUniTask(cancellationToken: token);

            Deactivate();
        }

        public void MoveLeft()
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = _hoveredCardIndex - 1 < 0 ? _maxIndex : _hoveredCardIndex - 1;
            
            foreach (var card in _cards) 
                card.MoveRight();

            UpdateHover(currentIndex, _hoveredCardIndex);
        }

        public void MoveRight()
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = _hoveredCardIndex + 1 > _maxIndex ? 0 : _hoveredCardIndex + 1;

            foreach (var card in _cards) 
                card.MoveLeft();
            
            UpdateHover(currentIndex, _hoveredCardIndex);
        }
        
        public void Select() => Selected?.Invoke(_hoveredCardIndex);

        public void SetData(IReadOnlyList<SelectionHeroTemplate> templates)
        {
            int templateIndex = 0;
            int templatesCount = templates.Count;
            
            foreach (var card in _cards)
            {
                if (templateIndex >= templatesCount)
                    templateIndex = 0;
                
                card.SetData(templates[templateIndex]);

                templateIndex++;
            }
        }
        
        private void UpdateHover(int currentIndex, int nextIndex)
        {
            _cards[currentIndex].UnHover();
            _cards[nextIndex].Hover();
            
            HoverUpdated?.Invoke(nextIndex);
        }
    }
}