using System;
using UnityEngine;

namespace Common.UI.Gameplay
{
    public class HealthBarsView : UIElement
    {
        [SerializeField] private HeroHealthBar _heroHealthBar;
        [SerializeField] private HealthBar[] _enemiesHealthBars;
        
        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateHeroHealth(int currentValue, int maxValue)
        {
            _heroHealthBar.UpdateValue(currentValue, maxValue);
        }

        public void UpdateEnemyHealth(int currentValue, int maxValue)
        {
            
        }
    }
}