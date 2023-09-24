using System;
using System.Collections.Generic;
using Common.Gameplay.Modifiers.Extensions;
using Common.Gameplay.Modifiers.Templates;
using Infrastructure.Utils;
using ModifierType = Infrastructure.Utils.Enums.Modifier;

namespace Common.Gameplay.Modifiers
{
    public class ModifiersHandler : IDisposable
    {
        private readonly Dictionary<ModifierType, Modifier> _modifiers;
        
        public ModifiersHandler()
        {
            _modifiers = new Dictionary<ModifierType, Modifier>();
        }
        
        public void Dispose()
        {
            foreach (var modifier in _modifiers.Values) 
                modifier.Dispose();
        }

        public void Clear() => _modifiers.Clear();

        public void AddModifier(ModifierType type, Modifier modifier)
        {
            if (_modifiers.ContainsKey(type))
            {
                _modifiers[type] = modifier;

                return;
            }
            
            _modifiers.Add(type, modifier);
        }
        
        public bool TryGetModifierByType(ModifierType type, out Modifier modifier)
        {
            modifier = null;
            
            if (_modifiers.TryGetValue(type, out Modifier concreteModifier))
            {
                modifier = concreteModifier;

                return true;
            }

            return false;
        }
    }
}