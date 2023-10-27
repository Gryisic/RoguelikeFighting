using System;
using System.Threading;
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

        private CancellationTokenSource _requirementCheckTokenSource;
        private NextWaveRequirement _requirement;
        private int _currentWaveIndex;

        public override Enums.RoomType Type => Enums.RoomType.Battle;

        public override void Initialize(IStageData stageData, IRunData runData)
        {
            _stageData = stageData;
            _requirement = DefineRequirement();
            
            SubscribeToEvents();

            foreach (var map in _wavesMap) 
                map.Wave.Initialize(stageData.UnitsHandler);
        }

        public override void Dispose()
        {
            UnsubscribeToEvents();
            
            foreach (var map in _wavesMap) 
                map.Wave.Dispose();

            _requirementCheckTokenSource?.Cancel();
            _requirementCheckTokenSource?.Dispose();
            
            base.Dispose();
        }

        public override void Enter()
        {
            _requirementCheckTokenSource = new CancellationTokenSource();
            
            foreach (var map in _wavesMap) 
                map.Trigger.Activate();
            
            ChangeTrigger.Activate();

            base.Enter();
        }

        private void ActivateWave()
        {
            _wavesMap[_currentWaveIndex].Wave.Activate();
            _requirement.StartChecking(_requirementCheckTokenSource.Token);
        }
        
        private void NextWave()
        {
            if (_wavesMap[_currentWaveIndex].Wave.HasNextSubWave)
            {
                _wavesMap[_currentWaveIndex].Wave.NextSubWave();
                _requirement.StartChecking(_requirementCheckTokenSource.Token);

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
                map.Wave.EnemiesDefeated += NextWave;
                map.Trigger.Triggered += ActivateWave;
            }
        }

        private void UnsubscribeToEvents()
        {
            _requirement.Fulfilled -= NextWave;
            
            foreach (var map in _wavesMap)
            {
                map.Wave.EnemiesDefeated -= NextWave;
                map.Trigger.Triggered -= ActivateWave;
            }
        }
        
        private NextWaveRequirement DefineRequirement()
        {
            switch (_template.Requirement)
            {
                case Enums.NextWaveRequirement.EnemiesDefeated:
                    return new EnemiesDefeatedWaveRequirement();
                
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