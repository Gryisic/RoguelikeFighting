using System;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.Gameplay
{
    public class ModifierCard : UIElement, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _card;
        [SerializeField] private Image _borders;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        
        private int _index;
        private float _initialPositionY;

        public event Action<int> CardSelected; 

        public override void Activate()
        {
            gameObject.SetActive(true);

            _initialPositionY = Transform.position.y;
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void SetData(int index, Sprite icon, string modifierName, string description, Color color)
        {
            _index = index;
            _icon.sprite = icon;
            _name.text = modifierName;
            _description.text = description;

            SetColors(color);
        }

        public void Hover()
        {
            float finalPosition = _initialPositionY + Constants.ModifierCardVerticalOffset;
            
            Transform.DOMoveY(finalPosition, 0.2f);
        }

        public void UnHover()
        {
            Transform.DOMoveY(_initialPositionY, 0.2f);
        }

        public void OnPointerClick(PointerEventData eventData) => CardSelected?.Invoke(_index);
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UnHover();
        }
        
        private void SetColors(Color color)
        {
            _icon.color = color;
            _card.color = color;
            _borders.color = color;
            _name.color = color;
        }
    }
}