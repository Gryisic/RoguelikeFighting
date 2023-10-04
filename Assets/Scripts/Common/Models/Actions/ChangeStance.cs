using Common.Models.Actions.Templates;
using Common.Units.Interfaces;

namespace Common.Models.Actions
{
    public class ChangeStance : ActionBase
    {
        private readonly IUnitInternalData _internalData;

        public ChangeStance(IUnitInternalData internalData, ActionTemplate data, ActionBase wrappedBase = null) : base(data, wrappedBase)
        {
            _internalData = internalData;
        }

        public override void Execute()
        {
            IncreaseAmountOnDecimalPercent(0.2f);
            
            _internalData.Animator.UpdateAnimatorController(data.AnimatorController);

            if (data.NextStanceTemplate != null) 
                UpdateTemplateData(data.NextStanceTemplate);

            wrappedBase?.Execute();
        }
    }
}