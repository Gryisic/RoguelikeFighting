using System;
using Common.Gameplay.Interfaces;
using Common.Models.Projectiles.Interfaces;
using Common.Models.Projectiles.LaunchStrategies;
using Common.Units;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Projectiles
{
    public class Lightning : Projectile
    {
        [SerializeField] private float _time;
        [SerializeField] private ParticleSystem _particle;
        
        protected override async UniTask LaunchAsync(Vector2 from, Vector2 to)
        {
            Vector2 point = GetFloorPoint(to);
            Vector2 offset = new Vector2(0, hitMarkerParticle.main.startSizeY.constant / 2);
            
            hitMarkerParticle.transform.position = point + offset;
            hitMarkerParticle.gameObject.SetActive(true);
            hitMarkerParticle.Play();

            await UniTask.Delay(TimeSpan.FromSeconds(Constants.ProjectileDangerZoneTime), cancellationToken: launchTokenSource.Token);
            
            isCollided = false;

            gameObject.SetActive(true);
            _particle.gameObject.SetActive(true);
            _particle.Play();

            offset = new Vector2(0, _particle.main.startSizeMultiplier / 2);
            
            transform.position = point + offset;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_time), cancellationToken: launchTokenSource.Token);

            if (launchTokenSource.IsCancellationRequested)
                return;
            
            localRigidbody.velocity = Vector2.zero;
            isCollided = true;

            launchTokenSource.Dispose();
            _particle.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private Vector2 GetFloorPoint(Vector2 from)
        {
            RaycastHit2D hit = Physics2D.Raycast(from, Vector2.down, Mathf.Infinity, _floorMask);
            
            return hit.point;
        }
    }
}