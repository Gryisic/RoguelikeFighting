using Common.Models.Actions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Enemies
{
    [CreateAssetMenu(menuName = "Configs/Templates/Units/Enemy", fileName = "Template")]
    public class EnemyTemplate : UnitTemplate
    {
        [SerializeField] private Enums.Enemy _type;
        [SerializeField] private EnemyActionTemplate _actionTemplate;

        public Enums.Enemy Type => _type;
        public EnemyActionTemplate ActionTemplate => _actionTemplate;
    }
}