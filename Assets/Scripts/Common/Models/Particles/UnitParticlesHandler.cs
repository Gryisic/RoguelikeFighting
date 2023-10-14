using System;
using System.Collections.Generic;
using Common.Models.Particles.Interfaces;
using Infrastructure.Factories.ParticleFactory;
using Infrastructure.Factories.ParticleFactory.Interfaces;
using UnityEngine;

namespace Common.Models.Particles
{
    public class UnitParticlesHandler
    {
        private readonly IParticlesFactory _factory;
        private readonly Dictionary<int, ParticleSystem> _particlesMap;

        public UnitParticlesHandler(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            _factory = new ParticlesFactory();
            _particlesMap = new Dictionary<int, ParticleSystem>();
            
            Initialize(parent, particlesData);
        }
        
        private void Initialize(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            for (var i = 0; i < particlesData.Count; i++)
            {
                var data = particlesData[i];
                ParticleSystem particle = _factory.Create(data.ParticleForCopy, parent);

                _particlesMap.Add(data.ID, particle);
            }
        }

        public ParticleSystem GetParticle(int id)
        {
            if (_particlesMap.ContainsKey(id) == false)
                throw new NullReferenceException($"Particle with ID {id} isn't presented in particles map");

            return _particlesMap[id];
        }
    }
}