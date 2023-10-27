using System;
using Common.Gameplay.Modifiers.Templates;
using Common.Models.StatusEffects;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Modifiers
{
    public abstract class Modifier
    {
        protected readonly ModifierTemplate data;
        protected readonly Modifier wrappedModifier;
        
        public ModifierTemplate DefaultData => data;

        protected Modifier(ModifierTemplate data, Modifier wrappedModifier = null)
        {
            this.data = data;
            this.wrappedModifier = wrappedModifier;
        }

        public T GetData<T>() where T : ModifierTemplate => GetDataInternal<T>();
        
        public abstract void Execute(IUnitInternalData internalData);

        public abstract void Reset();

        protected abstract T GetDataInternal<T>() where T: ModifierTemplate;

        protected bool TryDefineStatusEffect(out StatusEffect effect)
        {
            effect = null;

            switch (data.StatusEffectTemplate.StatusEffect)
            {
                case Enums.StatusEffect.None:
                    return false;
                
                case Enums.StatusEffect.Freeze:
                    break;
                
                case Enums.StatusEffect.Burn:
                    effect = new BurnEffect(GetStatusEffectData());
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.StatusEffectTemplate.StatusEffect), data.StatusEffectTemplate.StatusEffect, null);
            }

            return true;
        }
        
        private IStatusEffectData GetStatusEffectData()
        {
            IStatusEffectData statusEffectData = null;
            
            if (wrappedModifier != null) 
                statusEffectData = wrappedModifier.GetStatusEffectData();
            
            statusEffectData = statusEffectData == null ? new StatusEffectData(data.StatusEffectTemplate) : new StatusEffectData(data.StatusEffectTemplate, statusEffectData);

            return statusEffectData;
        }
    }
}