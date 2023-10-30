using System.Collections.Generic;
using Common.Models.Particles.Interfaces;
using DG.Tweening;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Particles
{
    public class UnitParticlesPlayer
    {
        private UnitParticlesHandler _handler;
        
        public void Initialize(Transform parent, IReadOnlyList<IParticleData> particlesData, GenericParticlesData genericParticlesData)
        {
            _handler = new UnitParticlesHandler(parent, particlesData, genericParticlesData);
        }
        
        public void Play(IParticleData data, float faceDirection)
        {
            ParticleSystem particleSystem = _handler.GetParticle(data.ID);
            ParticleSystem.MainModule particleSystemMain = particleSystem.main;
            
            SetRotation(particleSystemMain, faceDirection);

            if (particleSystem.transform.childCount > 0)
            {
                IReadOnlyList<ParticleSystem> childs = _handler.GetChildsOfParticle(data.ID);

                foreach (var child in childs) 
                    SetRotation(child.main, faceDirection);
            }

            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }
        
        public void PlayGenericParticle(Enums.GenericParticle particle, float faceDirection, bool isFreezedOnPosition = true)
        {
            ParticleSystem particleSystem = _handler.GetParticle(particle);
            ParticleSystem.MainModule particleSystemMain = particleSystem.main;

            if (particleSystem == null)
                return;

            SetRotation(particleSystemMain, faceDirection);
            
            if (isFreezedOnPosition == false)
                UnfreezePosition(particleSystem);

            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }

        private void UnfreezePosition(ParticleSystem particleSystem)
        {
            Transform transform = particleSystem.transform;
            Vector3 initialPosition = transform.position;
            Vector3 initialLocalPosition = transform.localPosition;

            DOTween.Sequence()
                .Append(particleSystem.transform.DOMove(initialPosition, particleSystem.main.duration))
                .Append(particleSystem.transform.DOLocalMove(initialLocalPosition, 0));
        }

        private void SetRotation(ParticleSystem.MainModule particle, float faceDirection)
        {
            particle.startRotation3D = true;
            particle.startRotationY = faceDirection > 0 ? 0 : 180 * Mathf.Deg2Rad;
        }
    }
}