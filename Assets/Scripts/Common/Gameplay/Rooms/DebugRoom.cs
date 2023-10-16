#if UNITY_EDITOR

using System;
using Common.Gameplay.Interfaces;
using Common.Gameplay.Triggers;
using Common.Gameplay.Waves;
using Common.Scene.Cameras.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public class DebugRoom : Room
    {
        [SerializeField] private TriggerWaveMap[] _wavesMap;
        
        private IRunData _runData;

        protected override ICameraService CameraService { get; set; }
        
        public override Enums.RoomType Type => Enums.RoomType.Battle;
        
        private void Awake()
        {
            SubscribeToEvents();
        }

        public override void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService)
        {
            _runData = runData;
            CameraService = cameraService;
        }

        public override void Dispose()
        {
            
        }

        public override void Enter()
        {
            CameraService.FollowUnit(_runData.SharedHeroData.Transform);

            base.Enter();
        }

        private void SubscribeToEvents()
        {
            foreach (var map in _wavesMap)
            {
                map.Trigger.Triggered += map.Wave.Activate;
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
#endif