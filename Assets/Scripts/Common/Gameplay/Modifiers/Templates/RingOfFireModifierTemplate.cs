using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Modifiers.Templates
{
    [CreateAssetMenu(menuName = "Configs/Templates/Modifiers/RingOfFire", fileName = "Template")]   
    public class RingOfFireModifierTemplate : ModifierTemplate
    {
        [Space, Header("Execution Data")]
        [SerializeField] private float _time;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private int _damage = 100;
        [SerializeField] private float _damageMultiplier = 1f;

        public float Time => _time;
        public float Radius => _radius;
        public int Damage => _damage;
        public float DamageMultiplier => _damageMultiplier;

        public override Enums.Modifier Type => Enums.Modifier.RingOfFire;
    }
}