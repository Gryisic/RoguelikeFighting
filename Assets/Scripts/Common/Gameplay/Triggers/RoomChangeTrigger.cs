using System;
using Common.Units;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    public abstract class RoomChangeTrigger : Trigger
    {
        public event Action<Enums.RoomType> Triggered;
        
        protected abstract Enums.RoomType RoomType { get; }

        public override void Execute() { }
        
        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (collidedWith.TryGetComponent(out Hero _) == false) 
                return;
            
            Triggered?.Invoke(RoomType);

            Deactivate();
        }
    }
}