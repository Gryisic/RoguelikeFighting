using Common.Units.Heroes;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Selection
{
    [CreateAssetMenu(menuName = "Configs/Units/Selection/Unit")]
    public class SelectionHeroTemplate : ScriptableObject
    {
        [SerializeField] private Enums.Hero _hero;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private Sprite[] _idleAnimation;
        [SerializeField] private Sprite[] _selectedAnimation;
        [SerializeField] private HeroTemplate _heroTemplate;

        public HeroTemplate HeroTemplate => _heroTemplate;

        public Enums.Hero Hero => _hero;
        public Sprite Portrait => _portrait;
        public Sprite[] IdleAnimation => _idleAnimation;
        public Sprite[] SelectedAnimation => _selectedAnimation;
    }
}