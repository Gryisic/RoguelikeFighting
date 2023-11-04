using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Actions
{
    public class MeleeAttackAction : ActionBase
    {
        private readonly IUnitInternalData _internalData;

        public MeleeAttackAction(IUnitInternalData internalData, ActionTemplate template, ActionBase wrappedBase = null) : base(template, wrappedBase)
        {
            _internalData = internalData;
        }

        public override void Execute()
        {
            List<Collider2D> colliders = new List<Collider2D>();
            ContactFilter2D filter2D = new ContactFilter2D
            {
                useTriggers = true
            };

            int collidersCount = Physics2D.OverlapCollider(_internalData.ActionsData.HitBox, filter2D, colliders);

            for (int i = 0; i < collidersCount; i++)
            {
                if (colliders[i].TryGetComponent(out IDamageable damageable) == false) 
                    continue;
                
                if (damageable.GetType() != _internalData.Type)
                {
                    int damage = (int) (data.Amount * _internalData.StatsData.GetStatValue(Enums.Stat.AttackMultiplier));
                    Vector2 directionalForce = new Vector2(data.KnockbackForce.x * _internalData.FaceDirection.x, data.KnockbackForce.y);
                    
                    damageable.TakeDamage(damage);
                    damageable.ApplyKnockBack(_internalData.ActionsData.HitBox, directionalForce, data.KnockbackTime, data.Knockback);
                }
            }
            
            wrappedBase?.Execute();
        }
    }
}