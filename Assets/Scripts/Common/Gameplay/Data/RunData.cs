using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Common.Units.Heroes;
using Common.Units.Interfaces;
using Core.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.Data
{
    public class RunData : IRunData
    {
        private Dictionary<Enums.RunDataType, IConcreteRunData> _dataMap;
        
        public IInitialHeroData InitialHeroData { get; }
        public IInitialLegacyUnitData InitialLegacyUnitData { get; }

        public HeroTemplate HeroTemplate { get; private set; }
        public ISharedUnitData SharedHeroData { get; private set; }
        public int VisitedRoomsAmount { get; private set; }

#if UNITY_EDITOR
        public RunData(IInitialHeroData initialData, IInitialLegacyUnitData initialLegacyUnitData, ModifiersData modifiersData)
        {
            HeroTemplate = initialData.HeroTemplate;
            InitialLegacyUnitData = initialLegacyUnitData;

            Initialize(modifiersData);
        }
#endif

        // public RunData(IInitialLegacyUnitData initialLegacyUnitData, ModifiersData modifiersData)
        // {
        //     InitialLegacyUnitData = initialLegacyUnitData;
        //     
        //     Initialize(modifiersData);
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
        
        public void SetHeroTemplate(HeroTemplate heroTemplate)
        {
            HeroTemplate = heroTemplate;
        }

        public void IncreaseVisitedRoomsAmount() => VisitedRoomsAmount++;

        public void Clear()
        {
            SharedHeroData = null;
            VisitedRoomsAmount = 0;

            foreach (var data in _dataMap.Values) 
                data.Clear();
        }
    }
}