using System;
using System.Threading;
using Common.Gameplay.Rooms;
using Common.Units;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Waves
{
    [Serializable]
    public class Wave : IDisposable
    {
        [SerializeField] private SubWave[] _subWaves;
        
        private CancellationTokenSource _requirementCheckTokenSource;
        private WaveRequirement _requirement;
        private int _currentSubWaveIndex;

        public event Action Ended;
        public bool HasNextSubWave => _currentSubWaveIndex < _subWaves.Length;

        public void Initialize(UnitsHandler unitsHandler)
        {
            foreach (var wave in _subWaves) 
                wave.Initialize(unitsHandler);
        }
        
        public void Dispose()
        {
            _requirementCheckTokenSource?.Cancel();
            _requirementCheckTokenSource?.Dispose();
            
            foreach (var wave in _subWaves) 
                wave.Dispose();
        }

        public void Activate(WaveTemplate template)
        {
            _requirementCheckTokenSource = new CancellationTokenSource();
            
            _requirement = DefineRequirement(template);
            _requirement.StartChecking(_requirementCheckTokenSource.Token);

            _requirement.Fulfilled += OnWaveRequirementFulfilled;

            foreach (var wave in _subWaves) 
                wave.EnemiesDefeated += NextSubWave;

            _currentSubWaveIndex = 0;
            
            _subWaves[_currentSubWaveIndex].Activate();
        }

        public void Deactivate()
        {
            _requirement.Fulfilled -= OnWaveRequirementFulfilled;
            
            foreach (var wave in _subWaves)
            {
                wave.EnemiesDefeated -= NextSubWave;
                
                wave.Deactivate();
            }
        }

        public void NextSubWave()
        {
            _currentSubWaveIndex++;

            if (HasNextSubWave == false)
            {
                if (_requirement is TimerWaveRequirement)
                {
                    _currentSubWaveIndex = 0;
                }
                else
                {
                    Ended?.Invoke();

                    return;
                }
            }
            
            _subWaves[_currentSubWaveIndex].Activate();
        }

        private void OnWaveRequirementFulfilled()
        {
            _subWaves[_currentSubWaveIndex].KillAllEnemies();
            
            Ended?.Invoke();
        }
        
        private WaveRequirement DefineRequirement(WaveTemplate template)
        {
            switch (template.Requirement)
            {
                case Enums.WaveRequirement.EnemiesDefeated:
                    return new EnemiesDefeatedWaveRequirement();
                
                case Enums.WaveRequirement.Timer:
                    return new TimerWaveRequirement(template.Timer);
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}