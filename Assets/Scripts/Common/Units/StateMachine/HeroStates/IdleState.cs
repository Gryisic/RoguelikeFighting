﻿using System;
using System.Threading;
using Common.Gameplay.Triggers;
using Common.Models.Actions;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.StateMachine.HeroStates
{
    public class IdleState : HeroState, IAttackExecutor, IDashExecutor, IJumpExecutor, ISkillExecutor, IInteractExecutor, IHealExecutor, ILegacySkillExecutor
    {
        public IdleState(IUnitStatesChanger unitStatesChanger, IHeroInternalData internalData) : base(unitStatesChanger, internalData) { }

        public override void Enter()
        {
            if (internalData.InAir)
            {
                unitStatesChanger.ChangeState<AirState>();
                
                return;
            }
            
            if (internalData.IsCrouching)
            {
                unitStatesChanger.ChangeState<CrouchState>();
                
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

            if (internalData.MoveDirection.y < 0)
            {
                unitStatesChanger.ChangeState<CrouchState>();
            }
        }

        public void Attack() => SetActionAndChangeState(Enums.HeroActionType.BasicAttack);

        public void Skill() => SetActionAndChangeState(Enums.HeroActionType.Skill);
        
        public void Heal() => SetActionAndChangeState(Enums.HeroActionType.Heal);
        
        public void LegacySkill(Enums.HeroActionType skillType)
        {
            if (internalData.ActionsContainer.TryGetLegacyAction(skillType, out LegacyAction action))
                action.Start();
        }

        public void Dash()
        {
            if (internalData.CanDash)
                unitStatesChanger.ChangeState<DashState>();
        }

        public void Jump()
        {
            internalData.UpdateInputDirection();
            
            if (internalData.InAir) 
                return;
            
            if (internalData.InputDirection == Enums.InputDirection.Down)
                internalData.Physics.DropThroughPlatform();
            else
                internalData.RequestJump();
                
            unitStatesChanger.ChangeState<AirState>();
        }
        
        public void Interact()
        {
            Collider2D[] colliders = new Collider2D[10];
            int collidersCount = Physics2D.OverlapCircleNonAlloc(internalData.Transform.position, Constants.InteractionRadius, colliders);

            for (int i = 0; i < collidersCount; i++)
            {
                Collider2D collider = colliders[i];
                
                if (collider.TryGetComponent(out Trigger trigger))
                {
                    trigger.Execute();
                    
                    return;
                }
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