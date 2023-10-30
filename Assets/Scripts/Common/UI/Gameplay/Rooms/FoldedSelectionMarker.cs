using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class FoldedSelectionMarker : UIElement
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _borders;

        private Sequence _activeSequence;
        
        public override void Activate()
        {
            _activeSequence?.Complete();

            _image.rectTransform.localScale = Vector3.one;
            
            _activeSequence = DOTween.Sequence()
                .Append(_image.DOFade(1f, 0.5f).From(0f))
                .Append(_borders.DOFade(1f, 0.2f).From(0f))
                .Append(_borders.rectTransform.DOScale(1f, 0.2f).From(2f));

            base.Activate();
        }

        public override void Deactivate()
        {
            _activeSequence.Complete();
            
            _activeSequence = DOTween.Sequence()
                .Append(_image.DOFade(0f, 0.3f).From(1f))
                .Append(_borders.DOFade(0f, 0.3f).From(1f))
                .Append(_image.rectTransform.DOScale(0f, 0.3f).From(1f))
                .Join(_borders.rectTransform.DOScale(0f, 0.3f).From(1f))
                .AppendCallback(() => base.Deactivate());
        }
    }
}