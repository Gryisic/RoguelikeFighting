using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Gameplay.Modifiers.Templates
{
    public class ModifierTemplatesSet
    {
        private readonly List<ModifierTemplate> _nonBaseTemplates;
        private readonly List<ModifierTemplate> _baseTemplate;

        public ModifierTemplatesSet(IReadOnlyList<ModifierTemplate> templates)
        {
            _baseTemplate = templates.Where(t => t.IsBase).ToList();
            _nonBaseTemplates = templates.Where(t => t.IsBase == false).ToList();
        }

        public void ReturnTemplate(ModifierTemplate template)
        {
            if (template.IsBase)
                _baseTemplate.Add(template);
            else
                _nonBaseTemplates.Add(template);
        }

        public bool HasFreeTemplate() => _baseTemplate.Count > 0 || _nonBaseTemplates.Count > 0;

        public bool TryGetBaseTemplate(out ModifierTemplate template)
        {
            template = null;
            
            if (_baseTemplate.Count > 0)
            {
                template = _baseTemplate[0];
                _baseTemplate.Remove(template);

                return true;
            }

            return false;
        }

        public bool TryGetRandomNonBaseTemplate(out ModifierTemplate template)
        {
            template = null;
            
            if (_nonBaseTemplates.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(0, _nonBaseTemplates.Count);
                
                template = _nonBaseTemplates[index];
                _nonBaseTemplates.Remove(template);

                return true;
            }

            return false;
        }
    }
}