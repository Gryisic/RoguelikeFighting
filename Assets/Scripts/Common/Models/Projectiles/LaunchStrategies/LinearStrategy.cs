using System;
using System.Threading;
using Common.Models.Projectiles.Interfaces;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Models.Projectiles.LaunchStrategies
{
    public class LinearStrategy : ILaunchStrategy
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly float _speed;

        public LinearStrategy(Rigidbody2D rigidbody, float speed)
        {
            _rigidbody = rigidbody;
            _speed = speed;
        }

        public async UniTask LaunchAsync(Vector2 from, Vector2 to, CancellationToken token)
        {
            float distance = (to - from).magnitude;
            float time = distance / _speed;
            float deltaTime = Time.fixedDeltaTime;
            float timer = 0;
            
            Vector2 direction = (to - from).Normalized(distance);
            
            _rigidbody.transform.position = from;
            
            while (timer < time && token.IsCancellationRequested == false)
            {
                if (token.IsCancellationRequested)
                    return;

                _rigidbody.velocity = direction * _speed;

                timer += deltaTime;
                await UniTask.WaitForFixedUpdate();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
        }
    }
}