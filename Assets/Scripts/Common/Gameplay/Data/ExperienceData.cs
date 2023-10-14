using System;
using Common.Gameplay.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Data
{
    public class ExperienceData : IConcreteRunData
    {
        public int Amount { get; private set; }

        public event Action AmountOverflowed;
        public event Action<int> AmountChanged;

        public void Add(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to add negative amount of experience. Amount: {amount}");

            Amount += amount;
            
            if (Amount >= Constants.ExperienceNeededToRequestNextModifier)
            {
                Amount -= Constants.ExperienceNeededToRequestNextModifier;
                
                AmountOverflowed?.Invoke();
            }

            AmountChanged?.Invoke(Amount);
        }
        
        public void Remove(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to add negative amount of experience. Amount: {amount}");

            Amount += amount;
            
            if (Amount >= Constants.ExperienceNeededToRequestNextModifier)
            {
                Amount -= Constants.ExperienceNeededToRequestNextModifier;
                
                AmountOverflowed?.Invoke();
            }

            AmountChanged?.Invoke(Amount);
        }
        
        public void Clear()
        {
            
        }
    }
}