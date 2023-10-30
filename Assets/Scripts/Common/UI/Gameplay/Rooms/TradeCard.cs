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
        [SerializeField] private Sprite _soldIcon;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _backgroundText;
        [SerializeField] private Image _card;
        [SerializeField] private Image _hideCard;
        [SerializeField] private Image _maskImage;
        [SerializeField] private Image _borders;
        [SerializeField] private Image _costIcon;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private Color _filledColor;
        [SerializeField] private Color _enoughGaldColor;
        [SerializeField] private Color _lessGaldColor;
            
        [SerializeField] private Vector3 _punchVector;

        private bool _isHoverAllowed;
        private bool _canBeSelected;
        private int _index;
        private int _itemCost;

        public event Action<int> CardSelected;
        public event Action<int> HoveredViaPointer; 

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

        public void SetData(int index, TradeItemData data, int currentGaldAmount)
        {
            _index = index;
            _itemCost = data.Cost;
            
            _icon.sprite = data.Icon;
            _description.text = data.Description;
            _cost.text = data.Cost.ToString(); 
            
            UpdateGaldInfo(currentGaldAmount);
        }

        public void OnPointerClick(PointerEventData eventData) => CardSelected?.Invoke(_index);

        public void UpdateGaldInfo(int currentGaldAmount) => _cost.color = currentGaldAmount > _itemCost ? _enoughGaldColor : _lessGaldColor;

        public void Select()
        {
            if (_canBeSelected == false)
                return;

            CardSelected?.Invoke(_index);
        }

        public void ConfirmSelection()
        {
            _canBeSelected = false;

            _icon.sprite = _soldIcon;
            _backgroundText.gameObject.SetActive(false);

            Transform.DOPunchScale(_punchVector, 0.2f);
            
            SetItemDataVisibilityTo(false);
        }

        public void UndoSelection()
        {
            Color initialColor = _borders.color;
            
            _borders.color = _lessGaldColor;
            
            DOTween.Sequence()
                .Append(Transform.DOShakeAnchorPos(0.5f, _punchVector))
                .AppendCallback(() => _borders.color = initialColor);
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
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isHoverAllowed)
                HoveredViaPointer?.Invoke(_index);
        }

        private void SetItemDataVisibilityTo(bool isVisible)
        {
            _description.gameObject.SetActive(isVisible);
            _cost.gameObject.SetActive(isVisible);
            _costIcon.gameObject.SetActive(isVisible);
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