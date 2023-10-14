using Common.Models.Particles.Interfaces;
using UnityEngine;

namespace Common.Models.Particles
{
    public class ParticleData : IParticleData
    {
        public int ID { get; }
        public float Rotation { get; }
        public ParticleSystem ParticleForCopy { get; }
        
        public ParticleData(ParticleSystem particleForCopy, int id, float rotation)
        {
            ParticleForCopy = particleForCopy;
            ID = id;
            Rotation = rotation;
        }
    }
}