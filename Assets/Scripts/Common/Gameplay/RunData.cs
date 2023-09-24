using Common.Gameplay.Interfaces;
using Common.Gameplay.Modifiers;
using Common.Units.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Common.Gameplay
{
    public class RunData : IRunData
    {
#if UNITY_EDITOR

        public IInitialHeroData InitialHeroData { get; }
#endif
        
        public ISharedUnitData SharedHeroData { get; private set; }

#if UNITY_EDITOR
        public RunData(IInitialHeroData initialData)
        {
            InitialHeroData = initialData;
        }
#endif

        public void SetHeroData(ISharedUnitData heroData)
        {
            SharedHeroData = heroData;
        }

        public void Reset()
        {
            SharedHeroData = null;
        }
    }
}