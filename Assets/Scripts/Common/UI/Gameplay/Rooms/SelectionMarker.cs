using System;
using Common.Gameplay.Rooms;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class SelectionMarker : UIElement
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _type;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private FoldedSelectionMarker _foldedMarker;
        [SerializeField] private ExpandedSelectionMarker _expandedMarker;

        private readonly float _scaleModifier = 1.4f;
        
        private Vector3 _initialIconScale;
        private float _initialTypeTextSize;

        public override void Activate()
        {
            _initialIconScale = _icon.rectTransform.localScale;
            _initialTypeTextSize = _type.fontSize;
            
            _foldedMarker.Activate();
            
            base.Activate();
        }

        public void Expand()
        {
            _type.gameObject.SetActive(false);
            
            _foldedMarker.Deactivate();
            _expandedMarker.Activate();

            _type.fontSize = _initialTypeTextSize * _scaleModifier;

            DOTween.Sequence()
                .Append(_icon.DOFade(0f, 0.3f))
                .Append(_icon.rectTransform.DOScale(_initialIconScale * _scaleModifier, 0f))
                .AppendCallback(() => _type.gameObject.SetActive(true))
                .AppendCallback(() => _description.gameObject.SetActive(true))
                .Append(_icon.DOFade(0.65f, 1f));
        }
        
        public void Fold()
        {
            _type.gameObject.SetActive(false);
            _description.gameObject.SetActive(false);
            
            _expandedMarker.Deactivate();
            _foldedMarker.Activate();
            
            _type.fontSize = _initialTypeTextSize;

            DOTween.Sequence()
                .Append(_icon.DOFade(0f, 0.3f))
                .Append(_icon.rectTransform.DOScale(_initialIconScale, 0f))
                .AppendCallback(() => _type.gameObject.SetActive(true))
                .Append(_icon.DOFade(0.65f, 0.5f));
        }

        public void SetData(RoomTemplate data)
        {
            _icon.sprite = data.Icon;
            _type.text = Enum.GetName(typeof(Enums.RoomType), data.Type);
            _description.text = data.Description;
        }
    }
}