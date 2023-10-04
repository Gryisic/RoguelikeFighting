using System;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Trigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        
        private void Awake()
        {
            if (_collider == null)
            {
                Debug.LogWarning($"Collider of {name}'s trigger is not assigned");

                _collider = GetComponent<BoxCollider2D>();
            }

            _collider.isTrigger = true;
        }

        public abstract void Execute();

        public virtual void Activate()
        {
            _collider.enabled = true;
        }
        
        public virtual void Deactivate()
        {
            _collider.enabled = false;
        }
    }
}