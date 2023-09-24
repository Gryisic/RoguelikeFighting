using Common.Units.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Units
{
    public abstract class UnitTemplate : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;

        [Header("Stat Data")] 
        [SerializeField] private int _maxHealth;

        public int ID => _id;
        public string Name => _name;
        
        public int MaxHealth => _maxHealth;
    }
}