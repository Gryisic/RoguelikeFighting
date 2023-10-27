using System;
using System.Collections.Generic;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    public class UnitsHandler : IDisposable
    {
        private readonly List<Unit> _units = new List<Unit>();
        private readonly Dictionary<Enums.Enemy, UnitPack<Enemy>> _enemiesPack;

        public IReadOnlyList<Unit> Units => _units;
        public Hero Hero { get; private set; }

        public UnitsHandler()
        {
            _enemiesPack = new Dictionary<Enums.Enemy, UnitPack<Enemy>>();
        }
        
        public void Dispose()
        {
            foreach (var u in _units)
            {
                if (u is IDisposable disposable)
                    disposable.Dispose();
            }
        }
        
        public void Add(Unit unit)
        {
            if (unit == null)
                throw new InvalidOperationException("Trying to add null unit");
            
            switch (unit)
            {
                case Hero hero:
                    Hero = hero;
                    break;
                
                case Enemy enemy:
                    if (_enemiesPack.ContainsKey(enemy.Type) == false)
                        _enemiesPack.Add(enemy.Type, new UnitPack<Enemy>());
                    
                    _enemiesPack[enemy.Type].Add(enemy);
                    break;

                default:
                    throw new InvalidOperationException($"Type of unit {unit.name} with id {unit.GetInstanceID()} is invalid");
            }

            _units.Add(unit);
        }

        public void Return(Unit unit)
        {
            switch (unit)
            {
                case Enemy enemy:
                    _enemiesPack[enemy.Type].Add(enemy);
                    break;
            }
        }

        public bool TryGetEnemy(Enums.Enemy type, out Enemy enemy)
        {
            enemy = null;

            if (_enemiesPack[type].HasUnit == false) 
                return false;
            
            enemy = _enemiesPack[type].Get();

            return true;
        }
        
        public bool Remove(Unit unit)
        {
            if (unit == null)
                throw new InvalidOperationException("Trying to remove null unit");
            
            switch (unit)
            {
                case Hero partyMember:
                    break;
                
                case Enemy enemy:
                    break;
                
                default:
                    throw new InvalidOperationException($"Type of unit {unit.name} with id {unit.GetInstanceID()} is invalid");
            }
            
            return _units.Remove(unit);
        }
        
        private class UnitPack<T> where T: Unit
        {
            private readonly List<T> _units = new List<T>();

            public bool HasUnit => _units.Count > 0;

            public T Get()
            {
                T unit = _units[0];

                _units.Remove(unit);

                return unit;
            }

            public void Add(T unit) => _units.Add(unit);
        }
    }
}