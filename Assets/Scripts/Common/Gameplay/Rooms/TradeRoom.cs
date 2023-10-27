using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Models.Items;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public class TradeRoom : DataModifierRoom
    {
        [SerializeField] private TradeItemData[] _items;
        [SerializeField] private MenuTrigger _menuTrigger;

        private Dictionary<int, TradeItemData> _itemsMap;
        private List<TradeItemData> _itemsToTrade;

        public event Action<int> ItemPurchased;
        public event Action<int> ItemNotPurchased;
        public event Action<IReadOnlyList<TradeItemData>> TradeRequested;
        
        public override Enums.RoomType Type => Enums.RoomType.Trade;

        public override void Initialize(IStageData stageData, IRunData runData)
        {
            _itemsMap = new Dictionary<int, TradeItemData>();
            
            base.Initialize(stageData, runData);
        }

        public override void Dispose()
        {
            TradeRequested = null;
            
            base.Dispose();
        }

        public override void Enter()
        {
            _menuTrigger.Triggered += OnMenuTriggerTriggered;
            ChangeTrigger.Deactivate();
            
            base.Enter();
        }

        public override void Exit()
        {
            _menuTrigger.Triggered -= OnMenuTriggerTriggered;
            
            _itemsToTrade = null;

            base.Exit();
        }

        public void ItemSelected(int index)
        {
            TradeItemData itemData = _itemsMap[index];
            
            if (itemData.Cost <= runData.Get<GaldData>(Enums.RunDataType.Gald).Amount)
            {
                ModifyData(itemData.Type, itemData);

                _itemsToTrade.Remove(itemData);
                
                ItemPurchased?.Invoke(index);
            }
            else
            {
                ItemNotPurchased?.Invoke(index);
            }
        }

        private List<TradeItemData> GetRandomItems()
        {
            int amount = _items.Length > Constants.MaxTradeItemsAmount ? Constants.MaxTradeItemsAmount : _items.Length;
            HashSet<TradeItemData> items = new HashSet<TradeItemData>();
            
            _itemsMap.Clear();

            int steps = 0;
            
            while (steps < Constants.SafeNumberOfStepsInLoops && items.Count < amount)
            {
                TradeItemData data = _items.Random();
                
                if (items.Add(data)) 
                    _itemsMap.Add(items.Count - 1, data);

                steps++;
            }
            
            return items.ToList();
        }

<<<<<<< Updated upstream
        private void OnMenuTriggerTriggered(Enums.MenuType type) => TradeRequested?.Invoke(GetRandomItems());
=======
        private void OnMenuTriggerTriggered(Enums.MenuType type)
        {
            animator.PlayNext();
            ChangeTrigger.Activate();
            CameraService.Shake();

            _itemsToTrade ??= GetRandomItems();

            TradeRequested?.Invoke(_itemsToTrade);
            ActivateExitParticles();
        }
>>>>>>> Stashed changes
    }
}