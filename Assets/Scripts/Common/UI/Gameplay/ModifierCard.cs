using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.Gameplay
{
    public class ModifierCard : UIElement, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _card;
        [SerializeField] private Image _borders;
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        
        private bool _canBeHovered;
        private int _index;
        private float _initialPositionY;

        public event Action<int> CardSelected;
        public event Action<int> HoveredViaPointer;

        public override void Activate()
        {
            _card.rectTransform.localScale = Vector3.one;
            _initialPositionY = Transform.position.y;

            gameObject.SetActive(false);
            
            DOTween.Sequence()
                .Append(Transform.DOMoveY(_initialPositionY - Constants.ModifierCardActivationVerticalOffset, 0))
                .AppendCallback(() => gameObject.SetActive(true))
                .Append(Transform.DOMoveY(_initialPositionY, 0.5f))
                .AppendCallback(() => _canBeHovered = true);
        }

        public override void Deactivate()
        {
            _canBeHovered = false;
            
            gameObject.SetActive(false);

            Transform.DOMoveY(_initialPositionY, 0f);
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
            if (_canBeHovered == false)
            {
                DOTween.Sequence().AppendInterval(0.5f).AppendCallback(Hover);
                return;
            }
            
            float finalPosition = _initialPositionY + Constants.ModifierCardHoverVerticalOffset;
            
            Transform.DOMoveY(finalPosition, 0.2f);
        }

        public void UnHover() => Transform.DOMoveY(_initialPositionY, 0.2f);

        public void Select() => CardSelected?.Invoke(_index);

        public void OnPointerClick(PointerEventData eventData) => Select();
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_canBeHovered)
            {
                HoveredViaPointer?.Invoke(_index);
            }
        }

        public async UniTask CardSelectionTask(CancellationToken token)
        {
            UniTask selectionTask = DOTween.Sequence()
                .Append(_card.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.05f))
                .Append(_card.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f))
                .ToUniTask(cancellationToken: token);

            await selectionTask;
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