using System;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Models.StatusEffects
{
    public class BurnEffect : StatusEffect
    {
        private IDamageable _damageable;
        private CancellationTokenSource _affectTokenSource;
        
        public BurnEffect(IStatusEffectData data) : base(data) { }

        public override event Action<StatusEffect> EffectEnded;

        public override void Affect(ISharedUnitData unitData)
        {
            _damageable = unitData as IDamageable;
            
            _affectTokenSource = new CancellationTokenSource();

            AffectAsync().Forget();
        }

        public override void Clear()
        {
            _affectTokenSource?.Cancel();
        }

        public override void Dispose()
        {
            EffectEnded = null;
            
            _affectTokenSource?.Cancel();
            _affectTokenSource?.Dispose();
        }

        private async UniTask AffectAsync()
        {
            float timer = Data.Lifespan;
            
            while (timer > 0 && _affectTokenSource.IsCancellationRequested == false)
            {
                _damageable.TakeDamageWithoutStagger(Data.AffectAmount);
                
                await UniTask.Delay(TimeSpan.FromSeconds(Data.AffectTimer), cancellationToken: _affectTokenSource.Token);
                
                timer -= Data.AffectTimer;
            }
            
            EffectEnded?.Invoke(this);
        }
    }
}