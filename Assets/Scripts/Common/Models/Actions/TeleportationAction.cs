using System;
using Common.Models.Actions.Templates;

namespace Common.Models.Actions
{
    public class TeleportationAction : ActionBase
    {
        private TeleportationActionTemplate TeleportationData => data as TeleportationActionTemplate;
        
        public TeleportationAction(ActionTemplate data, ActionBase wrappedBase = null) : base(data, wrappedBase)
        {
            if (data is TeleportationActionTemplate == false)
                ThrowInvalidTemplateException(nameof(TeleportationAction));
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}