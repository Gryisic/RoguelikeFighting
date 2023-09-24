using Common.Gameplay.Modifiers;
using Common.Units.Interfaces;

namespace Common.Models.Actions
{
    public class ModifierAction : ActionBase
    {
        private readonly IUnitInternalData _internalData;
        private readonly Modifier _modifier;

        public ModifierAction(IUnitInternalData internalData, Modifier modifier, ActionTemplate data, ActionBase wrappedBase = null) : base(data, wrappedBase)
        {
            _internalData = internalData;
            _modifier = modifier;
        }

        public override void Execute()
        {
            _modifier.Execute(_internalData);
            
            wrappedBase?.Execute();
        }
    }
}