using System;
using Common.Units.Heroes;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class DebugHeroTemplate : IInitialHeroData
    {
        [SerializeField] private HeroTemplate _template;

        public HeroTemplate HeroTemplate => _template;
    }
}