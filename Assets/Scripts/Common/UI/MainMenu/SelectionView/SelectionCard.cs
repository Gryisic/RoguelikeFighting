using System;
using Common.UI.Interfaces;
using Common.Units.Selection;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.MainMenu.SelectionView
{
    public class SelectionCard : UIElement
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private TextMeshProUGUI _type;
        [SerializeField] private TextMeshProUGUI _additionalText;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Transform _borders;

        private Vector3 _leftBorderPosition;
        private Vector3 _rightBorderPosition;
        private Vector3 _initialPosition;
        private float _initialPortraitPositionY;

        private Sequence _hoverSequence;
        private Sequence _moveSequence;
        
        public event Action<int> HoveredViaPointer;

        public override void Activate()
        {
            _initialPosition = Transform.localPosition;
            _initialPortraitPositionY = _portrait.rectTransform.localPosition.y;
            
            base.Activate();
        }

        public void MoveLeft()
        {
            _moveSequence?.Complete();
            
            _moveSequence = DOTween.Sequence()
                .Append(Transform.DOMoveX(Transform.position.x - Constants.UnitSelectionCardHorizontalOffset, 0.25f))
                .AppendCallback(ValidatePosition);
        }

        public void MoveRight()
        {
            _moveSequence?.Complete();
            
            _moveSequence = DOTween.Sequence()
                .Append(Transform.DOMoveX(Transform.position.x + Constants.UnitSelectionCardHorizontalOffset, 0.25f))
                .AppendCallback(ValidatePosition);
        }

        public void MoveTo(Vector3 position) => Transform.DOMove(position, 0.25f);
        
        public void ResetPosition() => Transform.localPosition = _initialPosition;

        public void Hover()
        {
            ToggleTypeActivity(true);

            _hoverSequence?.Complete();
            
            _hoverSequence = DOTween.Sequence()
                .Append(_group.DOFade(1f, 0.25f).From(0.5f))
                .Join(_portrait.rectTransform.DOLocalMoveY(_initialPortraitPositionY + Constants.UnitSelectionCardPortraitVerticalOffset, 0.25f))
                .Join(_type.DOFade(1, 0.25f))
                .Join(_additionalText.DOFade(1, 0.25f));
        }

        public void UnHover()
        {
            _hoverSequence?.Complete();
            
            _hoverSequence = DOTween.Sequence()
                .Append(_group.DOFade(0.5f, 0.25f).From(1f))
                .Join(_portrait.rectTransform.DOLocalMoveY(_initialPortraitPositionY, 0.25f))
                .Join(_type.DOFade(0, 0.25f))
                .Join(_additionalText.DOFade(0, 0.25f))
                .AppendCallback(() => ToggleTypeActivity(false));
        }
        
        public void SetData(SelectionHeroTemplate data)
        {
            _portrait.sprite = data.Portrait;
            _type.text = Enum.GetName(typeof(Enums.Hero), data.Hero);
        }

        public void SetBorders(Vector3 leftBorder, Vector3 rightBorder)
        {
            _leftBorderPosition = leftBorder;
            _rightBorderPosition = rightBorder;
        }

        private void ImmediatelyMoveTo(Vector3 position) => Transform.position = position;

        private void ValidatePosition()
        {
            if (Transform.localPosition.x < _leftBorderPosition.x)
                ImmediatelyMoveTo(_rightBorderPosition);
            else if (Transform.localPosition.x > _rightBorderPosition.x)
                ImmediatelyMoveTo(_leftBorderPosition);
        }
        
        private void ToggleTypeActivity(bool isActive)
        {
            switch (isActive)
            {
                case true:
                    _type.gameObject.SetActive(true);
                    _additionalText.gameObject.SetActive(true);
                    break;
                
                case false:
                    _type.gameObject.SetActive(false);
                    _additionalText.gameObject.SetActive(false);
                    break;
            }
        }
    }
}