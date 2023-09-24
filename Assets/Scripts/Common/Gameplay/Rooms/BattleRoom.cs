using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Gameplay.Waves;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public class BattleRoom : Room
    {
        [SerializeField] private BattleRoomTemplate _template;
        [SerializeField] private TriggerWaveMap[] _wavesMap;

        private IStageData _stageData;
        
        private NextWaveRequirement _requirement;
        private int _currentWaveIndex;

        private void Awake()
        {
            _requirement = DefineRequirement();
            
            SubscribeToEvents();
        }

        public override void Initialize(IStageData stageData)
        {
            _stageData = stageData;

            foreach (var map in _wavesMap)
            {
                map.Wave.Initialize(stageData.UnitsHandler);
            }
        }

        public override void Dispose()
        {
            UnsubscribeToEvents();
            
            if (_requirement is IDisposable disposable)
                disposable.Dispose();
        }

        private void ActivateWave()
        {
            _wavesMap[_currentWaveIndex].Wave.Activate();
            _requirement.StartChecking();
        }
        
        private void NextWave()
        {
            if (_wavesMap[_currentWaveIndex].Wave.HasNextSubWave)
            {
                _wavesMap[_currentWaveIndex].Wave.NextSubWave();
                _requirement.StartChecking();

                return;
            }
            
            _currentWaveIndex++;
            
            if (_currentWaveIndex == _wavesMap.Length)
            {
                Debug.Log("Win");
            }
        }
        
        private void SubscribeToEvents()
        {
            _requirement.Fulfilled += NextWave;
            
            foreach (var map in _wavesMap)
            {
                map.Trigger.Triggered += ActivateWave;
            }
        }

        private void UnsubscribeToEvents()
        {
            _requirement.Fulfilled -= NextWave;
            
            foreach (var map in _wavesMap)
            {
                map.Trigger.Triggered -= ActivateWave;
            }
        }
        
        private NextWaveRequirement DefineRequirement()
        {
            switch (_template.Requirement)
            {
                case Enums.NextWaveRequirement.EnemiesDefeated:
                    return null;
                
                case Enums.NextWaveRequirement.Timer:
                    return new TimerWaveRequirement(_template.Timer);
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        [Serializable]
        private struct TriggerWaveMap
        {
            [SerializeField] private BattleTrigger _trigger;
            [SerializeField] private Wave _wave;

            public BattleTrigger Trigger => _trigger;
            public Wave Wave => _wave;
        }
    }
}