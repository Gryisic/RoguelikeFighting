using System.Collections.Generic;
using Common.Models.Particles.Interfaces;
using UnityEngine;

namespace Common.Models.Particles
{
    public class UnitParticlesPlayer
    {
        private UnitParticlesHandler _handler;

        public void Initialize(Transform parent, IReadOnlyList<IParticleData> particlesData)
        {
            _handler = new UnitParticlesHandler(parent, particlesData);
        }
        
        public void Play(IParticleData data, float faceDirection)
        {
            ParticleSystem particleSystem = _handler.GetParticle(data.ID);
            ParticleSystem.MainModule particleSystemMain = particleSystem.main;
            
            SetRotation(particleSystemMain, faceDirection);

            if (particleSystem.transform.childCount > 0)
            {
                IReadOnlyList<ParticleSystem> childs = _handler.GetChildsOfParticle(data.ID);

                for (var i = 0; i < childs.Count; i++)
                {
                    ParticleSystem child = childs[i];
                    
                    SetRotation(child.main, faceDirection);
                }
            }
            
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }

        private void SetRotation(ParticleSystem.MainModule particle, float faceDirection)
        {
            particle.startRotation3D = true;
            particle.startRotationY = faceDirection > 0 ? 0 : 180 * Mathf.Deg2Rad;
        }
    }
}