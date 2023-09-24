using System;
using System.Threading;
using Common.Gameplay.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Models.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class Projectile : MonoBehaviour, IDisposable
    {
        [SerializeField] protected Rigidbody2D localRigidbody;
        [SerializeField] protected Collider2D localCollider;
        
        protected CancellationTokenSource launchTokenSource;

        protected bool isCollided;

        private Type _ignoredCollisionType;

        private void Awake()
        {
            if (localRigidbody == null)
            {
                Debug.LogWarning($"Rigidbody of projectile(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                localRigidbody = GetComponent<Rigidbody2D>();
            }
            
            if (localCollider == null)
            {
                Debug.LogWarning($"Collider of projectile(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                localCollider = GetComponent<Collider2D>();
            }

            localCollider.isTrigger = true;
        }
        
        public void Dispose()
        {
            if(isCollided == false)
            {
                launchTokenSource?.Cancel();
                launchTokenSource?.Dispose();
            }
        }

        public void Launch(Vector2 from, Vector2 to, Type ignoreCollisionWith)
        {
            _ignoredCollisionType = ignoreCollisionWith;
            
            gameObject.SetActive(true);
            
            launchTokenSource = new CancellationTokenSource();
            
            LaunchAsync(from, to).Forget();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.GetType() == _ignoredCollisionType)
                    return;
            }

            isCollided = true;
            
            Debug.Log($"Collided with {other.transform.name}");

            localCollider.enabled = false;
        }

        protected abstract UniTask LaunchAsync(Vector2 from, Vector2 to);
    }
}