using Infrastructure.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace Common.Models.Actions
{
    public abstract class ActionTemplate : ScriptableObject
    {
        [SerializeField] private string _name;
        
        [Space, Header("Base Effect Data")]
        [SerializeField] private Enums.ActionEffect _baseEffect;
        [SerializeField] private float _amount;
        [SerializeField] private Enums.Knockback _knockback;
        [SerializeField] private Vector2 _knockbackForce;
        [SerializeField] private float _knockbackTime = 0.3f;

        [Space, Header("Moving Data")]
        [SerializeField] private Vector2 _vector;
        [SerializeField] private float _distance;
        [SerializeField] private float _time;
        [SerializeField] private bool _useClipLengthAsTime;

        [Space, Header("Stance Change Data")]
        [SerializeField] private AnimatorController _animatorController;
        [SerializeField] private ActionTemplate _nextStanceTemplate;

        public string Name => _name;
        
        public Enums.ActionEffect BaseEffect => _baseEffect;
        public float Amount => _amount;
        public Enums.Knockback Knockback => _knockback;
        public Vector2 KnockbackForce => _knockbackForce;
        public float KnockbackTime => _knockbackTime;
        
        public Vector2 Vector => _vector;
        public float Distance => _distance;
        public float Time => _time;
        public bool UseClipLengthAsTime => _useClipLengthAsTime;
        
        public AnimatorController AnimatorController => _animatorController;
        public ActionTemplate NextStanceTemplate => _nextStanceTemplate;
    }
}