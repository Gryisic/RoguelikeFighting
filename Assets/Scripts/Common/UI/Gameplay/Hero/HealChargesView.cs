using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Gameplay.Hero
{
    [Serializable]
    public class HealChargesView
    {
        [SerializeField] private HealChargeView[] _chargesView;

        private int _activeCharges;
        
        public void UpdateCharges(int amount)
        {
            if (amount > Constants.MaxHealCharges || amount < Constants.MinHealCharges)
                throw new ArgumentOutOfRangeException($"Amount of heal charges is greater or less than bounds. Amount: {amount}");

            if (amount > _activeCharges)
                ActivateCharges(amount);
            else
                DeactivateCharges(amount);

            _activeCharges = amount;
        }

        private void ActivateCharges(int amount)
        {
            for (int i = _activeCharges; i < amount; i++)
            {
                _chargesView[i].Activate();
            }
        }
        
        private void DeactivateCharges(int amount)
        {
            if (_activeCharges == 1)
            {
                _chargesView[0].Deactivate();
                
                return;
            }
            
            for (int i = _activeCharges - 1; i > amount; i--)
            {
                _chargesView[i].Deactivate();
            }
        }
    }
}