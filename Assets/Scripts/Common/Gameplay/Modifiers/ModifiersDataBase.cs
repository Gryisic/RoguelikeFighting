using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Modifiers.Templates;
using UnityEngine;

using ModifierType = Infrastructure.Utils.Enums.Modifier;

namespace Common.Gameplay.Modifiers
{
    [Serializable]
    public class ModifiersDataBase
    {
        [SerializeField] private List<ModifierTemplate> _templates;

        private Dictionary<ModifierType, ModifierTemplatesSet> _typeSetMap;

        public void Initialize()
        {
            _typeSetMap = new Dictionary<ModifierType, ModifierTemplatesSet>();

            Array types = Enum.GetValues(typeof(ModifierType));
            
            foreach (ModifierType type in types)
            {
                List<ModifierTemplate> templates = _templates.Where(t => t.Type == type).Distinct().ToList();

                _typeSetMap.Add(type, new ModifierTemplatesSet(templates));
            }
        }
        
        public ModifierTemplatesSet GetTemplatesSetByType(ModifierType type)
        {
            if (_typeSetMap.ContainsKey(type) == false)
                throw new NullReferenceException($"Modifier of type '{type}' is not presented in modifiers database");

            return _typeSetMap[type];
        }
    }
}