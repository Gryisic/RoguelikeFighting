using System;
using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers;
using Common.Gameplay.Modifiers.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Data
{
    public class ModifiersData : IConcreteRunData
    {
        private readonly ModifiersResolver _modifiersResolver;
        private readonly List<Modifier> _bufferedModifiers;
        
        public event Action<IReadOnlyList<ModifierTemplate>> ModifiersSelectionRequested;
        public event Action<Modifier> ModifierAdded; 

        public ModifiersData(ModifiersResolver modifiersResolver)
        {
            _modifiersResolver = modifiersResolver;

            _bufferedModifiers = new List<Modifier>();
        }
        
        public void Clear()
        {
            _modifiersResolver.Clear();
            _bufferedModifiers.Clear();
        }

        public void GetModifierFromBuffer(int index)
        {
            Modifier modifier = _bufferedModifiers[index];

            _bufferedModifiers.RemoveAt(index);
            
            _modifiersResolver.AddModifierToHandler(modifier);
            _modifiersResolver.ReturnTemplatesToSet(_bufferedModifiers);
            
            ModifierAdded?.Invoke(modifier);
        }

        public void OnExperienceOverflowed() => RequestModifiersSelection();

        private void RequestModifiersSelection()
        {
            List<ModifierTemplate> modifiers = new List<ModifierTemplate>();

            int step = 0;
            int count = 0;
            
            _bufferedModifiers.Clear();

            while (step < Constants.SafeNumberOfStepsInLoops && count < Constants.DefaultAmountOfModifiersToRequest && _modifiersResolver.HasFreeModifier())
            {
                if (_modifiersResolver.TryGetRandomModifier(out Modifier modifier))
                {
                    ModifierTemplate modifierTemplate = modifier.DefaultData;

                    _bufferedModifiers.Add(modifier);
                    
                    modifiers.Add(modifierTemplate);
                    count++;
                }
                
                step++;
            }
            
            if (modifiers.Count > 0) 
                ModifiersSelectionRequested?.Invoke(modifiers);
        }
    }
}