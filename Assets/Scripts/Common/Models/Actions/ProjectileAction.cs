using Common.Units.Interfaces;

namespace Common.Models.Actions
{
    public class ProjectileAction : ActionBase
    {
        private readonly IUnitInternalData _internalData;

        public ProjectileAction(IUnitInternalData internalData, ActionTemplate template, ActionBase wrappedBase = null) : base(template, wrappedBase)
        {
            _internalData = internalData;
        }

        public override void Execute()
        {
            var test = _internalData as IEnemyInternalData;
            
            _internalData.ActionsData.Projectile.Launch(test.Transform.position, test.HeroData.Transform.position, _internalData.Type);
            
            wrappedBase?.Execute();
        }
    }
}