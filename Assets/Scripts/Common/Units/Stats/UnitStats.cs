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

        public void IncreaseStat(Enums.Stat type, int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to increase stat at negative value. Value: {amount}");

            _statsMap[type] += amount;
        }

        public void DecreaseStat(Enums.Stat type, int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to decrease stat at negative value. Value: {amount}");

            _statsMap[type] -= amount;
        }
    }
}