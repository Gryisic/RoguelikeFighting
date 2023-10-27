using Common.Models.Particles;
using UnityEngine;

namespace Common.UI.Gameplay
{
    public class DamageView : UIElement
    {
        [SerializeField] private TextRendererParticle _damageParticle;
        
        public void DisplayAt(Vector3 position, int value) => _damageParticle.DisplayAt(position, value.ToString());
    }
}