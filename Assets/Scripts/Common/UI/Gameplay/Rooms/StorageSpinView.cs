using System;
using System.Collections.Generic;
using Common.Models.Items;
using UnityEngine;

namespace Common.UI.Gameplay.Rooms
{
    public class StorageSpinView : UIElement
    {
        [SerializeField] private StorageCard _card;
        [SerializeField] private StorageRoulette _roulette;
        [SerializeField] private StorageRoulette _subRoulette;

        public event Action SpinEnded;

        public override void Activate()
        {
            base.Activate();
            
            _card.Ended += OnEnded;
            
            _card.Activate();
            _roulette.Activate();
            _subRoulette.Activate();
        }

        public override void Deactivate()
        {
            _card.Ended -= OnEnded;
            
            _card.Deactivate();
            _roulette.Deactivate();
            _subRoulette.Deactivate();
            
            base.Deactivate();
        }

        public void SetData(IReadOnlyList<StorageItemData> storageItemsData, StorageItemData selectedItem)
        {
            _card.SetData(selectedItem);
            _roulette.SetData(storageItemsData, selectedItem);
            _subRoulette.SetData(storageItemsData, selectedItem);
        }
        
        private void OnEnded()
        {
            SpinEnded?.Invoke();
        }
    }
}