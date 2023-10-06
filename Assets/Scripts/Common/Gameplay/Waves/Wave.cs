using System;
using System.Collections.Generic;
using Common.Scene;
using Common.Units;
using Common.Units.Enemies;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    [Serializable]
    public class Wave : IDisposable
    {
        [SerializeField] private SpawnPoint[] _spawnPoints;
        [SerializeField] private Wave[] _subWaves;

        private List<Enemy> _enemies = new List<Enemy>();
        
        private UnitsHandler _unitsHandler;

        private int _currentSubWaveIndex;

        public event Action EnemiesDefeated;
        
        public bool HasNextSubWave => _currentSubWaveIndex < _subWaves.Length;

        public void Initialize(UnitsHandler unitsHandler)
        {
            _unitsHandler = unitsHandler;
            
            foreach (var wave in _subWaves) 
                wave.Initialize(unitsHandler);
        }
        
        public void Dispose()
        {
            foreach (var enemy in _enemies) 
                enemy.Defeated -= EnemyDefeated;

            foreach (var wave in _subWaves) 
                wave.Dispose();
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

        public void NextSubWave()
        {
            _subWaves[_currentSubWaveIndex].Activate();

            _currentSubWaveIndex++;
        }
        
        private void EnemyDefeated(Unit unit)
        {
            Enemy enemy = unit as Enemy;

            _enemies.Remove(enemy);

            if (_enemies.Count <= 0)
                EnemiesDefeated?.Invoke();
        }
    }
}