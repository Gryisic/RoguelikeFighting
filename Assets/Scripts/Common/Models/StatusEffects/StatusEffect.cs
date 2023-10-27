using System;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Interfaces;

namespace Common.Models.StatusEffects
{
    public abstract class StatusEffect : IDisposable
    {
        public abstract event Action<StatusEffect> EffectEnded; 
        public IStatusEffectData Data { get; }

        protected StatusEffect(IStatusEffectData data)
        {
            Data = data;
        }

        public abstract void Dispose();
        public abstract void Affect(ISharedUnitData unitData);
        public abstract void Clear();
    }
}