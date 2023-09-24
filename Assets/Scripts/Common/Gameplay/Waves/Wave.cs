using System;
using Common.Scene;
using Common.Units;
using Common.Units.Enemies;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    [Serializable]
    public class Wave
    {
        [SerializeField] private SpawnPoint[] _spawnPoints;
        [SerializeField] private Wave[] _subWaves;

        private UnitsHandler _unitsHandler;
        
        private int _currentSubWaveIndex;
        
        public bool HasNextSubWave => _currentSubWaveIndex < _subWaves.Length;

        public void Initialize(UnitsHandler unitsHandler)
        {
            _unitsHandler = unitsHandler;
            
            foreach (var wave in _subWaves) 
                wave.Initialize(unitsHandler);
        }
        
        public void Activate()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                EnemyTemplate enemyTemplate = spawnPoint.Template as EnemyTemplate;

                if (_unitsHandler.TryGetEnemy(enemyTemplate.Type, out Enemy enemy))
                {
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
    }
}