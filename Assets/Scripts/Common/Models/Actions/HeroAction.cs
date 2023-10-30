using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Modifiers;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Actions
{
    public class HeroAction : UnitAction
    {
        private readonly IHeroInternalData _internalData;

        public new HeroActionTemplate Data { get; }

        public IReadOnlyList<HeroAction> ExtendsInto { get; }

        public HeroAction(HeroActionTemplate template, IReadOnlyList<HeroActionTemplate> extendsInto, IHeroInternalData internalData) : base(template)
        {
            Data = template;
            _internalData = internalData;

            List<HeroAction> actions = new List<HeroAction>();

            foreach (var actionTemplate in extendsInto)
            {
                HeroAction action = new HeroAction(actionTemplate, actionTemplate.ExtendsInto, internalData);
                
                actions.Add(action);
            }

            executionBase = DefineExecutionBase(template.BaseEffect, internalData);
            
            ExtendsInto = actions;
        }

        public bool IsConditionsFulfilled(Enums.HeroActionType extendsFrom, Enums.InputDirection direction, Enums.ActionExecutionPlacement placement) => 
            extendsFrom == Data.ExtendsFrom && direction == Data.Direction && placement == Data.ExecutionPlacement;

        public IReadOnlyList<HeroAction> GetAllChildsExtendedFromActionType(Enums.HeroActionType type)
        {
            List<HeroAction> actions = new List<HeroAction>();

            if (ExtendsInto.Count > 0)
            {
                List<HeroAction> childActions = ExtendsInto.Where(action => action.Data.ExtendsFrom == type).ToList();

                actions.AddRange(childActions);
                
                foreach (var action in childActions) 
                    actions.AddRange(action.GetAllChildsExtendedFromActionType(type));
            }

            return actions;
        }

        public IReadOnlyList<HeroAction> GetAllChilds()
        {
            List<HeroAction> actions = new List<HeroAction>();
            
            if (ExtendsInto.Count > 0)
            {
                List<HeroAction> childActions = ExtendsInto.ToList();
                
                actions.AddRange(childActions);
                
                foreach (var action in childActions) 
                    actions.AddRange(action.GetAllChilds());
            }

            return actions;
        }

        public void AddModifier(Modifier modifier)
        {
            ModifierAction modifiedAction = new ModifierAction(_internalData, modifier, Data, executionBase);
            
            executionBase = modifiedAction;
        }

        public override void Execute()
        {
            executionBase.Execute();
        }
    }
}