using Common.Models.Particles;
using Common.Units.Interfaces;

namespace Common.Units
{
    public class UnitRendererData : IUnitRendererData
    {
        public UnitParticlesPlayer ParticlesPlayer { get; }
        public UnitAnimator Animator { get; }
        public IAnimationEventsReceiver AnimationEventsReceiver { get; }
        
        public UnitRendererData(UnitParticlesPlayer particlesPlayer, UnitAnimator animator, IAnimationEventsReceiver animationEventsReceiver)
        {
            ParticlesPlayer = particlesPlayer;
            Animator = animator;
            AnimationEventsReceiver = animationEventsReceiver;
        }
    }
}