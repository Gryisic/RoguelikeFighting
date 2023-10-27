using Infrastructure.Utils;

namespace Common.Models.StatusEffects.Interfaces
{
    public interface IStatusEffectData
    {
        Enums.StatusEffect StatusEffect { get; }
        int ParticleID { get; }
        int AffectAmount { get; }
        float AffectChance { get; }
        float AffectTimer { get; }
        float Lifespan { get; }
    }
}