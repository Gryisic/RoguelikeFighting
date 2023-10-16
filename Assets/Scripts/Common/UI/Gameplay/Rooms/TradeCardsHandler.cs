using System;
using System.Collections.Generic;
using Common.Models.Items;
using Common.UI.Interfaces;
using UnityEngine;

namespace Common.UI.Gameplay.Rooms
{
    public class TradeCardsHandler : UIElement, ISelectableUIElement, IHorizontalNavigatableUIElement
    {
        [SerializeField] private TradeCard[] _cards;

        private int _hoveredCardIndex;
        private int _maxIndex;
        
        public event Action Entered;
        public event Action Exited;
        public event Action<int> CardSelected;

        public override void Activate()
        {
            _hoveredCardIndex = 0;
            
            foreach (var card in _cards)
            {
                card.CardSelected += SelectCard;
                card.Hovered += HoveredViaPointer;
            }
            
            _cards[_hoveredCardIndex].Hover();

            base.Activate();
        }

        public override void Deactivate()
        {
            foreach (var card in _cards)
            {
                card.CardSelected -= SelectCard;
                
                card.Deactivate();
            }
        }
        
        public void Select() => _cards[_hoveredCardIndex].Select();

        public void Undo() => Exited?.Invoke();
        
        public void MoveRight()
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = _hoveredCardIndex + 1 >= _maxIndex ? _maxIndex : _hoveredCardIndex + 1;
            
            UpdateHover(currentIndex, _hoveredCardIndex);
        }

        public void MoveLeft()
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = _hoveredCardIndex - 1 <= 0 ? 0 : _hoveredCardIndex - 1;
            
            UpdateHover(currentIndex, _hoveredCardIndex);
        }

        public void SetCardsData(IReadOnlyList<TradeItemData> itemsData)
        {
            _maxIndex = itemsData.Count - 1;
            
            for (var i = 0; i < itemsData.Count; i++)
            {
                TradeCard card = _cards[i];
                
                card.SetData(i, itemsData[i]);
                card.Activate();
            }
        }
        
        private void SelectCard(int index) => CardSelected?.Invoke(index);
        
        private void HoveredViaPointer(int index)
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = index;
            
            UpdateHover(currentIndex, _hoveredCardIndex);
        }

        private void UpdateHover(int currentIndex, int nextIndex)
        {
            _cards[currentIndex].UnHover();
            _cards[nextIndex].Hover();
        }
    }
}