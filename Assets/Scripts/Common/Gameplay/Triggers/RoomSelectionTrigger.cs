using System;
using Common.Units;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    public class RoomSelectionTrigger : Trigger
    {
        private Enums.RoomType _type;
        
        public event Action<Enums.RoomType> Triggered;

        public override void Execute() => Triggered?.Invoke(_type);

        public override void Activate()
        {
            gameObject.SetActive(true);
            
            base.Activate();
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
            
            base.Deactivate();
        }

        public void SetTypeAndIndex(Enums.RoomType type) => _type = type;

        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (collidedWith.TryGetComponent(out Hero _))
                Debug.Log(_type);
        }
    }
}