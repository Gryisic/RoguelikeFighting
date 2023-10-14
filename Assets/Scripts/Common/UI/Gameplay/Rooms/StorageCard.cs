using System;
using Common.Models.Items;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class StorageCard : UIElement
    {
        [SerializeField] private Image _card;
        [SerializeField] private Image _border1;
        [SerializeField] private Image _border2;

        [SerializeField] private Sprite _filledIcon;
        [SerializeField] private Sprite _borderIcon;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _description;

        [SerializeField] private float _punchSpeed;
        [SerializeField] private Vector3 _punchVector;
        
        public event Action Ended;

        public override void Activate()
        {
            base.Activate();

            Spin();
        }

        public void SetData(StorageItemData data)
        {
            _icon.sprite = data.Icon;
            _description.text = data.Description;
        }
        
        public void Spin()
        {
            int loops = Mathf.FloorToInt(Constants.StorageSpinTime / _punchSpeed);
            
            _border1.gameObject.SetActive(false);
            _border2.gameObject.SetActive(false);
            _icon.gameObject.SetActive(false);
            _description.gameObject.SetActive(false);

            _card.sprite = _borderIcon;

            DOTween.Sequence()
                .AppendInterval(Constants.StorageSpinPrewarmTime)
                .Append(_card.rectTransform.DOPunchScale(_punchVector, _punchSpeed).SetLoops(loops))
                .Append(_card.DOFade(0f, 0f))
                .AppendInterval(0.5f)
                .AppendCallback(() => _card.sprite = _filledIcon)
                .Append(_card.DOFade(1f, 0f))
                .AppendCallback(() => _icon.gameObject.SetActive(true))
                .AppendCallback(() => _description.gameObject.SetActive(true))
                .AppendCallback(() => _border1.gameObject.SetActive(true))
                .AppendCallback(() => _border2.gameObject.SetActive(true))
                .Append(_border1.rectTransform.DOScale(0f, 0f))
                .Append(_border2.rectTransform.DOScale(0f, 0f))
                .Append(_border1.rectTransform.DOScale(1f, 0.2f).From(0.8f))
                .Append(_border2.rectTransform.DOScale(1f, 0.2f).From(0.8f))
                .AppendInterval(Constants.StorageSpinAfterimageTime)
                .AppendCallback(() => Ended?.Invoke());
        }
    }
}