using System;
using Common.Models.Items;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class TradeCard : UIElement, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _backgroundText;
        [SerializeField] private Image _card;
        [SerializeField] private Image _hideCard;
        [SerializeField] private Image _maskImage;
        [SerializeField] private Image _borders;
        [SerializeField] private Image _costIcon;
        [SerializeField] private Sprite _soldIcon;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private Color _filledColor;

        [SerializeField] private Vector3 _punchVector;

        private bool _isHoverAllowed;
        private bool _canBeSelected;
        private int _index;

        public event Action<int> Hovered;
        public event Action<int> CardSelected;

        public override void Activate()
        {
            SetActivateData();
            SetItemDataVisibilityTo(true);
            PlayActivateSequence();

            base.Activate();
        }

        public override void Deactivate()
        {
            SetDeactivateData();
            PlayDeactivateSequence();
        }

        public void SetData(int index, TradeItemData data)
        {
            _index = index;
            
            _icon.sprite = data.Icon;
            _description.text = data.Description;
            _cost.text = data.Cost.ToString();
        }

        public void Select()
        {
            if (_canBeSelected == false)
                return;

            _canBeSelected = false;

            _icon.sprite = _soldIcon;
            _backgroundText.gameObject.SetActive(false);

            Transform.DOPunchScale(_punchVector, 0.2f);
            
            SetItemDataVisibilityTo(false);

            CardSelected?.Invoke(_index);
        }
        
        public void Hover()
        {
            _borders.color = _hoverColor;
            _card.color = _defaultColor;
            
            if (_canBeSelected)
                _backgroundText.gameObject.SetActive(true);

            DOTween.Sequence()
                .Append(Transform.DOScale(1.2f, 0.2f).From(1))
                .Join(_maskImage.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.InQuart).From(0))
                .AppendCallback(() => _card.color = _filledColor);
        }

        public void UnHover()
        {
            _borders.color = _defaultColor;
            _backgroundText.gameObject.SetActive(false);

            Transform.DOScale(1f, 0.2f).From(1.2f);
        }

        public void OnPointerClick(PointerEventData eventData) => Select();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isHoverAllowed)
            {
                Hovered?.Invoke(_index);
            }
        }

        private void SetItemDataVisibilityTo(bool flag)
        {
            _description.gameObject.SetActive(flag);
            _cost.gameObject.SetActive(flag);
            _costIcon.gameObject.SetActive(flag);
        }
        
        private void SetActivateData()
        {
            _canBeSelected = true;

            _borders.color = _defaultColor;
            _maskImage.color = _defaultColor;
            _card.color = _filledColor;
            _hideCard.color = _filledColor;

            Transform.localScale = Vector3.one;
            _maskImage.rectTransform.localScale = Vector3.one;
            _borders.rectTransform.localScale = Vector3.one;

            _hideCard.rectTransform.SetAsFirstSibling();
            _backgroundText.gameObject.SetActive(false);
        }
        
        private void PlayActivateSequence()
        {
            DOTween.Sequence()
                .Append(_group.DOFade(1f, 0.2f).From(0))
                .AppendCallback(() => _isHoverAllowed = true);
        }

        private void SetDeactivateData()
        {
            _isHoverAllowed = false;
            _canBeSelected = false;

            _card.color = _defaultColor;
            _hideCard.color = _defaultColor;
            _maskImage.color = _filledColor;
        }
        
        private void PlayDeactivateSequence()
        {
            DOTween.Sequence()
                .Append(_card.DOFade(0f, 0f))
                .Append(_maskImage.rectTransform.DOScale(0.15f, 0.2f))
                .Join(_borders.rectTransform.DOScale(0.5f, 0.2f))
                .AppendCallback(() => _hideCard.color = _filledColor)
                .AppendCallback(() => _hideCard.rectTransform.SetAsLastSibling())
                .Join(_maskImage.rectTransform.DOScale(0, 0.5f))
                .Join(Transform.DOScale(0, 0.1f))
                .AppendCallback(() => base.Deactivate());
        }
    }
}