using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Items
{
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private Enums.RunDataType _type;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _description;
        [SerializeField] private Enums.RunDataChange _change;
        [SerializeField] private int _amount;
        
        public Enums.RunDataType Type => _type;
        public Sprite Icon => _icon;
        public string Description => _description;
        public Enums.RunDataChange Change => _change;
        public int Amount => _amount;
    }
}