using System;
using System.Collections.Generic;
using Common.Scene;
using Common.Units;
using Common.Units.Enemies;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    [Serializable]
    public abstract class WaveBase : IDisposable
    {
        [SerializeField] private SpawnPoint[] _spawnPoints;
        
        private List<Enemy> _enemies;
        private UnitsHandler _unitsHandler;
        
        public event Action EnemiesDefeated;

        public virtual void Initialize(UnitsHandler unitsHandler)
        {
            _unitsHandler = unitsHandler;

            _enemies = new List<Enemy>();
        }
        
        public virtual void Dispose()
        {
            foreach (var enemy in _enemies) 
                enemy.Defeated -= EnemyDefeated;
        }
        
        public void Activate()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                EnemyTemplate enemyTemplate = spawnPoint.Template as EnemyTemplate;

                if (_unitsHandler.TryGetEnemy(enemyTemplate.Type, out Enemy enemy))
                {
                    _enemies.Add(enemy);
                    
                    enemy.Defeated += EnemyDefeated;
                    
                    enemy.transform.position = spawnPoint.Position;

                    enemy.gameObject.SetActive(true);
                    enemy.Activate();
                }
            }
        }
        
        private void EnemyDefeated(Unit unit)
        {
            Enemy enemy = unit as Enemy;

            _enemies.Remove(enemy);
            _unitsHandler.Return(enemy);

            if (_enemies.Count <= 0)
                EnemiesDefeated?.Invoke();
        }
    }
}