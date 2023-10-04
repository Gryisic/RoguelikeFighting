using System;
using System.Collections.Generic;
using Common.Gameplay.Modifiers;
using Common.Gameplay.Modifiers.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Data
{
    public class ModifiersData
    {
        private readonly ModifiersResolver _modifiersResolver;
        private readonly List<Modifier> _bufferedModifiers;

        private float _experience;
        
        public event Action<float> ExperienceChanged;
        public event Action<IReadOnlyList<ModifierTemplate>> ModifiersSelectionRequested;
        public event Action<Modifier> ModifierAdded; 

        public ModifiersData(ModifiersResolver modifiersResolver)
        {
            _modifiersResolver = modifiersResolver;

            _bufferedModifiers = new List<Modifier>();
        }
        
        public void Clear()
        {
            _experience = 0;
            _modifiersResolver.Clear();
            _bufferedModifiers.Clear();
        }
        
        public void AddExperience(float amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to add negative amount of experience. Amount: {amount}");

            _experience += amount;
            
            if (_experience >= Constants.ExperienceNeededToRequestNextModifier)
            {
                _experience -= Constants.ExperienceNeededToRequestNextModifier;
                
                RequestModifiersSelection();
            }
            
            ExperienceChanged?.Invoke(_experience);
        }

        public void GetModifierFromBuffer(int index)
        {
            Modifier modifier = _bufferedModifiers[index];

            _bufferedModifiers.RemoveAt(index);
            
            _modifiersResolver.AddModifierToHandler(modifier);
            _modifiersResolver.ReturnTemplatesToSet(_bufferedModifiers);
            
            ModifierAdded?.Invoke(modifier);
        }

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