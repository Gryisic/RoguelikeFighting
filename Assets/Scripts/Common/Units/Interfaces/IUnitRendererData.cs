using Common.Models.Particles;

namespace Common.Units.Interfaces
{
    public interface IUnitRendererData
    {
        UnitParticlesPlayer ParticlesPlayer { get; }
        UnitAnimator Animator { get; }
        IAnimationEventsReceiver AnimationEventsReceiver { get; }
    }
}