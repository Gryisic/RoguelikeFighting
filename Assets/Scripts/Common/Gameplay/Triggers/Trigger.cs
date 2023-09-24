using UnityEngine;

namespace Common.Gameplay.Triggers
{
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Trigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private bool _isOneShot;

        protected bool IsOneShot => _isOneShot;
        
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

        protected void Deactivate()
        {
            _collider.enabled = false;
        }
    }
}