using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Common.Units.Interfaces;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Data
{
    public class RunData : IRunData
    {
        private Dictionary<Enums.RunDataType, IConcreteRunData> _dataMap;

#if UNITY_EDITOR

        public IInitialHeroData InitialHeroData { get; }
        public IInitialLegacyUnitData InitialLegacyUnitData { get; }
#endif
        
        public ISharedUnitData SharedHeroData { get; private set; }

#if UNITY_EDITOR
        public RunData(IInitialHeroData initialData, IInitialLegacyUnitData initialLegacyUnitData, ModifiersData modifiersData)
        {
            InitialHeroData = initialData;
            InitialLegacyUnitData = initialLegacyUnitData;

            Initialize(modifiersData);
        }
#endif

        // public RunData(ModifiersData modifiersData)
        // {
        //     ModifiersData = modifiersData;
        // }
        
        private void Initialize(ModifiersData modifiersData)
        {
            _dataMap = new Dictionary<Enums.RunDataType, IConcreteRunData>()
            {
                { Enums.RunDataType.Modifiers, modifiersData },
                { Enums.RunDataType.Gald, new GaldData() },
                { Enums.RunDataType.Heal, new HealData() },
                { Enums.RunDataType.Experience, new ExperienceData() }
            };
        }

        public void SetHeroData(ISharedUnitData heroData)
        {
            SharedHeroData = heroData;
        }

        public T Get<T>(Enums.RunDataType type) where T : IConcreteRunData => (T) _dataMap[type];

        public void Clear()
        {
            SharedHeroData = null;

            foreach (var data in _dataMap.Values) 
                data.Clear();
        }
    }
}