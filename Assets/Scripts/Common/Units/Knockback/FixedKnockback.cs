using System.Threading;
using Common.Units.Knockback.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units.Knockback
{
    public class FixedKnockback : IKnockbackStrategy
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Vector2 _position;

        public FixedKnockback(Rigidbody2D rigidbody, Vector2 position)
        {
            _rigidbody = rigidbody;
            _position = position;
        }

        public async UniTask ExecuteAsync(float time, CancellationToken token)
        {
            float timer = 0;
            
            while (timer < time && token.IsCancellationRequested == false)
            {
                _rigidbody.MovePosition(_position);
                
                _rigidbody.velocity = Vector2.zero;
                
                timer += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}