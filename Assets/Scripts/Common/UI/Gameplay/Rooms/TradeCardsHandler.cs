using System;
using System.Collections.Generic;
using Common.Models.Items;
using UnityEngine;

namespace Common.UI.Gameplay.Rooms
{
    public class TradeCardsHandler : UIElement
    {
        [SerializeField] private TradeCard[] _cards;

<<<<<<< Updated upstream
=======
        private int _hoveredCardIndex;
        private int _maxIndex;

        public event Func<int> RequestGaldAmount; 
        public event Action Entered;
        public event Action Exited;
>>>>>>> Stashed changes
        public event Action<int> CardSelected;

        public override void Activate()
        {
            foreach (var card in _cards) 
                card.CardSelected += SelectCard;

            base.Activate();
        }
        
        public override void Deactivate()
        {
            foreach (var card in _cards)
            {
                card.CardSelected -= SelectCard;
                
                card.Deactivate();
            }

            base.Deactivate();
        }
<<<<<<< Updated upstream
        
        public void SetCardsData(IReadOnlyList<TradeItemData> itemsData)
=======

        public void ConfirmSelection(int index)
        {
            if (RequestGaldAmount == null)
                throw new NullReferenceException($"No one subscribed to '{nameof(RequestGaldAmount)}' event");

            int galdAmount = RequestGaldAmount.Invoke();

            foreach (var card in _cards) 
                card.UpdateGaldInfo(galdAmount);
            
            _cards[index].ConfirmSelection();
        }

        public void UndoSelection(int index) => _cards[index].UndoSelection();

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

        public void SetCardsData(IReadOnlyList<TradeItemData> itemsData, int currentGaldAmount)
>>>>>>> Stashed changes
        {
            for (var i = 0; i < itemsData.Count; i++)
            {
                TradeCard card = _cards[i];

                card.SetData(i, itemsData[i], currentGaldAmount);
                card.Activate();
            }
        }
        
        private void SelectCard(int index) => CardSelected?.Invoke(index);
<<<<<<< Updated upstream
=======

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
>>>>>>> Stashed changes
    }
}