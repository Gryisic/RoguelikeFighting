using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.MainMenu.MenuView.Buttons
{
    public abstract class Button : AnimatableUIElement, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _innerBorder;
        [SerializeField] private Image _outerBorder;
        [SerializeField] private Image _leftCorner;
        [SerializeField] private Image _rightCorner;

        protected bool isActive;

        private Sequence _activeSequence;

        private Vector3 _initialLeftCornerPosition;
        private Vector3 _initialRightCornerPosition;
        private Vector3 _initialInnerBorderScale;
        private Vector3 _initialOuterBorderScale;

        private int _index;

        public event Action<int> HoveredViaPointer;
        public abstract event Action Pressed;

        public abstract void Execute();

        public void Initialize()
        {
            _initialLeftCornerPosition = _leftCorner.rectTransform.localPosition;
            _initialRightCornerPosition = _rightCorner.rectTransform.localPosition;
            _initialInnerBorderScale = _innerBorder.rectTransform.localScale;
            _initialOuterBorderScale = _outerBorder.rectTransform.localScale;
        }

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            _leftCorner.rectTransform.localPosition = Vector3.zero;
            _rightCorner.rectTransform.localPosition = Vector3.zero;
            _innerBorder.rectTransform.localScale = Vector3.zero;
            _outerBorder.rectTransform.localScale = Vector3.zero;

            _text.DOFade(0, 0f);
            _outerBorder.DOFade(1, 0f);

            base.Activate();
            
            await DOTween.Sequence()
                .AppendInterval(1f)
                .Append(_leftCorner.DOFade(1, 0.25f).From(0))
                .Join(_rightCorner.DOFade(1, 0.25f).From(0))
                .Append(_leftCorner.rectTransform.DOLocalMoveX(_initialLeftCornerPosition.x, 0.25f))
                .Join(_rightCorner.rectTransform.DOLocalMoveX(_initialRightCornerPosition.x, 0.25f))
                .Join(_innerBorder.rectTransform.DOScaleX(_initialInnerBorderScale.x, 0.25f))
                .Join(_outerBorder.rectTransform.DOScaleX(_initialOuterBorderScale.x, 0.25f))
                .Append(_leftCorner.rectTransform.DOLocalMoveY(_initialLeftCornerPosition.y, 0.25f))
                .Join(_rightCorner.rectTransform.DOLocalMoveY(_initialRightCornerPosition.y, 0.25f))
                .Join(_innerBorder.rectTransform.DOScaleY(_initialInnerBorderScale.y, 0.25f))
                .Join(_outerBorder.rectTransform.DOScaleY(_initialOuterBorderScale.y, 0.25f))
                .AppendCallback(() => _text.gameObject.SetActive(true))
                .Append(_text.DOFade(1, 0.25f))
                .AppendCallback(() => isActive = true)
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            isActive = false;
            
            await DOTween.Sequence()
                .Append(_text.DOFade(0, 0.25f))
                .AppendCallback(() => _text.gameObject.SetActive(false))
                .Append(_outerBorder.rectTransform.DOScaleY(0, 0.25f))
                .Join(_innerBorder.rectTransform.DOScaleY(0, 0.25f))
                .Join(_rightCorner.rectTransform.DOLocalMoveY(0, 0.25f))
                .Join(_leftCorner.rectTransform.DOLocalMoveY(0, 0.25f))
                .Append(_outerBorder.rectTransform.DOScaleX(0, 0.25f))
                .Join(_innerBorder.rectTransform.DOScaleX(0, 0.25f))
                .Join(_rightCorner.rectTransform.DOLocalMoveX(0, 0.25f))
                .Join(_leftCorner.rectTransform.DOLocalMoveX(0, 0.25f))
                .Append(_leftCorner.DOFade(0, 0.25f).From(1))
                .Join(_rightCorner.DOFade(0, 0.25f).From(1))
                .ToUniTask(cancellationToken: token);
            
            base.Deactivate();
        }

        public void SetIndex(int index) => _index = index;
        
        public void Hover()
        {
            _activeSequence?.Complete();
            
            Vector3 cornersOffset = new Vector3(-0.5f, 0.25f, 0f);
            Vector3 bordersScale = new Vector3(0.1f, 0.1f, 0f);
            
            _activeSequence = DOTween.Sequence()
                                .Append(_leftCorner.rectTransform.DOLocalMove(_leftCorner.rectTransform.localPosition + cornersOffset, 0.25f))
                                .Join(_rightCorner.rectTransform.DOLocalMove(_rightCorner.rectTransform.localPosition - cornersOffset, 0.25f))
                                .Join(_outerBorder.rectTransform.DOScale(_outerBorder.rectTransform.localScale + bordersScale, 0.25f))
                                .Join(_innerBorder.rectTransform.DOScale(_innerBorder.rectTransform.localScale + bordersScale, 0.25f))
                                .Join(_outerBorder.DOFade(0, 0.25f));
        }

        public void UnHover()
        {
            _activeSequence?.Complete();

            _activeSequence = DOTween.Sequence()
                                .Append(_outerBorder.DOFade(1, 0.25f))
                                .Join(_innerBorder.rectTransform.DOScale(_initialInnerBorderScale, 0.25f))
                                .Join(_outerBorder.rectTransform.DOScale(_initialOuterBorderScale, 0.25f))
                                .Join(_rightCorner.rectTransform.DOLocalMove(_initialRightCornerPosition, 0.25f))
                                .Join(_leftCorner.rectTransform.DOLocalMove(_initialLeftCornerPosition, 0.25f));
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (isActive == false)
                return;
            
            Execute();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (isActive == false)
                return;
            
            HoveredViaPointer?.Invoke(_index);
        }
    }
}