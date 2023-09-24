using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Common.UI.Gameplay
{
    [Serializable]
    public class HeroHealthBar : HealthBar
    {
        [SerializeField] private TextMeshProUGUI _textValues;
        
        public override void UpdateValue(int maxHealth, int currentHealth)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(currentHealth).Append("/").Append("<alpha=#66>").Append(maxHealth).Append("<sup>").Append("HP");

            _textValues.text = stringBuilder.ToString();

            base.UpdateValue(maxHealth, currentHealth);
        }
    }
}