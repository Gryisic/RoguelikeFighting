using System;
using System.Collections.Generic;
using Common.Models.StatusEffects.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.StatusEffects
{
    public class StatusEffectsHandler : IStatusEffectsHandler
    {
        private readonly Dictionary<Enums.StatusEffect,StatusEffect> _effects;

        public StatusEffectsHandler()
        {
            _effects = new Dictionary<Enums.StatusEffect,StatusEffect>();
        }
        
        public void Dispose()
        {
            foreach (var effect in _effects.Values) 
                effect.Dispose();
        }
        
        public void AddEffect(StatusEffect effect)
        {
            if (effect == null)
                throw new NullReferenceException("Trying to add status effect that is null");

            if (_effects.TryGetValue(effect.Data.StatusEffect, out StatusEffect oldEffect))
            {
                oldEffect.Clear();
                _effects[effect.Data.StatusEffect] = effect;
            }
            else
            {
                _effects.Add(effect.Data.StatusEffect, effect);
            }
        }

        public bool RemoveEffect(StatusEffect effect) => _effects.Remove(effect.Data.StatusEffect);
        
        public void Clear()
        {
            foreach (var effect in _effects.Values)
                effect.Clear();
            
            _effects.Clear();
        }
    }
}