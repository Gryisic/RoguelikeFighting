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
        [SerializeField] protected ParticleSystem hitMarkerParticle;
        [SerializeField] protected LayerMask _floorMask;
        [SerializeField] protected bool deactivateOnCollide;
        
        protected CancellationTokenSource launchTokenSource;

        protected bool isCollided;

        private int _damage;
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

        public void Launch(Vector2 from, Vector2 to, int damage, Type ignoreCollisionWith)
        {
            _ignoredCollisionType = ignoreCollisionWith;
            _damage = damage;
            
            launchTokenSource = new CancellationTokenSource();
            
            LaunchAsync(from, to).Forget();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable) == false) 
                return;
            
            if (damageable.GetType() == _ignoredCollisionType)
                return;
                
            isCollided = true;
                
            damageable.TakeDamage(_damage);
        }

        protected abstract UniTask LaunchAsync(Vector2 from, Vector2 to);
    }
}