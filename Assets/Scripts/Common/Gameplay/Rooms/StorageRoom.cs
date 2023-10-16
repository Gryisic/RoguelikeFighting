using System;
using System.Collections.Generic;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Models.Items;
using Common.Scene.Cameras.Interfaces;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Gameplay.Rooms
{
    public class StorageRoom : DataModifierRoom
    {
        [SerializeField] private StorageItemData _emptyItem;
        [SerializeField] private StorageItemData[] _itemsData;
        [SerializeField] private MenuTrigger _menuTrigger;

        private Dictionary<int, ItemsPack> _rangeItemMap;

        private CancellationTokenSource _awaitTokenSource;
        
        public event Action<IReadOnlyList<StorageItemData>, StorageItemData> StorageSpinRequested;

        public override Enums.RoomType Type => Enums.RoomType.Storage;

        public override void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService)
        {
            _rangeItemMap = new Dictionary<int, ItemsPack>();
            
            InitializePacks();
            
            base.Initialize(stageData, runData, cameraService);
        }

        public override void Dispose()
        {
            _awaitTokenSource?.Cancel();
            _awaitTokenSource?.Dispose();
            
            StorageSpinRequested = null;
        }

        public override void Enter()
        {
            _menuTrigger.Triggered += OnMenuTriggerTriggered;

            CameraService.FocusOn(cameraFocusPoint);

            base.Enter();
        }
        
        public override void Exit()
        {
            _menuTrigger.Triggered -= OnMenuTriggerTriggered;

            base.Exit();
        }

        private void InitializePacks()
        {
            foreach (var data in _itemsData)
            {
                int chance = RoundedChance(data.DropChance);
                
                if (_rangeItemMap.TryGetValue(chance, out ItemsPack pack))
                {
                    pack.Add(data);
                }
                else
                {
                    ItemsPack newPack = new ItemsPack();
                    
                    _rangeItemMap.Add(chance, newPack);
                    newPack.Add(data);
                }
            }
        }
        
        private void OnMenuTriggerTriggered(Enums.MenuType type)
        {
            animator.PlayNext();

            _awaitTokenSource = new CancellationTokenSource();
            
            AwaitAsync().Forget();
            
            StorageSpinRequested?.Invoke(GetItems(), GetRandomItem());
        }

        private IReadOnlyList<StorageItemData> GetItems()
        {
            List<StorageItemData> items = new List<StorageItemData>();

            foreach (var pack in _rangeItemMap.Values)
            {
                items.AddRange(pack.Items);
            }

            return items;
        }

        private StorageItemData GetRandomItem()
        {
            int step = 0;
            StorageItemData item = null;
            
            while (step < Constants.SafeNumberOfStepsInLoops)
            {
                int initialChance = Random.Range(0, 101);
                int roundedChance = RoundedChance(initialChance);

                if (_rangeItemMap.TryGetValue(roundedChance, out ItemsPack pack))
                {
                    item = pack.GetRandomItemAndChangeItToEmpty(_emptyItem);

                    break;
                }
                
                step++;
            }

            return item;
        }
        
        private int RoundedChance(int initialChance)
        {
            int intValue = initialChance / 10;
            int fracValue = initialChance % 10;

            if (fracValue == 5)
                return intValue + 5;
            
            if (fracValue > 5)
                return intValue + 10;
            
            return intValue;
        }

        private async UniTask AwaitAsync()
        {
            float delay = Constants.StorageSpinPrewarmTime + Constants.StorageSpinTime;

            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _awaitTokenSource.Token);
            
            animator.PlayNext();

            delay = Constants.StorageSpinAfterimageTime;
            
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _awaitTokenSource.Token);
            
            animator.PlayNext();
            ChangeTrigger.Activate();
        }
        
        private class ItemsPack
        {
            private readonly List<StorageItemData> _items;

            public IReadOnlyList<StorageItemData> Items => _items;

            public ItemsPack()
            {
                _items = new List<StorageItemData>();
            }

            public void Add(StorageItemData item) => _items.Add(item);

            public StorageItemData GetRandomItemAndChangeItToEmpty(StorageItemData emptyItem)
            {
                StorageItemData item = _items.Random();
                int index = _items.IndexOf(item);

                _items[index] = emptyItem;
                
                return item;
            }
        }
    }
}