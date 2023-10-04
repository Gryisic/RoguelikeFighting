using System.Collections.Generic;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Actions.Templates
{
    [CreateAssetMenu(menuName = "Configs/Actions/HeroAction")]
    public class HeroActionTemplate : ActionTemplate
    {
        [Space, Header("Hero Data")]
        [SerializeField] private HeroActionTemplate[] _extendsInto;
        [SerializeField] private Enums.HeroActionType _extendsFrom;
        [SerializeField] private Enums.InputDirection _direction;
        [SerializeField] private Enums.HeroActionExecutionPlacement _executionPlacement;
        
        public IReadOnlyList<HeroActionTemplate> ExtendsInto => _extendsInto;
        public Enums.HeroActionType ExtendsFrom => _extendsFrom;
        public Enums.InputDirection Direction => _direction;
        public Enums.HeroActionExecutionPlacement ExecutionPlacement => _executionPlacement;
    }
}