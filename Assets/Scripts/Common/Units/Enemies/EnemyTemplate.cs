using System.Collections.Generic;
using Common.Models.Actions.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Enemies
{
    [CreateAssetMenu(menuName = "Configs/Templates/Units/Enemy", fileName = "Template")]
    public class EnemyTemplate : UnitTemplate
    {
        [SerializeField] private Enums.Enemy _type;
        [SerializeField] private Enums.ActionExecutionAwait _executionType;
        [SerializeField] private ActionTemplate[] _actionTemplates;

        [Space, Header("Near Execution")]
        [SerializeField] private float _attackDistance;

        public Enums.Enemy Type => _type;
        public Enums.ActionExecutionAwait ExecutionType => _executionType;
        public IReadOnlyList<ActionTemplate> ActionTemplates => _actionTemplates;
        
        public float AttackDistance => _attackDistance;
    }
}