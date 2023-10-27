using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Common.UI.Gameplay.RunData
{
    public class GaldView : RunDataView
    {
        [SerializeField] private TextMeshProUGUI _amount;

        private int _currentAmount;
        
        public override void SetAmount(int amount)
        {
            DOVirtual.Int(_currentAmount, amount, 1f, v => _amount.text = v.ToString());

            _currentAmount = amount;
        }
    }
}