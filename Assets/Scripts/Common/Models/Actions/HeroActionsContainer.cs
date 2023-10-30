using System;
using System.Collections.Generic;
using Common.Gameplay.Modifiers;
using Common.Models.Actions.Templates;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using ModifierType = Infrastructure.Utils.Enums.Modifier;

namespace Common.Models.Actions
{
    public class HeroActionsContainer
    {
        private readonly IReadOnlyList<HeroAction> _actions;
        private readonly IHeroInternalData _internalData;
        private readonly Dictionary<Enums.HeroActionType, LegacyAction> _legacyActionsMap;

        private readonly Dictionary<ModifierType, Modifier> _timeBasedModifiers;

        private HeroAction _currentAction;

        public HeroActionsContainer(IReadOnlyList<HeroActionTemplate> actionTemplates, IHeroInternalData internalData)
        {
            _legacyActionsMap = new Dictionary<Enums.HeroActionType, LegacyAction>();
            _timeBasedModifiers = new Dictionary<ModifierType, Modifier>();
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

        public void AddLegacyAction(LegacyAction action, Enums.HeroActionType actionType) => _legacyActionsMap[actionType] = action;

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

        public bool TryGetLegacyAction(Enums.HeroActionType actionType, out LegacyAction action) => _legacyActionsMap.TryGetValue(actionType, out action);

        public IReadOnlyList<HeroAction> GetAllActions()
        {
            List<HeroAction> actions = new List<HeroAction>();

            for (var i = 0; i < _actions.Count; i++)
            {
                HeroAction action = _actions[i];
                
                actions.Add(action);
                actions.AddRange(action.GetAllChilds());
            }

            return actions;
        }

        private void DefineModifier(Modifier modifier)
        {
            if (modifier.DefaultData.ExecutionCondition == Enums.ModifierExecutionCondition.Time)
            {
                if (_timeBasedModifiers.TryGetValue(modifier.DefaultData.Type, out Modifier oldModifier))
                    oldModifier.Reset();
                
                _timeBasedModifiers[modifier.DefaultData.Type] = modifier;
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