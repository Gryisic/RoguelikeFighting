using Common.Gameplay.Interfaces;
using Common.Units.Interfaces;
using Core.Interfaces;

namespace Common.Gameplay.Data
{
    public class RunData : IRunData
    {
#if UNITY_EDITOR

        public IInitialHeroData InitialHeroData { get; }
#endif
        
        public ISharedUnitData SharedHeroData { get; private set; }
        public ModifiersData ModifiersData { get; }

#if UNITY_EDITOR
        public RunData(IInitialHeroData initialData, ModifiersData modifiersData)
        {
            InitialHeroData = initialData;
            ModifiersData = modifiersData;
        }
#endif

        // public RunData(ModifiersData modifiersData)
        // {
        //     ModifiersData = modifiersData;
        // }

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