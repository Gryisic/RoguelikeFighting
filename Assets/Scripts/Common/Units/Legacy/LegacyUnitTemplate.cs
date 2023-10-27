using Common.Models.Actions.Templates;
using UnityEngine;

namespace Common.Units.Legacy
{
    [CreateAssetMenu(menuName = "Configs/Templates/Units/Legacy", fileName = "Template")]
    public class LegacyUnitTemplate : UnitTemplate
    {
        [SerializeField] private LegacyActionTemplate _legacyActionTemplate;

        public LegacyActionTemplate LegacyActionTemplate => _legacyActionTemplate;
    }
}