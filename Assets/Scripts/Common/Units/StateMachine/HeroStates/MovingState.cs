using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class MovingState : HeroState, IDashExecutor, IJumpExecutor
    {
        public MovingState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

        public override void Enter()
        {
            internalData.Animator.PlayDefaultAnimation(Enums.DefaultAnimation.Walk);
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            Vector2 direction = internalData.MoveDirection * Constants.PlayerMovementSpeed;
                
            internalData.Physics.UpdateHorizontalVelocity(direction.x);
            
            if (internalData.MoveDirection.x == 0)
                unitStatesChanger.ChangeState<IdleState>();
            
            if (internalData.InAir) 
                unitStatesChanger.ChangeState<AirState>();
        }

        public void Dash()
        {
            if (internalData.CanDash)
                unitStatesChanger.ChangeState<DashState>();
        }

        public void Jump()
        {
            if (internalData.InAir == false)
            {
                internalData.RequestJump();
                unitStatesChanger.ChangeState<AirState>();
            }
        }
    }
}