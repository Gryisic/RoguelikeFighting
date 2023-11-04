using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Particles.Interfaces;
using Infrastructure.Factories.ParticleFactory;
using Infrastructure.Factories.ParticleFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Particles
{
    public class UnitParticlesHandler
    {
        private readonly IParticlesFactory _factory;
        private readonly Dictionary<int, LocalParticlesData> _particlesMap;
        private readonly Dictionary<Enums.GenericParticle, ParticleSystem> _genericParticlesMap;

        public UnitParticlesHandler(Transform parent, IReadOnlyList<IParticleData> particlesData, GenericParticlesData genericParticlesData)
        {
            _factory = new ParticlesFactory();
            _particlesMap = new Dictionary<int, LocalParticlesData>();
            _genericParticlesMap = new Dictionary<Enums.GenericParticle, ParticleSystem>();
            
            Initialize(parent, particlesData, genericParticlesData);
        }
        
        private void Initialize(Transform parent, IReadOnlyList<IParticleData> particlesData, GenericParticlesData genericParticlesData)
        {
            FillParticlesMap(parent, particlesData);
            FillGenericParticlesMap(parent, genericParticlesData);
        }

        public ParticleSystem GetParticle(int id)
        {
            if (_particlesMap.ContainsKey(id) == false)
                throw new NullReferenceException($"Particle with ID {id} isn't presented in particles map");

            return _particlesMap[id].ParticleSystem;
        }
        
        public ParticleSystem GetParticle(Enums.GenericParticle particle)
        {
            if (_genericParticlesMap.ContainsKey(particle) == false)
                throw new NullReferenceException($"Particle with ID {particle} isn't presented in generic particles map");

            return _genericParticlesMap[particle];
        }

        public IReadOnlyList<ParticleSystem> GetChildsOfParticle(int id)
        {
            if (_particlesMap.ContainsKey(id) == false)
                throw new NullReferenceException($"Particle with ID {id} isn't presented in particles map");

            return _particlesMap[id].Childs;
        }

        private void FillParticlesMap(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            for (var i = 0; i < particlesData.Count; i++)
            {
                IParticleData data = particlesData[i];

                if (_particlesMap.ContainsKey(data.ID))
                    continue;
                
                ParticleSystem particle = _factory.Create(data.ParticleForCopy, parent);
                List<ParticleSystem> childs = new List<ParticleSystem>();

                particle.gameObject.SetActive(false);
                
                if (particle.transform.childCount > 0)
                    childs.AddRange(particle.GetComponentsInChildren<ParticleSystem>());

                LocalParticlesData localData = new LocalParticlesData(particle, childs);

                _particlesMap.Add(data.ID, localData);
            }
        }
        
        private void FillGenericParticlesMap(Transform parent, GenericParticlesData genericParticlesData)
        {
            IReadOnlyList<Enums.GenericParticle> types = Enum.GetValues(typeof(Enums.GenericParticle))
                .Cast<Enums.GenericParticle>().ToList();

            for (var i = 0; i < types.Count; i++)
            {
                Enums.GenericParticle type = types[i];
                ParticleSystem copy = genericParticlesData.GetParticleOfType(type);

                _genericParticlesMap.Add(type, copy);
            }
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