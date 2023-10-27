using System;
using Common.Gameplay.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.Data
{
    public class HealData : IConcreteRunData
    {
        public int HealCharges { get; private set; }
        public bool CanHeal => HealCharges > 0;
        
        public event Action<Enums.RunDataType, int> ChargesUpdated;

        public HealData()
        {
            Clear();
        }

        public void UseCharge()
        {
            if (CanHeal == false)
                throw new InvalidOperationException($"Trying to heal while heal charges is less than 0. Amount {HealCharges}");

            HealCharges--;
            
            ChargesUpdated?.Invoke(Enums.RunDataType.Heal, HealCharges);
        }

        public void RestoreCharge()
        {
            if (HealCharges >= Constants.MaxHealCharges) 
                return;
            
            HealCharges++;
                
            ChargesUpdated?.Invoke(Enums.RunDataType.Heal, HealCharges);
        }
        
        public void Clear()
        {
            HealCharges = Constants.DefaultHealCharges;
            
            ChargesUpdated?.Invoke(Enums.RunDataType.Heal, HealCharges);
        }
    }
}