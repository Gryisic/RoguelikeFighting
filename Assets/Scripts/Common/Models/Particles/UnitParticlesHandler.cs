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
        private readonly Dictionary<int, LocalParticlesData> _particlesMap;

        public UnitParticlesHandler(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            _factory = new ParticlesFactory();
            _particlesMap = new Dictionary<int, LocalParticlesData>();
            
            Initialize(parent, particlesData);
        }
        
        private void Initialize(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            for (var i = 0; i < particlesData.Count; i++)
            {
                IParticleData data = particlesData[i];

                if (_particlesMap.ContainsKey(data.ID))
                    continue;
                
                ParticleSystem particle = _factory.Create(data.ParticleForCopy, parent);
                List<ParticleSystem> childs = new List<ParticleSystem>();
                
                if (particle.transform.childCount > 0)
                    childs.AddRange(particle.GetComponentsInChildren<ParticleSystem>());

                LocalParticlesData localData = new LocalParticlesData(particle, childs);

                _particlesMap.Add(data.ID, localData);
            }
        }

        public ParticleSystem GetParticle(int id)
        {
            if (_particlesMap.ContainsKey(id) == false)
                throw new NullReferenceException($"Particle with ID {id} isn't presented in particles map");

            return _particlesMap[id].ParticleSystem;
        }

        public IReadOnlyList<ParticleSystem> GetChildsOfParticle(int id)
        {
            if (_particlesMap.ContainsKey(id) == false)
                throw new NullReferenceException($"Particle with ID {id} isn't presented in particles map");

            return _particlesMap[id].Childs;
        }

        private struct LocalParticlesData
        {
            
            public ParticleSystem ParticleSystem { get; }
            public IReadOnlyList<ParticleSystem> Childs { get; }
            
            public LocalParticlesData(ParticleSystem particleSystem, IReadOnlyList<ParticleSystem> childs)
            {
                ParticleSystem = particleSystem;
                Childs = childs;
            }
        }
    }
}