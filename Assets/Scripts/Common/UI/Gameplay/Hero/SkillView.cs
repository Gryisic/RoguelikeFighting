using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Hero
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Enums.HeroActionType _skillType;

        [SerializeField] private Color _filledColor;
        [SerializeField] private Color _nullColor;

        public Enums.HeroActionType SkillType => _skillType;

        public void UpdateIcon(Sprite sprite)
        {
            _icon.color = sprite == null ? _nullColor : _filledColor;

            _icon.sprite = sprite;
        }
    }
}