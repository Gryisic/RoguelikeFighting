using System;
using System.Linq;
using System.Threading;
using Common.Units.Knockback;
using Common.Units.Knockback.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    public class UnitPhysics : IDisposable
    {
        private readonly Vector2 _gravityVector;
        private readonly Rigidbody2D _rigidbody;
        private readonly BoxCollider2D _collider;

        private CancellationTokenSource _updateTokenSource;
        private CancellationTokenSource _dropTokenSource;
        private CancellationTokenSource _knockbackTokenSource;

        private IKnockbackStrategy _knockback;
        
        private bool _isUpdating;
        private bool _isDropping;
        private bool _isKnockbacking;
        private bool _isFrozen;
        private bool _isVelocityChangeSuppressed;
        
        public bool IsGrounded => CheckGround(out Collider2D _) && VerticalVelocity == 0;
        public float VerticalVelocity => _rigidbody.velocity.y;
        
        public UnitPhysics(Rigidbody2D rigidbody, BoxCollider2D collider) 
        {
            _rigidbody = rigidbody;
            _gravityVector = new Vector2(0, -Physics2D.gravity.y);
            _collider = collider;
        }
        
        public void Dispose()
        {
            if (_isUpdating)
                StopUpdating();
            
            if (_isDropping)
            {
                _dropTokenSource.Cancel();
                _dropTokenSource.Dispose();
            }

            if (_isKnockbacking)
            {
                _knockbackTokenSource.Cancel();
                _knockbackTokenSource.Dispose();
            }
        }
        
        public void StartUpdating()
        {
            _isUpdating = true;
            
            _updateTokenSource = new CancellationTokenSource();
            
            UpdateAsync().Forget();
        }

        public void StopUpdating()
        {
            _updateTokenSource.Cancel();
            _updateTokenSource.Dispose();
            
            _isUpdating = false;
        }

        public void FreezeFalling()
        {
            _isFrozen = true;
            
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }

        public void UnfreezeFalling()
        {
            if (_isFrozen == false)
                return;
            
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.velocity = Vector2.down;

            _isFrozen = false;
        }

        public void SuppressManualVelocityChange() => _isVelocityChangeSuppressed = true;
        
        public void UnSuppressManualVelocityChange() => _isVelocityChangeSuppressed = false;

        public void UpdateVelocity(Vector2 velocityVector)
        {
            if (_isVelocityChangeSuppressed)
                return;
            
            _rigidbody.velocity = velocityVector;
        }
        
        public void UpdateHorizontalVelocity(float velocity)
        {
            if (velocity == 0 && _rigidbody.velocity.y != 0 || _isVelocityChangeSuppressed)
                return;
            
            _rigidbody.velocity = new Vector2(velocity, _rigidbody.velocity.y);
        }

        public void AddForce(Vector2 force)
        {
            _rigidbody.velocity += force;
        }
        
        public void AddVerticalForce(Vector2 force)
        {
            Vector2 velocity = new Vector2(_rigidbody.velocity.x, 0);

            _rigidbody.velocity = velocity;
            _rigidbody.velocity += force;
        }
        
        public void AddKnockback(Collider2D hitbox, Vector2 force, float time, Enums.Knockback knockback)
        {
            if (_isKnockbacking)
            {
                _knockbackTokenSource.Cancel();
                _knockbackTokenSource.Dispose();
            }
            
            _knockbackTokenSource = new CancellationTokenSource();
            
            KnockbackAsync(hitbox, force, time, knockback).Forget();
        }

        public void DropThroughPlatform()
        {
            _dropTokenSource = new CancellationTokenSource();
            
            DropAsync().Forget();
        }

        private bool CheckGround(out Collider2D collider)
        {
            float distance = 0.1f;
            Bounds bounds = _collider.bounds;
            RaycastHit2D[] results = new RaycastHit2D[2];
            
            Physics2D.BoxCastNonAlloc(bounds.center, bounds.size, 0f, Vector2.down, results, distance);

            results = results.Skip(1).ToArray();
            collider = results[0].collider;
            
            return results[0].collider != null;
        }
        
        private IKnockbackStrategy DefineKnockback(Collider2D hitbox, Vector2 force, Enums.Knockback knockback)
        {
            switch (knockback)
            {
                case Enums.Knockback.Fixed:
                    return new FixedKnockback(_rigidbody, hitbox.bounds.center);
                
                case Enums.Knockback.Free:
                    return new FreeKnockback(force, _rigidbody);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(knockback), knockback, null);
            }
        }

        private async UniTask KnockbackAsync(Collider2D hitBox, Vector2 force, float time, Enums.Knockback knockback)
        {
            _isKnockbacking = true;
            
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(force);

            _knockback = DefineKnockback(hitBox, force, knockback);

            await _knockback.ExecuteAsync(time, _knockbackTokenSource.Token);
            
            _knockbackTokenSource.Dispose();
            _isKnockbacking = false;
        }
        
        private async UniTask DropAsync()
        {
            bool isOnPlatform = CheckGround(out Collider2D collider) && collider.TryGetComponent(out PlatformEffector2D _);
            
            if (isOnPlatform == false)
                return;
            
            _isDropping = true;
            _collider.enabled = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(Constants.DropThroughPlatformTime), cancellationToken: _dropTokenSource.Token);
            
            _dropTokenSource.Dispose();
            
            _collider.enabled = true;
            _isDropping = false;
        }
        
        private async UniTask UpdateAsync() 
        {
            while (_updateTokenSource.IsCancellationRequested == false) 
            {
                if (_rigidbody.velocity.y < 0)
                {
                    _rigidbody.velocity -= _gravityVector * Constants.FallMultiplier * Time.fixedDeltaTime;
                }
                else if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity *= Constants.LinearVelocitySlowdownSpeed;
                }

                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}