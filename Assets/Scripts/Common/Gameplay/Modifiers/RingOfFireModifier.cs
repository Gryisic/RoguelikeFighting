using System;
using System.Collections.Generic;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers.Templates;
using Common.Models.StatusEffects;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Gameplay.Modifiers
{
    public class RingOfFireModifier : Modifier, IDisposable
    {
        private readonly CancellationTokenSource _timerTokenSource;

        public RingOfFireModifier(ModifierTemplate data, Modifier wrappedModifier = null) : base(data, wrappedModifier)
        {
            _timerTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _timerTokenSource.Cancel();
            _timerTokenSource.Dispose();
        }
        
        public override void Execute(IUnitInternalData unitInternalData)
        {
            ExecuteAsync(unitInternalData).Forget();
        }

        public override void Reset()
        {
            _timerTokenSource.Cancel();
        }

        protected override T GetDataInternal<T>()
        {
            if (wrappedModifier != null)
                return wrappedModifier.GetData<T>();

            return data as T;
        }

        private bool TryApplyStatusEffect(StatusEffectTemplate statusEffectTemplate, out StatusEffect effect)
        {
            int chance = Random.Range(0, 100);

            if (TryDefineStatusEffect(out effect))
                return chance < effect.Data.AffectChance;

            return false;
        }
        
        private async UniTask ExecuteAsync(IUnitInternalData unitInternalData)
        {
            RingOfFireModifierTemplate internalData = GetDataInternal<RingOfFireModifierTemplate>();
            Collider2D[] colliders = new Collider2D[30];
            
            while (_timerTokenSource.IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(internalData.Time));

                int collidersCount = Physics2D.OverlapCircleNonAlloc(unitInternalData.Transform.position, internalData.Radius, colliders);

                for (int i = 0; i < collidersCount; i++)
                {
                    if (colliders[i].TryGetComponent(out IDamageable damageable) == false) 
                        continue;
                
                    if (damageable.GetType() != unitInternalData.Type)
                    {
                        int damage = (int) (internalData.Damage * internalData.DamageMultiplier);
                    
                        damageable.TakeDamage(damage);
                        
                        if (TryApplyStatusEffect(internalData.StatusEffectTemplate, out StatusEffect effect))
                            damageable.ApplyStatusEffect(effect);
                    }
                }
            }
        }
    }
}