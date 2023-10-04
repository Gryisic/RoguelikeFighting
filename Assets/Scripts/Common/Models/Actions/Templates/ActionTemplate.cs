using Infrastructure.Utils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Models.Actions.Templates
{
    [CreateAssetMenu(menuName = "Configs/Actions/DefaultAction")]
    public class ActionTemplate : ScriptableObject
    {
        [SerializeField] private string _name;
        
        [SerializeField] private AnimationClip _actionClip;

        [Space, Header("Base Data")]
        [SerializeField] private Enums.ActionEffect _baseEffect;
        [SerializeField] private bool _executeAfterClipPlayed;
        [SerializeField] private bool _isChargeable;
        [SerializeField] private float _chargeTime;
        [SerializeField] private AnimationClip _chargeClip;

        [Space, Header("Damage Data")]
        [SerializeField] private float _amount;
        [SerializeField] private Enums.Knockback _knockback;
        [SerializeField] private Vector2 _knockbackForce;
        [SerializeField] private float _knockbackTime = 0.3f;
        
        [Space, Header("Moving Data")]
        [SerializeField] private Vector2 _vector;
        [SerializeField] private float _distance;
        [FormerlySerializedAs("_time")] [SerializeField] private float _movingTime;
        [SerializeField] private bool _useClipLengthAsTime;

        [Space, Header("Stance Change Data")]
        [SerializeField] private AnimatorController _animatorController;
        [SerializeField] private ActionTemplate _nextStanceTemplate;

        public string Name => _name;
        
        public AnimationClip ActionClip => _actionClip;

        public Enums.ActionEffect BaseEffect => _baseEffect;
        
        public bool ExecuteAfterClipPlayed => _executeAfterClipPlayed;
        public bool IsChargeable => _isChargeable;
        public float ChargeTime => _chargeTime;
        public AnimationClip ChargeClip => _chargeClip;
        
        public float Amount => _amount;
        public Enums.Knockback Knockback => _knockback;
        public Vector2 KnockbackForce => _knockbackForce;
        public float KnockbackTime => _knockbackTime;

        public Vector2 Vector => _vector;
        public float Distance => _distance;
        public float MovingTime => _movingTime;
        public bool UseClipLengthAsTime => _useClipLengthAsTime;
        
        public AnimatorController AnimatorController => _animatorController;
        public ActionTemplate NextStanceTemplate => _nextStanceTemplate;
    }
}