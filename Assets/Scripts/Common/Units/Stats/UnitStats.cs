using System;
using System.Collections.Generic;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Stats
{
    public class UnitStats : IUnitStatsData
    {
        private Dictionary<Enums.Stat, float> _statsMap;

        public event Action<int, int> HealthChanged;
        
        public void SetData(UnitTemplate template)
        {
            _statsMap = new Dictionary<Enums.Stat, float>()
            {
                { Enums.Stat.MaxHealth, template.MaxHealth },
                { Enums.Stat.Health, template.MaxHealth },
                { Enums.Stat.AttackMultiplier, Constants.DefaultStatMultiplier},
                { Enums.Stat.DefenceMultiplier , Constants.DefaultStatMultiplier}
            };
        }

        public float GetStatValue(Enums.Stat type)
        {
            if (_statsMap.ContainsKey(type) == false)
                throw new InvalidOperationException($"Type '{type}' doesn't present in stats map");
            
            return _statsMap[type];
        }

        public int GetStatValueAsInt(Enums.Stat type) => (int) GetStatValue(type);

        public void IncreaseStat(Enums.Stat type, float amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to increase stat at negative value. Value: {amount}");

            _statsMap[type] += amount;

            if (type == Enums.Stat.Health) 
                RaiseHealthChangedEvent();
        }

        public void DecreaseStat(Enums.Stat type, float amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to decrease stat at negative value. Value: {amount}");

            //_statsMap[type] -= amount;
            _statsMap[type] = Mathf.Max(0, _statsMap[type] - amount);
            
            if (type == Enums.Stat.Health) 
                RaiseHealthChangedEvent();
        }
        
        private void RaiseHealthChangedEvent()
        {
            int currentHealth = (int) _statsMap[Enums.Stat.Health];
            int maxHealth = (int) _statsMap[Enums.Stat.MaxHealth];

            if (currentHealth > maxHealth)
            {
                _statsMap[Enums.Stat.Health] = maxHealth;
                currentHealth = maxHealth;
            }

            HealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}