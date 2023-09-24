using UnityEngine;

namespace Common.Models.Actions
{
    [CreateAssetMenu(menuName = "Configs/Actions/EnemyAction")]
    public class EnemyActionTemplate : ActionTemplate
    {
        [Space, Header("Action Data")]
        [SerializeField] private float _cooldownTime;
        [SerializeField] private AnimationClip _actionClip;
        
        [SerializeField] private bool _requireCharge;
        [SerializeField] private float _chargeTime;
        [SerializeField] private AnimationClip _chargeClip;

        public float CooldownTime => _cooldownTime;
        public AnimationClip ActionClip => _actionClip;
        
        public bool RequireCharge => _requireCharge;
        public float ChargeTime => _chargeTime;
        public AnimationClip ChargeClip => _chargeClip;
    }
}