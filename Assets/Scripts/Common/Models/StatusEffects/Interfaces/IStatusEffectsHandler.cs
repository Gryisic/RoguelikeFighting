using System;

namespace Common.Models.StatusEffects.Interfaces
{
    public interface IStatusEffectsHandler : IDisposable
    {
        void AddEffect(StatusEffect effect);
        bool RemoveEffect(StatusEffect effect);
        void Clear();
    }
}