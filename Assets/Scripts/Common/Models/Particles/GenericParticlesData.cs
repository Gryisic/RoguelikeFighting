using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Particles
{
    [Serializable]
    public class GenericParticlesData
    {
        [SerializeField] private ParticleSystem _dashParticle;
        [SerializeField] private ParticleSystem _landingParticle;
        [SerializeField] private ParticleSystem _attackMarker;
        [SerializeField] private ParticleSystem _takeHit;
        [SerializeField] private ParticleSystem _midAirJump;

        public ParticleSystem GetParticleOfType(Enums.GenericParticle type)
        {
            switch (type)
            {
                case Enums.GenericParticle.Dash:
                    return _dashParticle;
                
                case Enums.GenericParticle.Landing:
                    return _landingParticle;
                
                case Enums.GenericParticle.TakeHit:
                    return _takeHit;
                
                case Enums.GenericParticle.AttackMarker:
                    return _attackMarker;
                
                case Enums.GenericParticle.MidAirJump:
                    return _midAirJump;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}