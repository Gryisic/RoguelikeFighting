using Common.Models.Projectiles.Interfaces;
using Common.Models.Projectiles.LaunchStrategies;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Models.Projectiles
{
    public class Lightning : Projectile
    {
        [SerializeField] private float _time;
        [SerializeField] private AnimationCurve _trajectory;
        
        private ILaunchStrategy _launchStrategy;

        protected override async UniTask LaunchAsync(Vector2 from, Vector2 to)
        {
            if (_launchStrategy == null)
                _launchStrategy = new CurveStrategy(_trajectory, localRigidbody, _time);
            
            localCollider.enabled = true;
            isCollided = false;

            UniTask collideTask = UniTask.WaitUntil(() => isCollided, cancellationToken: launchTokenSource.Token);
            UniTask strategyTask = _launchStrategy.LaunchAsync(from, to, launchTokenSource.Token);

            await UniTask.WhenAny(collideTask, strategyTask);

            if (launchTokenSource.IsCancellationRequested)
                return;
            
            localRigidbody.velocity = Vector2.zero;
            isCollided = true;
            
            launchTokenSource.Dispose();
            gameObject.SetActive(false);
        }
    }
}