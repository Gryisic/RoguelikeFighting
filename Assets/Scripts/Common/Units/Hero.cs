using System;
using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers;
using Common.Models.Actions;
using Common.Models.Particles;
using Common.Models.Particles.Interfaces;
using Common.Units.Extensions;
using Common.Units.Heroes;
using Common.Units.Interfaces;
using Common.Units.StateMachine.HeroStates;
using Infrastructure.Utils;
using UnityEngine;
using Zenject;

namespace Common.Units
{
    public class Hero : Unit, IManuallyMovable, IHeroActions
    {
        private HealData _healData;
        
        public Enums.Hero HeroType { get; private set; }
        
        private IHeroInternalData HeroInternalData => internalData as IHeroInternalData;
        private IHeroState HeroState => activeState as IHeroState;

        [Inject]
        private void Construct(IRunData runData)
        {
            _healData = runData.Get<HealData>(Enums.RunDataType.Heal);
        }
        
        public void Attack() => HeroState.Attack();

        public void Skill() => HeroState.Skill();

        public void Interact() => HeroState.Interact();

        public void Jump() => HeroState.Jump();

        public void Dash() => HeroState.Dash();

        public void LegacySkill(Enums.HeroActionType skillType) { }
        
        public void Heal()
        {
            if (_healData.CanHeal == false)
                return;
            
            _healData.UseCharge();
            
            HeroState.Heal();
        }

        public void AddModifier(Modifier modifier)
        {
            HeroInternalData.ActionsContainer.AddModifier(modifier);
        }
        
        public override void Initialize(UnitTemplate template)
        {
            if (template is HeroTemplate == false)
                throw new InvalidOperationException("Trying to initialize Hero from non Hero Template");
            
            HeroTemplate heroTemplate = (HeroTemplate) template;

            List<IParticleData> particlesData = (from actionTemplate in heroTemplate.Actions where actionTemplate.ParticleForCopy != null 
                select new ParticleData(actionTemplate.ParticleForCopy, actionTemplate.ParticleID, actionTemplate.Rotation)).Cast<IParticleData>().ToList();

            UnitParticlesPlayer particlesPlayer = new UnitParticlesPlayer(transform, particlesData);
            IUnitRendererData rendererData = new UnitRendererData(particlesPlayer, animator, animationEventsReceiver);

            internalData = new HeroInternalData(Transform, physics, heroTemplate.heroAnimationData, rendererData, StatsData, actionsData, GetType());
            
            IHeroInternalData heroInternalData = (IHeroInternalData) internalData;
            HeroActionsContainer actionsContainer = new HeroActionsContainer(heroTemplate.Actions, heroInternalData);

            HeroInternalData data = internalData as HeroInternalData;
            data.SetActionsContainer(actionsContainer);

            HeroType = heroTemplate.HeroType;

            states = new IUnitState[]
            {
                new IdleState(this, heroInternalData),
                new MovingState(this, heroInternalData),
                new DashState(this, heroInternalData),
                new AirState(this, heroInternalData),
                new ActionState(this, heroInternalData),
                new StateMachine.StaggerState(this, heroInternalData)
            };
            
            heroInternalData.SetDashData(heroTemplate.DashDistance, heroTemplate.DashForce, heroTemplate.MaxDashesCount);
            heroInternalData.SetJumpData(heroTemplate.JumpsCount);

            base.Initialize(template);
        }

        public override void Activate()
        {
            ChangeState<IdleState>();
            
            base.Activate();
        }

        public void StartMoving(Vector2 direction)
        {
            internalData.SetMoveDirection(direction);
            
            Flip(direction);
        }

        public void StopMoving()
        {
            physics.UpdateHorizontalVelocity(0);
            internalData.SetMoveDirection(Vector2.zero);
        }
    }
}