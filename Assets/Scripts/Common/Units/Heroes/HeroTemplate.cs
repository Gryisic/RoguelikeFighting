using System.Collections.Generic;
using Common.Models.Actions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Heroes
{
    [CreateAssetMenu(menuName = "Configs/Templates/Units/Hero", fileName = "Template")]
    public class HeroTemplate : UnitTemplate
    {
        [SerializeField] private Enums.Hero _heroType;

        [Space, Header("Dash Data")]
        [SerializeField] private float _dashDistance;
        [SerializeField] private float _dashForce;
        [SerializeField] private int _maxDashesCount;

        [Space, Header("Jump Data")]
        [SerializeField] private int _jumpsCount;

        [Space, Header("Animation Data")]
        [SerializeField] private HeroAnimationData _heroAnimationData;

        [Space, Header("ActionsData")]
        [SerializeField] private List<HeroActionTemplate> _actions;
        
        public Enums.Hero HeroType => _heroType;
        
        public float DashDistance => _dashDistance;
        public float DashForce => _dashForce;
        public int MaxDashesCount => _maxDashesCount;
        
        public int JumpsCount => _jumpsCount;

        public HeroAnimationData heroAnimationData => _heroAnimationData;
        
        public IReadOnlyList<HeroActionTemplate> Actions => _actions;
    }
}