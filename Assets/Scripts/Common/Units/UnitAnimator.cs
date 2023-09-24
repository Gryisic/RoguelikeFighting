using System;
using System.Collections.Generic;
using Infrastructure.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace Common.Units
{
    [Serializable]
    public class UnitAnimator
    {
        [SerializeField] private Animator _animator;

        private Dictionary<Enums.DefaultAnimation, int> _animationKeyMap = new Dictionary<Enums.DefaultAnimation, int>();

        public void PlayDefaultAnimation(Enums.DefaultAnimation animation)
        {
            if (_animationKeyMap.ContainsKey(animation) == false)
            {
                string name = Enum.GetName(typeof(Enums.DefaultAnimation), animation);
                int hash = Animator.StringToHash(name);
                
                _animationKeyMap.Add(animation, hash);
            }
            
            _animator.Play(_animationKeyMap[animation]);
        }
        
        public void PlayAnimationClip(AnimationClip clip)
        {
            _animator.Play(clip.name);
        }

        public void UpdateAnimatorController(AnimatorController controller) =>
            _animator.runtimeAnimatorController = controller;
    }
}