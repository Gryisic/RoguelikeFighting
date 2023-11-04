using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Gameplay.Waves;
using Common.Scene.Cameras.Interfaces;
using Core.Configs.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public class BattleRoom : Room
    {
        [SerializeField] private WaveTemplate _template;
        [SerializeField] private TriggerWaveMap[] _wavesMap;

        private IStageData _stageData;
        private IRunData _runData;
        private IDifficultyConfig _difficultyConfig;

        private int _currentWaveIndex;

        protected override ICameraService CameraService { get; set; }
        public override Enums.RoomType Type => Enums.RoomType.Battle;

        public event Action<int> ExperienceObtained;

        public override void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService)
        {
            _stageData = stageData;
            _runData = runData;
            CameraService = cameraService;

            SubscribeToEvents();

            foreach (var map in _wavesMap) 
                map.Wave.Initialize(stageData.UnitsHandler);
        }

        public override void Dispose()
        {
            UnsubscribeToEvents();
            
            foreach (var map in _wavesMap) 
                map.Wave.Dispose();

            base.Dispose();
        }

        public override void Enter()
        {
            foreach (var map in _wavesMap) 
                map.Trigger.Activate();
            
            _runData.IncreaseVisitedRoomsAmount();
            ChangeTrigger.Deactivate();
            CameraService.FollowUnit(_stageData.UnitsHandler.Hero.Transform);
            
            base.Enter();
        }

        public void SetDifficultyData(IDifficultyConfig difficultyConfig) => _difficultyConfig = difficultyConfig;

        private void ActivateWave() => _wavesMap[_currentWaveIndex].Wave.Activate(_template);

        private void OnWaveEnded()
        {
            TriggerWaveMap waveMap = _wavesMap[_currentWaveIndex];
            
            waveMap.EndTrigger.Activate();
            waveMap.Wave.Deactivate();

            if (_currentWaveIndex + 1 >= _wavesMap.Length)
            {
                _currentWaveIndex = 0;

                AddExperience();
            }
            else
            {
                _currentWaveIndex++;
            }
        }

        private void SubscribeToEvents()
        {
            foreach (var map in _wavesMap)
            {
                map.Wave.Ended += OnWaveEnded;
                map.Trigger.Triggered += ActivateWave;
            }
        }

        private void UnsubscribeToEvents()
        {
            foreach (var map in _wavesMap)
            {
                map.Wave.Ended -= OnWaveEnded;
                map.Trigger.Triggered -= ActivateWave;
            }
        }
        
        private void AddExperience()
        {
            int experienceAmount = Mathf.CeilToInt(Constants.DefaultExperiencePerBattleAmount *
                                                   _difficultyConfig.BattleExperienceMultiplierCurve.Evaluate(_runData.VisitedRoomsAmount));
            
            ExperienceObtained?.Invoke(experienceAmount);
        }

        [Serializable]
        private struct TriggerWaveMap
        {
            [SerializeField] private BattleTrigger _trigger;
            [SerializeField] private Trigger _endTrigger;
            [SerializeField] private Wave _wave;

            public BattleTrigger Trigger => _trigger;
            public Trigger EndTrigger => _endTrigger;
            public Wave Wave => _wave;
        }
    }
}