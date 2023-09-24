using System;
using Common.Models.Projectiles;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Models.Actions
{
    [Serializable]
    public class ActionsData : IActionsData
    {
        [SerializeField] private Collider2D _hitBox;
        [SerializeField] private Projectile _projectile;
        
        public Collider2D HitBox => _hitBox;
        public Projectile Projectile => _projectile;

        public void Dispose()
        {
            if(_projectile != null)
                _projectile.Dispose();
        }
    }
}