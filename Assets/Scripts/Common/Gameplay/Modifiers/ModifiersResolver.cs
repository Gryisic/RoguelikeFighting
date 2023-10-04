using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Modifiers.Templates;
using UnityEngine;
using ModifierType = Infrastructure.Utils.Enums.Modifier;
using Random = System.Random;

namespace Common.Gameplay.Modifiers
{
    public class ModifiersResolver
    {
        private readonly ModifiersDataBase _modifiersDataBase;
        private readonly ModifiersHandler _modifiersHandler;

        private readonly Array _modifierTypes;

        public ModifiersResolver(ModifiersDataBase modifiersDataBase, ModifiersHandler modifiersHandler)
        {
            _modifiersDataBase = modifiersDataBase;
            _modifiersHandler = modifiersHandler;
            
            _modifierTypes = Enum.GetValues(typeof(ModifierType));
        }

        public void Clear() => _modifiersHandler.Clear();

        public void AddModifierToHandler(Modifier modifier) => _modifiersHandler.AddModifier(modifier);

        public void ReturnTemplatesToSet(IReadOnlyList<Modifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                ModifierTemplate template = modifier.DefaultData;
                ModifierTemplatesSet set = _modifiersDataBase.GetTemplatesSetByType(template.Type);
                
                set.ReturnTemplate(template);
            }
        }
        
        public bool HasFreeModifier()
        {
            int freeSets = (from ModifierType type in _modifierTypes select _modifiersDataBase.GetTemplatesSetByType(type)).Count(set => set.HasFreeTemplate());
        
            return freeSets > 0;
        }
        
        public bool TryGetRandomModifier(out Modifier modifier)
        {
            int index = UnityEngine.Random.Range(0, _modifierTypes.Length);
            ModifierType type = (ModifierType)_modifierTypes.GetValue(index);
            
            modifier = null;

            return TryDefineModifier(type, out modifier);
        }
        
        // public Modifier GetModifierOfType(ModifierType type)
        // {
        //     Modifier modifier = TryDefineModifier(type);
        //     
        //     _modifiersHandler.AddModifier(type, modifier);
        //     
        //     return modifier;
        // }
        
        private bool TryDefineModifier(ModifierType type, out Modifier modifier)
        {
            ModifierTemplatesSet set = _modifiersDataBase.GetTemplatesSetByType(type);
            ModifierTemplate template;

            modifier = null;

            if (_modifiersHandler.HasModifierOfType(type) && set.TryGetRandomNonBaseTemplate(out template))
            {
                modifier = SelectModifier(type, template);

                return true;
            }

            if (_modifiersHandler.HasModifierOfType(type) == false && set.TryGetBaseTemplate(out template))
            {
                modifier = SelectModifier(type, template);

                return true;
            }

            return false;
        }

        private Modifier SelectModifier(ModifierType type, ModifierTemplate template)
        {
            switch (type)
            {
                case ModifierType.RingOfFire:
                    return new RingOfFireModifier(template);

                case ModifierType.Freeze:
                    return new FreezeModifier(template);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}