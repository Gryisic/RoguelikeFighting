using System;
using Common.Models.Projectiles;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IActionsData : IDisposable
    {
        Collider2D HitBox { get; }
        Projectile Projectile { get; }
    }
}