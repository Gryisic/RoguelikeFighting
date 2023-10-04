using System;
using System.Text;
using Core.Extensions;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay
{
    [Serializable]
    public class CanvasHealthBar : HealthBar
    {
        [SerializeField] private Image _bar;
        [SerializeField] private Image _supBar;
        [SerializeField] private TextMeshProUGUI _textValues;

        [SerializeField] private Color _increaseColor;
        [SerializeField] private Color _decreaseColor;
        
        private StringBuilder _stringBuilder = new StringBuilder();

        private float _lastAmount = Constants.MaxStatValue;

        public override void UpdateValue(int currentHealth, int maxHealth)
        {
            base.UpdateValue(currentHealth, maxHealth);
            
            maxHealth = Mathf.Min(maxHealth, Constants.MaxStatValue);
            currentHealth = Mathf.Max(Constants.MinStatValue, currentHealth);
            
            SetTextView(currentHealth, maxHealth);
            SetImageView(currentHealth, maxHealth);
        }

        private void SetTextView(int currentHealth, int maxHealth)
        {
            _stringBuilder.Clear();
            
            _stringBuilder.Append(currentHealth).Append("/").StartOpacity(66).Append(maxHealth).StartSupString().Append("HP");

            _textValues.text = _stringBuilder.ToString();
        }
        
        private void SetImageView(int currentHealth, int maxHealth)
        {
            float amount = (float)currentHealth / maxHealth;

            if (amount > _lastAmount)
                IncreaseAmount(amount);
            else
                DecreaseAmount(amount);

            _lastAmount = amount;
        }

        private void IncreaseAmount(float amount)
        {
            _bar.color = _increaseColor;
            _bar.fillAmount = amount;
            
            DOTween.To(() => _supBar.fillAmount, v => _supBar.fillAmount = v, amount, 0.5f).SetEase(Ease.InQuint);
        }
        
        private void DecreaseAmount(float amount)
        {
            _bar.color = _decreaseColor;
            _supBar.fillAmount = amount;
            
            DOTween.To(() => _bar.fillAmount, v => _bar.fillAmount = v, amount, 0.5f).SetEase(Ease.InQuint);
        }
    }
}