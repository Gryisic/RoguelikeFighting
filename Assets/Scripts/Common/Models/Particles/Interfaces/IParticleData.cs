using UnityEngine;

namespace Common.Models.Particles.Interfaces
{
    public interface IParticleData
    {
        int ID { get; }
        ParticleSystem ParticleForCopy { get; }
    }
}