using System;
using System.Linq;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Models.Actions;
using Common.Models.Particles;
using Common.Models.StatusEffects;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Interfaces;
using Common.Units.Stats;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;
using Zenject;

namespace Common.Units
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(AnimationEventsReceiver))]
    public abstract class Unit : MonoBehaviour, ISharedUnitData, IUnitStatesChanger, IDamageable, IDisposable
    {
        [SerializeField] protected new Rigidbody2D rigidbody;
        [SerializeField] protected UnitAnimator animator;
        [SerializeField] protected AnimationEventsReceiver animationEventsReceiver; 
        [SerializeField] protected ActionsData actionsData;
        [SerializeField] protected GenericParticlesData genericParticlesData;
        [SerializeField] protected new BoxCollider2D collider;

        public event Action<int, int> HealthUpdated;
        public event Action<Vector3, int> DamageTaken; 
        public event Action<Unit> Defeated; 

        protected IUnitInternalData internalData;
        protected UnitPhysics physics;
        
        protected IUnitState[] states;
        protected IUnitState activeState;
        
        private CancellationTokenSource _innerStateUpdateToken;
        private bool _isActive;

        public IUnitStatsData StatsData { get; private set; }
        public IStatusEffectsHandler EffectsHandler { get; private set; }

        public Transform Transform => transform;

        [Inject]
        private void Construct()
        {
            if (rigidbody == null)
            {
                Debug.LogWarning($"Rigidbody of unit(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                rigidbody = GetComponent<Rigidbody2D>();
            }
            
            if (collider == null)
            {
                Debug.LogWarning($"Collider of unit(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                collider = GetComponent<BoxCollider2D>();
            }
            
            if (animationEventsReceiver == null)
            {
                Debug.LogWarning($"AnimationEventsReceiver of unit(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                animationEventsReceiver = GetComponent<AnimationEventsReceiver>();
            }

            physics = new UnitPhysics(rigidbody, collider);
            StatsData = new UnitStats();
        }
        
        public virtual void Dispose()
        {
            if (_isActive)
            {
                _innerStateUpdateToken?.Cancel();
                _innerStateUpdateToken?.Dispose();
            }
            
            foreach (IUnitState unitState in states)
            {
                if (unitState is IDisposable disposable)
                    disposable.Dispose();
            }
            
            StatsData.HealthChanged -= OnHealthChanged;

            EffectsHandler.Dispose();
            internalData.Dispose();
            physics.Dispose();
        }

        public virtual void Initialize(UnitTemplate template)
        {
            EffectsHandler = new StatusEffectsHandler();
            
            UnitStats stats = StatsData as UnitStats;
            stats.SetData(template);
            
            StatsData.HealthChanged += OnHealthChanged;
            
            physics.StartUpdating();
            
            _isActive = true;
            _innerStateUpdateToken = new CancellationTokenSource();
        }

        public virtual void Activate() => UpdateInnerState().Forget();

        public void TakeDamage(int amount)
        {
            if (internalData.IsInvincible)
                return;

            amount = (int) (amount * StatsData.GetStatValue(Enums.Stat.DefenceMultiplier));
            
            internalData.SetStaggerTime(0.3f);
            StatsData.DecreaseStat(Enums.Stat.Health, amount);

            if (StatsData.GetStatValue(Enums.Stat.Health) <= 0)
            {
                Defeated?.Invoke(this);
                
                activeState?.Exit();
                EffectsHandler.Clear();
                gameObject.SetActive(false);
                
                return;
            }
            
            DamageTaken?.Invoke(Transform.position, amount);
            
            ChangeState<StateMachine.StaggerState>();
        }

        public void TakeDamageWithoutStagger(int amount)
        {
            if (internalData.IsInvincible)
                return;

            amount = (int) (amount * StatsData.GetStatValue(Enums.Stat.DefenceMultiplier));

            int currentHealth = StatsData.GetStatValueAsInt(Enums.Stat.Health);
            
            if (currentHealth - amount <= 0) 
                amount = Mathf.Clamp(currentHealth - amount, currentHealth, amount);

            StatsData.DecreaseStat(Enums.Stat.Health, amount);
            DamageTaken?.Invoke(Transform.position, amount);
        }

        public void ApplyKnockBack(Collider2D hitbox, Vector2 force, float time, Enums.Knockback knockback)
        {
            physics.AddKnockback(hitbox, force, time, knockback);
        }

        public void ApplyStatusEffect(StatusEffect effect)
        {
            if (internalData.IsInvincible || _isActive == false)
                return;
            
            effect.EffectEnded += RemoveStatusEffect;

            EffectsHandler.AddEffect(effect);
            effect.Affect(this);
        }

        public void ChangeState<T>() where T : IUnitState
        {
            activeState?.Exit();
            activeState = states.First(s => s is T);
            activeState.Enter();
        }

        private void RemoveStatusEffect(StatusEffect effect)
        {
            effect.EffectEnded -= RemoveStatusEffect;
            
            EffectsHandler.RemoveEffect(effect);
        }

        private void OnHealthChanged(int currentHealth, int maxHealth) => HealthUpdated?.Invoke(currentHealth, maxHealth);

        private async UniTask UpdateInnerState()
        {
            while (_innerStateUpdateToken.IsCancellationRequested == false)
            {
                activeState.Update();

                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}