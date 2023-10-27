using Common.Models.StatusEffects.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.StatusEffects
{
    public struct StatusEffectData : IStatusEffectData
    {
        public Enums.StatusEffect StatusEffect { get; }
        public int ParticleID { get; }
        public int AffectAmount { get; }
        public float AffectChance { get; }
        public float AffectTimer { get; }
        public float Lifespan { get; }
        
        public StatusEffectData(StatusEffectTemplate template)
        {
            StatusEffect = template.StatusEffect;
            ParticleID = template.ParticleID;
            AffectAmount = template.AffectAmount;
            AffectChance = template.AffectChance;
            AffectTimer = template.AffectTimer;
            Lifespan = template.Lifespan;
        }
        
        public StatusEffectData(StatusEffectTemplate template, IStatusEffectData data)
        {
            StatusEffect = data.StatusEffect;
            ParticleID = data.ParticleID;
            AffectAmount = template.AffectAmount + data.AffectAmount;
            AffectChance = template.AffectChance + data.AffectChance;
            AffectTimer = template.AffectTimer + data.AffectTimer;
            Lifespan = template.Lifespan + data.Lifespan;
        }
    }
}