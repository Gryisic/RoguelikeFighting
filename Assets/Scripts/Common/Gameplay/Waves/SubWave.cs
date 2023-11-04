using System;
using System.Collections.Generic;
using Common.Scene;
using Common.Units;
using Common.Units.Enemies;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    [Serializable]
    public class SubWave : IDisposable
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
                enemy.Defeated -= OnEnemyDefeated;

            EnemiesDefeated = null;
        }
        
        public void Activate()
        {
            _enemies.Clear();
            
            foreach (var spawnPoint in _spawnPoints)
            {
                EnemyTemplate enemyTemplate = spawnPoint.Template as EnemyTemplate;

                if (_unitsHandler.TryGetEnemy(enemyTemplate.Type, out Enemy enemy))
                {
                    _enemies.Add(enemy);
                    
                    enemy.Defeated += OnEnemyDefeated;
                    
                    enemy.transform.position = spawnPoint.Position;

                    enemy.gameObject.SetActive(true);
                    enemy.Activate();
                }
            }
        }

        public void Deactivate()
        {
            foreach (var enemy in _enemies) 
                enemy.Defeated -= OnEnemyDefeated;
        }

        public void KillAllEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Defeated -= OnEnemyDefeated;
                
                enemy.Kill();
                _unitsHandler.Return(enemy);
            }
        }

        private void OnEnemyDefeated(Unit unit)
        {
            Enemy enemy = unit as Enemy;

            enemy.Defeated -= OnEnemyDefeated;
            
            _enemies.Remove(enemy);
            _unitsHandler.Return(enemy);

            if (_enemies.Count <= 0)
                EnemiesDefeated?.Invoke();
        }
    }
}