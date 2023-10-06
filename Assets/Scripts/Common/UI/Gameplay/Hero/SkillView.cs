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

        private void Awake()
        {
            _icon.color = _nullColor;
        }

        public void UpdateIcon(Sprite sprite)
        {
            if (sprite == null)
            {
                Debug.Log(_skillType);
                _icon.color = _nullColor;
            }
            else
            {
                _icon.color = _filledColor;
            }
            
            _icon.sprite = sprite;
        }
    }
}