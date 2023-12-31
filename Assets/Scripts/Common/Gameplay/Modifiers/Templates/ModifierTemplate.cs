﻿using Common.Models.StatusEffects;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Modifiers.Templates
{
    public abstract class ModifierTemplate : ScriptableObject
    {
        [Header("Generic Data")]
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Enums.ModifierExecutionCondition _executionCondition;
        [SerializeField] private Enums.ModifierRarity _rarity;
        [SerializeField] private bool _isBase;

        [Space, Header("Action Based Extension Data")]
        [SerializeField] private Enums.HeroActionType _extendsFromAction;

        [Space, Header("Status Effect Data")] 
        [SerializeField] private StatusEffectTemplate _statusEffectTemplate;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public Enums.ModifierExecutionCondition ExecutionCondition => _executionCondition;
        public Enums.ModifierRarity Rarity => _rarity;
        public bool IsBase => _isBase;
        
        public Enums.HeroActionType ExtendsFromAction => _extendsFromAction;
        
        public abstract Enums.Modifier Type { get; }
        
        public StatusEffectTemplate StatusEffectTemplate => _statusEffectTemplate;
    }
}