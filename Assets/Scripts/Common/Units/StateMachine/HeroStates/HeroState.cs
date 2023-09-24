using Common.Units.Interfaces;

namespace Common.Units.StateMachine.HeroStates
{
    public abstract class HeroState : IHeroState
    {
        protected readonly IUnitStatesChanger unitStatesChanger;
        protected readonly IHeroInternalData internalData;

        protected HeroState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData)
        {
            this.unitStatesChanger = unitStatesChanger;
            this.internalData = internalData;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}