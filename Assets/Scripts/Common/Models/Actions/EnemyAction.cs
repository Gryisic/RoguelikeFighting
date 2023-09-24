using Common.Units.Interfaces;

namespace Common.Models.Actions
{
    public class EnemyAction : UnitAction
    {
        private readonly IEnemyInternalData _internalData;
        
        public new EnemyActionTemplate Data { get; }

        public EnemyAction(EnemyActionTemplate template, IEnemyInternalData internalData) : base(template)
        {
            Data = template;
            _internalData = internalData;

            executionBase = DefineExecutionBase(template.BaseEffect, internalData);
        }
        
        public override void Execute()
        {
            executionBase.Execute();
        }
    }
}