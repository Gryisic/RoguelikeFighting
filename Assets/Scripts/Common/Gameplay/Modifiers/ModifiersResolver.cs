using System;
using Common.Gameplay.Interfaces;
using ModifierType = Infrastructure.Utils.Enums.Modifier;

namespace Common.Gameplay.Modifiers
{
    public class ModifiersResolver
    {
        private readonly IRunData _runData;
        private readonly ModifiersDataBase _modifiersDataBase;
        private readonly ModifiersHandler _modifiersHandler;

        public ModifiersResolver(IRunData runData, ModifiersDataBase modifiersDataBase, ModifiersHandler modifiersHandler)
        {
            _runData = runData;
            _modifiersDataBase = modifiersDataBase;
            _modifiersHandler = modifiersHandler;
        }

        public Modifier GetModifier(ModifierType type)
        {
            var modifier = DefineModifier(type);
            
            _modifiersHandler.AddModifier(type, modifier);
            
            return modifier;
        }
        
        private Modifier DefineModifier(ModifierType type)
        {
            switch (type)
            {
                case ModifierType.RingOfFire:
                    return new RingOfFireModifier(_modifiersDataBase.GetTemplatesSetByType(type).Templates[0]);

                case ModifierType.Freeze:
                    return new FreezeModifier(_modifiersDataBase.GetTemplatesSetByType(type).Templates[0]);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}