using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    [RequireComponent(typeof(Canvas))]
    public class SelectionRoomView : UIElement
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SelectionMarkersHandler _selectionMarkersHandler;
        [SerializeField] private Image _finalIcon;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _filledColor;

        public SelectionMarkersHandler SelectionMarkersHandler => _selectionMarkersHandler;

        private void Awake()
        {
            if (_canvas == null)
            {
                Debug.LogError("Canvas of SelectionRoomView is not assigned");

                _canvas = GetComponent<Canvas>();
            }
        }

        public override void Activate()
        {
            _selectionMarkersHandler.Activate();
            
            _canvas.enabled = true;
        }

        public override void Deactivate()
        {
            _canvas.enabled = false;
            
            _selectionMarkersHandler.Deactivate();
            
            _finalIcon.gameObject.SetActive(false);
        }

        public void SetIcon(Sprite icon)
        {
            _finalIcon.sprite = icon;

            _finalIcon.gameObject.SetActive(true);

            DOTween.Sequence()
                .Append(_finalIcon.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.InQuart).From(2f))
                .Join(_finalIcon.DOColor(_filledColor, 0.5f).From(_defaultColor));
        }
    }
}