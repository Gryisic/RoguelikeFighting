using System;
using System.Linq;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Common.Units.StateMachine.EnemyStates;
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
        
        [SerializeField] private BoxCollider2D _collider;

        public event Action<int, int> HealthUpdated; 

        protected IUnitInternalData internalData;
        protected UnitPhysics physics;
        
        protected IUnitState[] states;
        protected IUnitState activeState;
        
        private CancellationTokenSource _innerStateUpdateToken;
        private bool _isActive;

        public IUnitStatsData StatsData { get; private set; }
        
        public Transform Transform => transform;

        [Inject]
        private void Construct()
        {
            if (rigidbody == null)
            {
                Debug.LogWarning($"Rigidbody of unit(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                rigidbody = GetComponent<Rigidbody2D>();
            }
            
            if (_collider == null)
            {
                Debug.LogWarning($"Collider of unit(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                _collider = GetComponent<BoxCollider2D>();
            }
            
            if (animationEventsReceiver == null)
            {
                Debug.LogWarning($"AnimationEventsReceiver of unit(Name: {name}, ID: {GetInstanceID()}) isn't assigned");

                animationEventsReceiver = GetComponent<AnimationEventsReceiver>();
            }

            physics = new UnitPhysics(rigidbody, _collider);
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
            
            internalData.Dispose();
            physics.Dispose();
        }

        public virtual void Initialize(UnitTemplate template)
        {
            UnitStats stats = StatsData as UnitStats;
            stats.SetData(template);
            
            physics.StartUpdating();
            
            _isActive = true;
            _innerStateUpdateToken = new CancellationTokenSource();
        }

        public virtual void Activate() => UpdateInnerState().Forget();

        public void TakeDamage(int amount)
        {
            internalData.SetStaggerTime(0.3f);

            int currentHealth = StatsData.GetStatValue(Enums.Stat.Health);
            int maxHealth = StatsData.GetStatValue(Enums.Stat.MaxHealth);
            
            HealthUpdated?.Invoke(currentHealth, maxHealth);
            
            ChangeState<StaggerState>();
        }

        public void ApplyKnockBack(Collider2D hitbox, Vector2 force, float time, Enums.Knockback knockback)
        {
            physics.AddKnockback(hitbox, force, time, knockback);
        }

        public void ChangeState<T>() where T : IUnitState
        {
            activeState?.Exit();
            activeState = states.First(s => s is T);
            activeState.Enter();
        }

        protected void Flip(Vector2 direction)
        {
            Vector2 faceDirection = new Vector2(Mathf.Ceil(direction.x), Mathf.Ceil(direction.y));
            internalData.SetFaceDirection(faceDirection);
            
            float rotation = direction.x >= 0 ? 0 : 180;
            Quaternion quaternion = new Quaternion(0, rotation, 0, 0);

            transform.rotation = quaternion;
        }

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