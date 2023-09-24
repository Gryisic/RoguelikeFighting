using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay
{
    [Serializable]
    public class HealthBar
    {
        [SerializeField] private Image _bar;

        public virtual void UpdateValue(int maxHealth, int currentHealth)
        {
            if (maxHealth > currentHealth)
                throw new ArgumentException("Current health is greater then max health");
            
            maxHealth = Mathf.Min(maxHealth, 999);
            currentHealth = Mathf.Max(0, currentHealth);
            
            _bar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}