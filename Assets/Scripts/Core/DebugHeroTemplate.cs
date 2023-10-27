using System;
using Common.Units.Heroes;
using Common.Units.Legacy;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class DebugHeroTemplate : IInitialHeroData, IInitialLegacyUnitData
    {
        [SerializeField] private HeroTemplate _template;
        [SerializeField] private LegacyUnitTemplate _firsLegacyUnitTemplate;
        [SerializeField] private LegacyUnitTemplate _secondLegacyUnitTemplate;

        public HeroTemplate HeroTemplate => _template;
        public LegacyUnitTemplate FirstLegacyUnitTemplate => _firsLegacyUnitTemplate;
        public LegacyUnitTemplate SecondLegacyUnitTemplate => _secondLegacyUnitTemplate;
    }
}