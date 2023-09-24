using System.Collections.Generic;
using Common.Gameplay.Modifiers.Templates;

namespace Common.Gameplay.Modifiers.Templates
{
    public class ModifierTemplatesSet
    {
        public IReadOnlyList<ModifierTemplate> Templates { get; }

        public ModifierTemplatesSet(IReadOnlyList<ModifierTemplate> templates)
        {
            Templates = templates;
        }
    }
}