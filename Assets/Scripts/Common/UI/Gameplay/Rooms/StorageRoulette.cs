using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Models.Items;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class StorageRoulette : UIElement
    {
        [SerializeField] private StorageRouletteItemView _centralView;
        [SerializeField] private StorageRouletteItemView[] _itemsView;
        [SerializeField] private Image _content;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        [SerializeField] private float _spinVelocity;

        private IReadOnlyList<StorageItemData> _storageItemsData;
        private StorageItemData _selectedItem;
        private CancellationTokenSource _spinTokenSource;

        private float _borderCoordinate;

        private float _leftBorder;
        private float _center;
        private float _rightBorder;

        private void OnDestroy()
        {
            _spinTokenSource?.Cancel();
            _spinTokenSource?.Dispose();
        }

        public override void Activate()
        {
            _borderCoordinate = _itemsView.Last().Transform.rect.xMax + _layoutGroup.spacing * (_itemsView.Length - 1);

            Rect rect = _content.rectTransform.rect;
            
            _leftBorder = rect.xMin;
            _center = rect.center.x;
            _rightBorder = rect.xMax;

            base.Activate();

            Spin();
        }

        public void SetData(IReadOnlyList<StorageItemData> storageItemsData, StorageItemData selectedItem)
        {
            _storageItemsData = storageItemsData;
            _selectedItem = selectedItem;
            
            FillViews();
        }
        
        public void Spin()
        {
            _spinTokenSource = new CancellationTokenSource();
            
            SpinAsync().Forget();
        }

        private void FillViews()
        {
            int index = 0;

            foreach (var view in _itemsView)
            {
                if (index >= _storageItemsData.Count)
                    index = 0;
                
                view.UpdateData(_storageItemsData[index]);

                index++;
            }
        }

        private async UniTask SpinAsync()
        {
            float timer = 0;
            float spinTime = Constants.StorageSpinTime / _spinVelocity;
            float delta = 0;
            float prewarmEasingTime = 0.75f;
            
            TweenerCore<Vector3, Vector3, VectorOptions> tween = Transform.DOMoveX(_rightBorder, Constants.StorageSpinPrewarmTime).From(_center).SetEase(Ease.InBack);
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.StorageSpinPrewarmTime * prewarmEasingTime), cancellationToken: _spinTokenSource.Token);
            
            tween.Kill();
            
            while (timer < Constants.StorageSpinTime && _spinTokenSource.IsCancellationRequested == false)
            {
                Transform.DOMoveX(_rightBorder, spinTime).From(_leftBorder).SetEase(Ease.Linear);
                
                await UniTask.Delay(TimeSpan.FromSeconds(spinTime), cancellationToken: _spinTokenSource.Token);

                timer += spinTime;

                delta = Constants.StorageSpinTime - timer;
                
                if (delta <= spinTime)
                    break;
            }

            _centralView.UpdateData(_selectedItem);
            
            Transform.DOMoveX(_center, delta).From(_leftBorder);

            await UniTask.Delay(TimeSpan.FromSeconds(delta), cancellationToken: _spinTokenSource.Token);
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.StorageSpinAfterimageTime / 2), cancellationToken: _spinTokenSource.Token);
            
            Deactivate();
        }
    }
}