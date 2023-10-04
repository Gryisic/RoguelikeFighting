using System;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Gameplay
{
    [Serializable]
    public class EnemyHealthBar : HealthBar
    {
        [SerializeField] private SpriteRenderer _renderer;

        private int _healthValueReferenceID;

        public override void UpdateValue(int currentHealth, int maxHealth)
        {
            base.UpdateValue(currentHealth, maxHealth);
            
            if (_healthValueReferenceID == 0)
                _healthValueReferenceID = Shader.PropertyToID(Constants.ShaderHealthValueReference);

            float amount = (float)currentHealth / maxHealth;

            _renderer.material.SetFloat(_healthValueReferenceID, amount);
            
            Show();
        }

        private void Show() => DOTween.Sequence().Append(_renderer.DOFade(1, 0.8f)).AppendInterval(2f).AppendCallback(Hide);

        private void Hide() => _renderer.DOFade(0, 0.8f);
    }
}