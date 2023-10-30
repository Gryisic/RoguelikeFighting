using System.Collections.Generic;
using System.Linq;
using Common.Models.Actions;
using Common.Models.Particles;
using Common.Models.Particles.Interfaces;
using Common.UI.Gameplay;
using Common.Units.Enemies;
using Common.Units.Interfaces;
using Common.Units.StateMachine.EnemyStates;
using Infrastructure.Utils;
using Ink.Parsed;
using UnityEngine;

namespace Common.Units
{
    public class Enemy : Unit
    {
        [SerializeField] private EnemyHealthBar _healthBar;

        private ISharedUnitData _heroData;
        
        public Enums.Enemy Type { get; private set; }

        public override void Initialize(UnitTemplate template)
        {
            EnemyTemplate enemyTemplate = template as EnemyTemplate;

            Type = enemyTemplate.Type;

            List<IParticleData> particlesData = (from actionTemplate in enemyTemplate.ActionTemplates where actionTemplate.ParticleForCopy != null 
                select new ParticleData(actionTemplate.ParticleForCopy, actionTemplate.ParticleID)).Cast<IParticleData>().ToList();

            UnitParticlesPlayer particlesPlayer = new UnitParticlesPlayer();
            IUnitRendererData rendererData = new UnitRendererData(particlesPlayer, animator, animationEventsReceiver);
            
            particlesPlayer.Initialize(transform, particlesData, genericParticlesData);
            
            internalData = new EnemyInternalData(enemyTemplate, physics, Transform, rendererData, StatsData, actionsData, GetType());

            EnemyInternalData enemyData = internalData as EnemyInternalData;
            
            enemyData.SetHeroData(_heroData);
            
            List<EnemyAction> actions = enemyTemplate.ActionTemplates.Select(actionTemplate => new EnemyAction(actionTemplate, internalData as EnemyInternalData)).ToList();

            enemyData.SetActions(actions);

            states = new IUnitState[]
            {
                new IdleState(this, internalData as EnemyInternalData),
                new ActionState(this, internalData as EnemyInternalData),
                new StateMachine.StaggerState(this, internalData as EnemyInternalData)
            };
            
            HealthUpdated += _healthBar.UpdateValue;
            
            base.Initialize(template);
        }
        
        public override void Dispose()
        {
            HealthUpdated -= _healthBar.UpdateValue;
            
            base.Dispose();
        }

        public override void Activate()
        {
            ChangeState<IdleState>();
            
            base.Activate();
        }

        public void SetHeroData(ISharedUnitData heroData) => _heroData = heroData;
    }
}