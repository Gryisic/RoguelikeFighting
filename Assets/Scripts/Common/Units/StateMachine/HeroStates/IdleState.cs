using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class IdleState : HeroState, IAttackExecutor, IDashExecutor, IJumpExecutor, ISkillExecutor
    {
        public IdleState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

        public override void Enter()
        {
            if (internalData.InAir)
            {
                unitStatesChanger.ChangeState<AirState>();
                
                return;
            }
            
            internalData.ResetJumps();
            internalData.Animator.PlayDefaultAnimation(Enums.DefaultAnimation.Idle);
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            if (internalData.MoveDirection.x != 0)
                unitStatesChanger.ChangeState<MovingState>();
        }

        public void Attack() => SetActionAndChangeState(Enums.HeroActionType.BasicAttack);

        public void Skill() => SetActionAndChangeState(Enums.HeroActionType.Skill);

        public void Dash()
        {
            if (internalData.CanDash)
                unitStatesChanger.ChangeState<DashState>();
        }

        public void Jump()
        {
            if (internalData.InAir == false)
            {
                if (internalData.InputDirection == Enums.InputDirection.Down)
                    internalData.Physics.DropThroughPlatform();
                else
                    internalData.RequestJump();
                
                unitStatesChanger.ChangeState<AirState>();
            }
        }

        private void SetActionAndChangeState(Enums.HeroActionType actionType)
        {
            if (internalData.LastActionType == Enums.HeroActionType.None)
                internalData.SetAction(actionType);
            
            unitStatesChanger.ChangeState<ActionState>();
        }
    }
}