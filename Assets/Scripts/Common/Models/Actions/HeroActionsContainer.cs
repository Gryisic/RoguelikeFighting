using System;
using System.Collections.Generic;
using Common.Gameplay.Modifiers;
using Common.Units.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Actions
{
    public class HeroActionsContainer
    {
        private readonly IReadOnlyList<HeroAction> _actions;
        private readonly IHeroInternalData _internalData;

        private readonly List<Modifier> _timeBasedModifiers;

        private HeroAction _currentAction;

        public HeroActionsContainer(IReadOnlyList<HeroActionTemplate> actionTemplates, IHeroInternalData internalData)
        {
            _timeBasedModifiers = new List<Modifier>();
            _internalData = internalData;
            
            List<HeroAction> actions = new List<HeroAction>();

            for (var i = 0; i < actionTemplates.Count; i++)
            {
                HeroActionTemplate actionTemplate = actionTemplates[i];
                HeroAction action = new HeroAction(actionTemplate, actionTemplate.ExtendsInto, internalData);
                
                actions.Add(action);
            }

            _actions = actions;
        }

        public void ResetAction() => _currentAction = null;

        public void AddModifier(Modifier modifier)
        {
            if (modifier == null)
                throw new NullReferenceException("Trying to add modifier that is null");

            DefineModifier(modifier);
        }
        
        public bool TryGetNextAction(out HeroAction heroAction)
        {
            IReadOnlyList<HeroAction> cycleThrough = _actions;

            if (_currentAction != null)
                cycleThrough = _currentAction.ExtendsInto;

            for (var i = 0; i < cycleThrough.Count; i++)
            {
                HeroAction action = cycleThrough[i];
                
                if (action.IsConditionsFulfilled(_internalData.LastActionType, _internalData.InputDirection, _internalData.Placement) == false)
                    continue;

                heroAction = action;
                _currentAction = action;

                return true;
            }

            heroAction = null;
            
            return false;
        }

        private void DefineModifier(Modifier modifier)
        {
            if (modifier.DefaultData.ExecutionCondition == Enums.ModifierExecutionCondition.Time)
            {
                _timeBasedModifiers.Add(modifier);
                modifier.Execute(_internalData);
            }
            else
            {
                foreach (var action in _actions)
                {
                    if (action.Data.ExtendsFrom == modifier.DefaultData.ExtendsFromAction)
                        action.AddModifier(modifier);
                    
                    IReadOnlyList<HeroAction> childs = action.GetAllChildsExtendedFromActionType(modifier.DefaultData.ExtendsFromAction);
                    
                    for (var i = 0; i < childs.Count; i++)
                    {
                        HeroAction child = childs[i];
                        
                        child.AddModifier(modifier);
                    }
                }
            }
        }
    }
}