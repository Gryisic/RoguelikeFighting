using System;
using Common.Models.Actions.Templates;
using UnityEngine;

namespace Common.Models.Actions
{
    public abstract class ActionBase
    {
        protected readonly ActionBase wrappedBase;
        
        protected ActionTemplate data;
        
        protected ActionBase(ActionTemplate data, ActionBase wrappedBase = null)
        {
            this.data = data;
            this.wrappedBase = wrappedBase;
        }

        public abstract void Execute();

        protected void UpdateTemplateData(ActionTemplate data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Trying to set action template data that is null");
            
            this.data = data;
        }
        
        private void ThrowException(string message, float value) => 
            throw new InvalidOperationException($"Trying to {message} multiplier on negative value. Value: {value}");
    }
}