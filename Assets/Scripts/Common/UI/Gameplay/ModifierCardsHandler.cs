using System;
using System.Collections.Generic;
using Common.Gameplay.Modifiers.Templates;
using DG.Tweening;
using UnityEngine;

namespace Common.UI.Gameplay
{
    public class ModifierCardsHandler : UIElement
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private ModifierCard[] _cards;

        public event Action<int> CardSelected; 

        public override void Activate()
        {
            _group.DOFade(1, 0.2f).From(0);
            
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

        public void SetCardsData(IReadOnlyList<ModifierTemplate> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                ModifierCard card = _cards[i];
                ModifierTemplate data = dataList[i];
                
                card.SetData(i, data.Icon, data.Name, data.Description);
                card.Activate();
            }
        }

        private void SelectCard(int index)
        {
            CardSelected?.Invoke(index);
        }
    }
}