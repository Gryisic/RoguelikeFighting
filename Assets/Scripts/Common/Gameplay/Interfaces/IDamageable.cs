using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        void ApplyKnockBack(Collider2D hitbox, Vector2 force, float time, Enums.Knockback knockback);
    }
}