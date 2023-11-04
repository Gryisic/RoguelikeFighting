using System;
using UnityEngine;

namespace Common.Units.Heroes
{
    [Serializable]
    public class HeroAnimationData : UnitAnimationData
    {
        [SerializeField] private AnimationClip _dashClip;
        
        [SerializeField] private JumpAnimationData _jumpAnimationData;
        [SerializeField] private CrouchAnimationData _crouchAnimationData;
        
        public AnimationClip DashClip => _dashClip;
        public AnimationClip RiseClip => _jumpAnimationData.RiseClip;
        public AnimationClip JumpCycleClip => _jumpAnimationData.CycleClip;
        public AnimationClip LandingClip => _jumpAnimationData.LandingClip;
        
        public AnimationClip CrouchClip => _crouchAnimationData.CrouchClip;
        public AnimationClip CrouchCycleClip => _crouchAnimationData.CrouchCycleClip;
        public AnimationClip StandingClip => _crouchAnimationData.StandingClip;

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
        private struct CrouchAnimationData
        {
            [SerializeField] private AnimationClip _crouchClip;
            [SerializeField] private AnimationClip _crouchCycleClip;
            [SerializeField] private AnimationClip _standingClip;

            public AnimationClip CrouchClip => _crouchClip;
            public AnimationClip CrouchCycleClip => _crouchCycleClip;
            public AnimationClip StandingClip => _standingClip;
        }
    }
}