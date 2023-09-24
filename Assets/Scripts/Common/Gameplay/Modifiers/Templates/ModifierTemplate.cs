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

        [Space, Header("Action Based Extension Data")]
        [SerializeField] private Enums.HeroActionType _extendsFromAction;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public Enums.ModifierExecutionCondition ExecutionCondition => _executionCondition;
        public Enums.ModifierRarity Rarity => _rarity;
        
        public Enums.HeroActionType ExtendsFromAction => _extendsFromAction;
        
        public abstract Enums.Modifier Type { get; }
    }
}