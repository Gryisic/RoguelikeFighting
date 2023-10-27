using Infrastructure.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace Common.Models.Actions.Templates
{
    [CreateAssetMenu(menuName = "Configs/Actions/DefaultAction")]
    public class ActionTemplate : ScriptableObject
    {
        [SerializeField] private string _name;
        
        [SerializeField] private AnimationClip _actionClip;

        [Space, Header("Base Data")]
        [SerializeField] private Enums.ActionEffect _baseEffect;
        [SerializeField] private Enums.ActionExecutionPlacement _executionPlacement;
        [SerializeField] private bool _executeAfterClipPlayed;
        [SerializeField] private bool _isChargeable;
        [SerializeField] private float _chargeTime;
        [SerializeField] private AnimationClip _chargeClip;

        [Space, Header("Particle Data")] 
        [SerializeField] private ParticleSystem _particleForCopy;
        [SerializeField] private int _particleID;
        [SerializeField] private float _rotation;

        [Space, Header("Damage Data")]
        [SerializeField] private int _amount;
        [SerializeField] private Enums.Knockback _knockback;
        [SerializeField] private Vector2 _knockbackForce;
        [SerializeField] private float _knockbackTime = 0.3f;
        
        [Space, Header("Moving Data")]
        [SerializeField] private Vector2 _vector;
        [SerializeField] private float _distance;
        [SerializeField] private float _movingTime;
        [SerializeField] private float _freezeAfterMoving;
        [SerializeField] private bool _useClipLengthAsTime;

        [Space, Header("Stance Change Data")]
        [SerializeField] private AnimatorController _animatorController;
        [SerializeField] private ActionTemplate _nextStanceTemplate;

        [Space, Header("Teleportation Data")] 
        [SerializeField] private Vector2 _positionRelativeToTarget;

        [Space, Header("Heal Data")]
        [SerializeField] private float _healPercent;
        [SerializeField] private float _healTime;

        public string Name => _name;
        
        public AnimationClip ActionClip => _actionClip;

        public Enums.ActionEffect BaseEffect => _baseEffect;
        public Enums.ActionExecutionPlacement ExecutionPlacement => _executionPlacement;
        
        public bool ExecuteAfterClipPlayed => _executeAfterClipPlayed;
        public bool IsChargeable => _isChargeable;
        public float ChargeTime => _chargeTime;
        public AnimationClip ChargeClip => _chargeClip;
        
        public ParticleSystem ParticleForCopy => _particleForCopy;
        public int ParticleID => _particleID;
        public float Rotation => _rotation;
        
        public int Amount => _amount;
        public Enums.Knockback Knockback => _knockback;
        public Vector2 KnockbackForce => _knockbackForce;
        public float KnockbackTime => _knockbackTime;

        public Vector2 Vector => _vector;
        public float Distance => _distance;
        public float MovingTime => _movingTime;
        public bool UseClipLengthAsTime => _useClipLengthAsTime;
        
        public AnimatorController AnimatorController => _animatorController;
        public ActionTemplate NextStanceTemplate => _nextStanceTemplate;
        
        public Vector2 PositionRelativeToTarget => _positionRelativeToTarget;
        public float FreezeAfterMoving => _freezeAfterMoving;

        public float HealPercent => _healPercent;
        public float HealTime => _healTime;
    }
}