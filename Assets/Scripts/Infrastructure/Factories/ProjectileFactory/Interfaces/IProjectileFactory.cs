using Common.Models.Projectiles;

namespace Infrastructure.Factories.ProjectileFactory.Interfaces
{
    public interface IProjectileFactory
    {
        void Load();
        Projectile Create();
    }
}