using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class ExpandedSelectionMarker : UIElement
    {
        [SerializeField] private Image _image;
        
        public override void Activate()
        {
            float initialPositionY = _image.rectTransform.position.y;
            
            DOTween.Sequence()
                .Append(_image.DOFade(0.7f, 1f).From(0f))
                .Join(_image.rectTransform.DOMoveY(initialPositionY, 0.5f).SetEase(Ease.InQuart).From(initialPositionY + 10));
            
            base.Activate();
        }

        public override void Deactivate()
        {
            DOTween.Sequence()
                .Append(_image.DOFade(0f, 0.3f))
                .AppendCallback(() => base.Deactivate());
        }
    }
}