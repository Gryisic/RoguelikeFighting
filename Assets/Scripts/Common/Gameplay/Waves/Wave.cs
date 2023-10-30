using System;
using Common.Units;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    [Serializable]
    public class Wave : WaveBase
    {
        [SerializeField] private SubWave[] _subWaves;

        private int _currentSubWaveIndex;

        public bool HasNextSubWave => _currentSubWaveIndex < _subWaves.Length;

        public override void Initialize(UnitsHandler unitsHandler)
        {
            foreach (var wave in _subWaves) 
                wave.Initialize(unitsHandler);
            
            base.Initialize(unitsHandler);
        }
        
        public override void Dispose()
        {
            foreach (var wave in _subWaves) 
                wave.Dispose();
            
            base.Dispose();
        }

        public void NextSubWave()
        {
            _subWaves[_currentSubWaveIndex].Activate();

            _currentSubWaveIndex++;
        }
    }
}