using System;
using System.Collections.Generic;
using Common.Models.Items;
using UnityEngine;

namespace Common.UI.Gameplay.Rooms
{
    public class TradeCardsHandler : UIElement
    {
        [SerializeField] private TradeCard[] _cards;

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
        
        public void SetCardsData(IReadOnlyList<TradeItemData> itemsData)
        {
            for (var i = 0; i < itemsData.Count; i++)
            {
                TradeCard card = _cards[i];
                
                card.SetData(i, itemsData[i]);
                card.Activate();
            }
        }
        
        private void SelectCard(int index) => CardSelected?.Invoke(index);
    }
}