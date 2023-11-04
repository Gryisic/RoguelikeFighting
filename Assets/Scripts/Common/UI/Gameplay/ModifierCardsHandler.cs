using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Gameplay.Modifiers.Templates;
using Common.UI.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Gameplay
{
    public class ModifierCardsHandler : UIElement, IHorizontallyNavigatableUIElement, ISelectableUIElement, IDisposable
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private ModifierCard[] _cards;
        [SerializeField] private List<RarityColorMap> _rarityColorMaps;

        private CancellationTokenSource _cardsTasksTokenSource;
        
        private int _hoveredCardIndex;
        private int _maxIndex;
        
        public event Action Selected;
        public event Action Backed;
        public event Action<int> CardSelected; 

        public void Dispose()
        {
            _cardsTasksTokenSource?.Cancel();
            _cardsTasksTokenSource?.Dispose();
        }
        
        public override void Activate()
        {
            _group.alpha = 1;
            
            foreach (var card in _cards)
            {
                card.CardSelected += SelectCard;
                card.HoveredViaPointer += HoveredViaPointer;
            }

            base.Activate();
        }

        public override void Deactivate()
        {
            foreach (var card in _cards)
            {
                card.CardSelected -= SelectCard;
                card.HoveredViaPointer -= HoveredViaPointer;
                
                card.Deactivate();
            }
            
            base.Deactivate();
        }

        public void SetCardsData(IReadOnlyList<ModifierTemplate> dataList)
        {
            _maxIndex = dataList.Count - 1;

            for (int i = 0; i < dataList.Count; i++)
            {
                ModifierCard card = _cards[i];
                ModifierTemplate data = dataList[i];
                Color rarityColor = _rarityColorMaps.First(m => m.Rarity == data.Rarity).Color;
                
                card.SetData(i, data.Icon, data.Name, data.Description, rarityColor);
                card.Activate();
            }
            
            _hoveredCardIndex = 0;
            _cards[_hoveredCardIndex].Hover();
        }
        
        public void MoveLeft()
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = _hoveredCardIndex - 1 <= 0 ? 0 : _hoveredCardIndex - 1;
            
            UpdateHover(currentIndex, _hoveredCardIndex);
        }

        public void MoveRight()
        {
            int currentIndex = _hoveredCardIndex;
            _hoveredCardIndex = _hoveredCardIndex + 1 >= _maxIndex ? _maxIndex : _hoveredCardIndex + 1;
            
            UpdateHover(currentIndex, _hoveredCardIndex);
        }
        
        public void Select() => _cards[_hoveredCardIndex].Select();

        public void Back() { }

        private void SelectCard(int index)
        {
            _cardsTasksTokenSource = new CancellationTokenSource();
            
            SelectCardAsync(index).Forget();
        }

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

        private async UniTask SelectCardAsync(int index)
        {
            ModifierCard selectedCard = _cards[index];
            
            await selectedCard.CardSelectionTask(_cardsTasksTokenSource.Token);
            await _group.DOFade(0, 0.25f).ToUniTask(cancellationToken: _cardsTasksTokenSource.Token);
            
            CardSelected?.Invoke(index);
        }
        
        [Serializable]
        private struct RarityColorMap
        {
            [SerializeField] private Enums.ModifierRarity _rarity;
            [SerializeField] private Color _color;

            public Enums.ModifierRarity Rarity => _rarity;
            public Color Color => _color;
        }
    }
}