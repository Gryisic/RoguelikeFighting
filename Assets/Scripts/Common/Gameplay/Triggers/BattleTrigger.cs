using System;
using Common.Units;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    public class BattleTrigger : Trigger
    {
        public event Action Triggered;

        public override void Execute() { }

        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (collidedWith.TryGetComponent(out Hero _) == false) 
                return;
            
            Triggered?.Invoke();

            Deactivate();
        }
    }
}