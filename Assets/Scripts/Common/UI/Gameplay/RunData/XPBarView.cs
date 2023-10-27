using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.RunData
{
    public class XPBarView : RunDataView
    {
        [SerializeField] private Image _bar;
        [SerializeField] private TextMeshProUGUI _percent;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private int _currentAmount;
        
        public override void SetAmount(int amount)
        {
            float floatAmount = (float)_currentAmount / 100;
            float fillAmount = (float)amount / 100;
            
            DOVirtual.Float(floatAmount, fillAmount, 1f, v => _bar.fillAmount = v);
            DOVirtual.Int(_currentAmount, amount, 1f, v => _percent.text = _stringBuilder.Clear().Append(v).Append("%").ToString());

            _currentAmount = amount;
        }
    }
}