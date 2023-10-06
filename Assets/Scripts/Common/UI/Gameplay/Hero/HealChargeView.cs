using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Hero
{
    public class HealChargeView : UIElement
    {
        [SerializeField] private Image _icon;

        [SerializeField] private Color _filledColor;
        [SerializeField] private Color _emptyColor;

        public override void Activate() => _icon.color = _filledColor;
        
        public override void Deactivate() => _icon.color = _emptyColor;
    }
}