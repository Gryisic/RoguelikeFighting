using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.UI.Gameplay
{
    public class HeroView : UIElement
    {
        [FormerlySerializedAs("_heroHealthBar")] [SerializeField] private CanvasHealthBar _canvasHealthBar;
        
        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateHealth(int currentValue, int maxValue) => _canvasHealthBar.UpdateValue(currentValue, maxValue);
    }
}