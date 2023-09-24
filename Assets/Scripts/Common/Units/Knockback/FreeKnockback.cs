using System.Threading;
using Common.Units.Knockback.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Knockback
{
    public class FreeKnockback : IKnockbackStrategy
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Vector2 _force;

        public FreeKnockback(Vector2 force, Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
            _force = force;
        }

        public async UniTask ExecuteAsync(float time, CancellationToken token)
        {
            float timer = 0;
            
            _rigidbody.velocity = _force;

            while (timer < time && token.IsCancellationRequested == false)
            {
                 if (_rigidbody.velocity.x == 0) 
                     return;
                
                 Vector2 currentVelocity = _rigidbody.velocity;
                 float slowdownSpeed = currentVelocity.x - Constants.LinearVelocitySlowdownSpeed * Time.fixedDeltaTime;
                 Vector2 velocity = new Vector2(slowdownSpeed, currentVelocity.y);

                 _rigidbody.velocity = velocity;

                 timer += Time.fixedDeltaTime;
                 await UniTask.WaitForFixedUpdate();
            }
        }
    }
}