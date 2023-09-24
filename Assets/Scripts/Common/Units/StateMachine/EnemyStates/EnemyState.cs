using Common.Units.Interfaces;

namespace Common.Units.StateMachine.EnemyStates
{
    public abstract class EnemyState : IEnemyState
    {
        protected readonly IUnitStatesChanger unitStatesChanger;
        protected readonly IEnemyInternalData internalData;

        protected EnemyState(IUnitStatesChanger unitStatesChanger, IEnemyInternalData internalData)
        {
            this.unitStatesChanger = unitStatesChanger;
            this.internalData = internalData;
        }
        
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}