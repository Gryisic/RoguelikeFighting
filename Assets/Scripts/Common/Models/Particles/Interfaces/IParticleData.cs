using UnityEngine;

namespace Common.Models.Particles.Interfaces
{
    public interface IParticleData
    {
        int ID { get; }
        float Rotation { get; }
        ParticleSystem ParticleForCopy { get; }
    }
}