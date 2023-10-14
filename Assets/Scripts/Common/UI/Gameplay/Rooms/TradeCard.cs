using System;
using Common.Models.Items;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class TradeCard : UIElement, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _backgroundText;
        [SerializeField] private Image _borders;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoverColor;

        private int _index;

        public event Action<int> CardSelected;

        public override void Activate()
        {
            _borders.color = _defaultColor;
            
            base.Activate();
        }

        public void SetData(int index, TradeItemData data)
        {
            _index = index;
            
            _icon.sprite = data.Icon;
            _description.text = data.Description;
            _cost.text = data.Cost.ToString();
        }

        public void OnPointerClick(PointerEventData eventData) => CardSelected?.Invoke(_index);

        public void Hover()
        {
            _borders.color = _hoverColor;
            
            Transform.DOScale(1.2f, 0.2f).From(1);
        }

        public void UnHover()
        {
            _borders.color = _defaultColor;
            
            Transform.DOScale(1f, 0.2f).From(1.2f);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UnHover();
        }
    }
}