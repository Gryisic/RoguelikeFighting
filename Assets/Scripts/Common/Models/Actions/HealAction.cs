using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Actions
{
    public class HealAction : ActionBase
    {
        private readonly IUnitInternalData _internalData;
        
        public HealAction(IUnitInternalData internalData, ActionTemplate data, ActionBase wrappedBase = null) : base(data, wrappedBase)
        {
            _internalData = internalData;
        }

        public override void Execute()
        {
            int maxHealth = _internalData.StatsData.GetStatValue(Enums.Stat.MaxHealth);
            int healAmount = Mathf.CeilToInt((float) maxHealth / 100 * data.HealPercent);
            
            _internalData.StatsData.IncreaseStat(Enums.Stat.Health, healAmount);
        }
    }
}