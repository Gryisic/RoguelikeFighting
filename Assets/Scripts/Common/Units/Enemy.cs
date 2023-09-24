using Common.Models.Actions;
using Common.Units.Enemies;
using Common.Units.Interfaces;
using Common.Units.StateMachine.EnemyStates;
using Infrastructure.Utils;

namespace Common.Units
{
    public class Enemy : Unit
    {
        public Enums.Enemy Type { get; private set; }

        public override void Initialize(UnitTemplate template)
        {
            EnemyTemplate enemyTemplate = template as EnemyTemplate;

            Type = enemyTemplate.Type;

            internalData = new EnemyInternalData(physics, Transform, animator, StatsData, actionsData, animationEventsReceiver, GetType());
            EnemyAction action = new EnemyAction(enemyTemplate.ActionTemplate, internalData as EnemyInternalData);

            EnemyInternalData enemyData = internalData as EnemyInternalData;
            enemyData.SetAction(action);

            states = new IUnitState[]
            {
                new IdleState(this, internalData as EnemyInternalData),
                new ActionState(this, internalData as EnemyInternalData),
                new StaggerState(this, internalData as EnemyInternalData)
            };
            
            base.Initialize(template);
        }

        public override void Activate()
        {
            ChangeState<IdleState>();
            
            base.Activate();
        }
    }
}