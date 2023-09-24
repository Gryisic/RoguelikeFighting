using System;
using System.Collections.Generic;
using System.Linq;
using Common.Units;
using UnityEngine;

namespace Common.Scene
{
    [Serializable]
    public class SpawnInfo
    {
        [SerializeField] private Transform _unitsRoot;
        [SerializeField] private Transform _playerSpawnPoint;
        
        [SerializeField] private UnitTemplate[] _templatesToSpawn;
        
        public Transform UnitsRoot => _unitsRoot;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public IReadOnlyList<UnitTemplate> TemplatesToSpawn => _templatesToSpawn;
        public IReadOnlyList<int> GetIDList => _templatesToSpawn.Select(t => t.ID).ToList();
    }
}