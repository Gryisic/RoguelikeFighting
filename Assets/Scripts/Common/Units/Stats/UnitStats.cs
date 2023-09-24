using System;
using System.Collections.Generic;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Stats
{
    public class UnitStats : IUnitStatsData
    {
        private Dictionary<Enums.Stat, int> _statsMap;

        public void SetData(UnitTemplate template)
        {
            _statsMap = new Dictionary<Enums.Stat, int>()
            {
                { Enums.Stat.MaxHealth, template.MaxHealth },
                { Enums.Stat.Health, template.MaxHealth },
            };
        }
        
        public int GetStatValue(Enums.Stat type)
        {
            if (_statsMap.ContainsKey(type) == false)
                throw new InvalidOperationException($"Type '{type}' doesn't present in stats map");
            
            return _statsMap[type];
        }
    }
}