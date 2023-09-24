using System;
using UnityEngine;

namespace Common.Models.Actions
{
    public abstract class ActionBase
    {
        protected readonly ActionBase wrappedBase;
        
        protected ActionTemplate data;

        protected float AffectMultiplier { get; private set; } = 1;

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
        
        protected void IncreaseAmountOnDecimalPercent(float percent)
        {
            percent = ValidatedValue(percent, "increase");

            AffectMultiplier *= percent;
        }
        
        protected void DecreaseAmountOnDecimalPercent(float percent)
        {
            percent = ValidatedValue(percent, "decrease");

            AffectMultiplier *= percent;
        }

        private void ThrowException(string message, float value) => 
            throw new InvalidOperationException($"Trying to {message} multiplier on negative value. Value: {value}");
        
        private float ValidatedValue(float value, string exceptionMessage)
        {
            if (value < 0)
                ThrowException("decrease", value);
            
            if (value > 100)
            {
                Debug.LogWarning($"Trying to increase multiplier on value that exceeds limit. Value will be divided by 100. Value: {value}");

                value /= 100;
            }

            return value;
        }
    }
}