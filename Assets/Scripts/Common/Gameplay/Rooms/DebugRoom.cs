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

        public override Enums.RoomType Type => Enums.RoomType.Battle;
        protected override ICameraService CameraService { get; set; }
        
        private void Awake()
        {
            SubscribeToEvents();
        }

        public override void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService)
        {
            CameraService = cameraService;
        }

        public override void Dispose()
        {
            base.Dispose();
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