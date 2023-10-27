using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.StatusEffects
{
    [CreateAssetMenu(menuName = "Configs/Templates/StatusEffects/Effect")]
    public class StatusEffectTemplate : ScriptableObject
    {
        [SerializeField] private Enums.StatusEffect _statusEffect;
        [SerializeField] private int _particleID;
        [SerializeField] private int _affectAmount;
        [SerializeField, Range(1, 100)] private float _affectChance;
        [SerializeField] private float _affectTimer;
        [SerializeField] private float _lifespan;
        
        public Enums.StatusEffect StatusEffect => _statusEffect;
        public int ParticleID => _particleID;
        public int AffectAmount => _affectAmount;
        public float AffectChance => _affectChance;
        public float AffectTimer => _affectTimer;
        public float Lifespan => _lifespan;
    }
}