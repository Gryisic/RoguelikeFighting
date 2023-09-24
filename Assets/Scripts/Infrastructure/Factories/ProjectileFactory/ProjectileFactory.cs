using System;
using Common.Models.Projectiles;
using Infrastructure.Factories.ProjectileFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Factories.ProjectileFactory
{
    [Serializable]
    public class ProjectileFactory : IProjectileFactory
    {
        [SerializeField] private Transform _root;
        
        private Projectile _prefab;
        
        public void Load()
        {
            _prefab = Resources.Load<Projectile>(Constants.PathToProjectilesPrefabs);

            if (_prefab == null)
                throw new NullReferenceException("Projectile prefab isn't founded");
        }

        public Projectile Create() => Object.Instantiate(_prefab, _root);
    }
}