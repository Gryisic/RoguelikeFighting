using System;
using System.Collections.Generic;
using Infrastructure.Factories.ProjectileFactory.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Projectiles
{
    public class ProjectilePool : IDisposable
    {
        private readonly IProjectileFactory _projectileFactory;
        private readonly Queue<Projectile> _projectiles;

        public ProjectilePool(IProjectileFactory projectileFactory)
        {
            _projectileFactory = projectileFactory;

            _projectiles = new Queue<Projectile>();
            
            GenerateInitialProjectiles();
        }

        public void Dispose()
        {
            for (int i = 0; i < _projectiles.Count; i++) 
                _projectiles.Dequeue().Dispose();
        }
        
        public Projectile Get()
        {
            if (_projectiles.Count <= 0) 
                CreateProjectile();

            return _projectiles.Dequeue();
        }

        public void Return(Projectile projectile) => _projectiles.Enqueue(projectile);

        private void GenerateInitialProjectiles()
        {
            for (int i = 0; i < Constants.InitialCopiesOfProjectiles; i++) 
                CreateProjectile();
        }

        private void CreateProjectile()
        {
            Projectile projectile = _projectileFactory.Create();
                
            _projectiles.Enqueue(projectile);
        }
    }
}