using System;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Actions
{
    public abstract class UnitAction : IDisposable
    {
        protected ActionBase executionBase;
        
        public ActionTemplate Data { get; }

        protected UnitAction(ActionTemplate data)
        {
            Data = data;
        }

        public virtual void Dispose()
        {
            if (executionBase is IDisposable disposable)
                disposable.Dispose();
        }

        public abstract void Execute();
        
        protected ActionBase DefineExecutionBase(Enums.ActionEffect effect, IUnitInternalData internalData)
        {
            switch (effect)
            {
                case Enums.ActionEffect.MeleeAttack:
                    return new MeleeAttackAction(internalData, Data);

                case Enums.ActionEffect.Projectile:
                    return new ProjectileAction(internalData, Data);

                case Enums.ActionEffect.ChangeStance:
                    return new ChangeStance(internalData, Data);

                case Enums.ActionEffect.Teleportation:
                    return new TeleportationAction(internalData, Data);
                
                case Enums.ActionEffect.Heal:
                    return new HealAction(internalData, Data);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
            }
        }
    }
}