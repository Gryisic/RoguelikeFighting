using System;
using UnityEngine;

namespace Common.Units.Heroes
{
    [Serializable]
    public class HeroAnimationData : UnitAnimationData
    {
        [SerializeField] private AnimationClip _dashClip;
        
        [SerializeField] private JumpAnimationData _jumpAnimationData;
        [SerializeField] private AttackAnimationData _attackAnimationData;

        public AnimationClip DashClip => _dashClip;
        public AnimationClip RiseClip => _jumpAnimationData.RiseClip;
        public AnimationClip CycleClip => _jumpAnimationData.CycleClip;
        public AnimationClip LandingClip => _jumpAnimationData.LandingClip;
        public AnimationClip GroundAttack1 => _attackAnimationData.GroundAttack1;
        
        [Serializable]
        private struct JumpAnimationData
        {
            [SerializeField] private AnimationClip _riseClip;
            [SerializeField] private AnimationClip _cycleClip;
            [SerializeField] private AnimationClip _landingClip;

            public AnimationClip RiseClip => _riseClip;
            public AnimationClip CycleClip => _cycleClip;
            public AnimationClip LandingClip => _landingClip;
        }
        
        [Serializable]
        private struct AttackAnimationData
        {
            [SerializeField] private AnimationClip _groundAttack1;

            public AnimationClip GroundAttack1 => _groundAttack1;
        }
    }
}