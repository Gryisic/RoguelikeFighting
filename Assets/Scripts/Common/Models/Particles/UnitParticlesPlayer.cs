using System.Collections.Generic;
using Common.Models.Particles.Interfaces;
using UnityEngine;

namespace Common.Models.Particles
{
    public class UnitParticlesPlayer
    {
        private readonly UnitParticlesHandler _handler;
        
        public UnitParticlesPlayer(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            _handler = new UnitParticlesHandler(parent, particlesData);
        }
        
        public void Play(IParticleData data)
        {
            ParticleSystem particleSystem = _handler.GetParticle(data.ID);
            ParticleSystem.MainModule particleSystemMain = particleSystem.main;
            
            particleSystemMain.startRotationMultiplier = data.Rotation;

            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }
    }
}