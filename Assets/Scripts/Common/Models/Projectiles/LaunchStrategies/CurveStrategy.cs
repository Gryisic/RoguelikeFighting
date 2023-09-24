using System.Linq;
using System.Threading;
using Common.Models.Projectiles.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Models.Projectiles.LaunchStrategies
{
    public class CurveStrategy : ILaunchStrategy
    {
        private readonly Rigidbody2D _localRigidbody;
        private readonly AnimationCurve _trajectory;
        private readonly float _time;
        private readonly float _fixedDeltaTime;

        public CurveStrategy(AnimationCurve trajectory, Rigidbody2D localRigidbody, float time)
        {
            _trajectory = trajectory;
            _localRigidbody = localRigidbody;
            _time = time;

            _fixedDeltaTime = Time.fixedDeltaTime;
        }

        public async UniTask LaunchAsync(Vector2 from, Vector2 to, CancellationToken token)
        {
            float timer = 0;

            _localRigidbody.transform.position = from;
            
            while (timer < _time && token.IsCancellationRequested == false)
            {
                float normalizedTime = timer / _time;
                float evaluatedValue = _trajectory.Evaluate(normalizedTime);
                float height = Mathf.Lerp(0, 3, evaluatedValue);

                Vector2 pos = Vector2.Lerp(from, to, normalizedTime) + new Vector2(0, height);
                
                _localRigidbody.MovePosition(pos);
                
                timer += _fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}