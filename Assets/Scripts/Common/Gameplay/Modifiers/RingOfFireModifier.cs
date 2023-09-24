using System;
using System.Collections.Generic;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers.Templates;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Gameplay.Modifiers
{
    public class RingOfFireModifier : Modifier, IDisposable
    {
        private readonly CancellationTokenSource _timerTokenSource = new CancellationTokenSource();
        
        public RingOfFireModifier(ModifierTemplate data, Modifier wrappedModifier = null) : base(data, wrappedModifier) { }

        public void Dispose()
        {
            _timerTokenSource.Cancel();
            _timerTokenSource.Dispose();
        }
        
        public override void Execute(IUnitInternalData unitInternalData)
        {
            ExecuteAsync(unitInternalData).Forget();
        }
        
        protected override T GetDataInternal<T>()
        {
            if (wrappedModifier != null)
                return wrappedModifier.GetData<T>();

            return data as T;
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
                    }
                }
                
                Debug.Log("Ring");
            }
        }
    }
}