using System;
using Common.Gameplay.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Data
{
    public class GaldData : IConcreteRunData
    {
        public int Amount { get; private set; }

        public event Action<Enums.RunDataType, int> AmountChanged; 

        public GaldData()
        {
            Clear();
        }

        public bool HasEnoughAmount(int amount) => Amount >= amount;
        
        public void Increase(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to add negative amount of Gald. Amount: {amount}");

            Amount = Mathf.Min(Amount + amount, Constants.MaxAmountOfGald);
            
            AmountChanged?.Invoke(Enums.RunDataType.Gald, Amount);
        }

        public void Decrease(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Trying to remove negative amount of Gald. Amount: {amount}");

            Amount = Mathf.Max(Amount - amount, Constants.MinAmountOfGald);
            
            AmountChanged?.Invoke(Enums.RunDataType.Gald, Amount);
        }
        
        public void Clear()
        {
            Amount = Constants.DefaultAmountOfGald;
            
            AmountChanged?.Invoke(Enums.RunDataType.Gald, Amount);
        }
    }
}