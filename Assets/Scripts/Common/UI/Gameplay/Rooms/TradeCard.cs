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
<<<<<<< Updated upstream

=======
        [SerializeField] private Color _filledColor;
        [SerializeField] private Color _enoughGaldColor;
        [SerializeField] private Color _lessGaldColor;
            
        [SerializeField] private Vector3 _punchVector;

        private bool _isHoverAllowed;
        private bool _canBeSelected;
>>>>>>> Stashed changes
        private int _index;
        private int _itemCost;

        public event Action<int> CardSelected;

        public override void Activate()
        {
            _borders.color = _defaultColor;
            
            base.Activate();
        }

<<<<<<< Updated upstream
        public void SetData(int index, TradeItemData data)
=======
        public override void Deactivate()
        {
            SetDeactivateData();
            PlayDeactivateSequence();
        }

        public void SetData(int index, TradeItemData data, int currentGaldAmount)
>>>>>>> Stashed changes
        {
            _index = index;
            _itemCost = data.Cost;
            
            _icon.sprite = data.Icon;
            _description.text = data.Description;
            _cost.text = data.Cost.ToString(); 
            
            UpdateGaldInfo(currentGaldAmount);
        }

<<<<<<< Updated upstream
        public void OnPointerClick(PointerEventData eventData) => CardSelected?.Invoke(_index);

=======
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

>>>>>>> Stashed changes
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