using System;
using Common.Units;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    public class RoomSelectionTrigger : Trigger
    {
        private Enums.RoomType _type;
        private int _index;

        public event Action<int> HeroEntered;
        public event Action<int> HeroExited;
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

        public void SetIndexAndType(int index, Enums.RoomType type)
        {
            _index = index;
            _type = type;
        }

        public void SetPosition(Vector2 position) => transform.position = position;

        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (collidedWith.TryGetComponent(out Hero _))
                HeroEntered?.Invoke(_index);
        }
        
        private void OnTriggerExit2D(Collider2D collidedWith)
        {
            if (collidedWith.TryGetComponent(out Hero _))
                HeroExited?.Invoke(_index);
        }
    }
}